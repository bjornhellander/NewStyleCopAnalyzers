// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.MaintainabilityRules
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Composition;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using StyleCop.Analyzers.Helpers;
    using StyleCop.Analyzers.Lightup;

    /// <summary>
    /// Implements a code fix for <see cref="SA1402FileMayOnlyContainASingleType"/>.
    /// </summary>
    /// <remarks>
    /// <para>To fix a violation of this rule, move each type into its own file.</para>
    /// </remarks>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SA1402CodeFixProvider))]
    [Shared]
    internal class SA1402CodeFixProvider : CodeFixProvider
    {
        /// <inheritdoc/>
        public override ImmutableArray<string> FixableDiagnosticIds { get; } =
            ImmutableArray.Create(SA1402FileMayOnlyContainASingleType.DiagnosticId);

        /// <inheritdoc/>
        public override FixAllProvider GetFixAllProvider()
        {
            // The batch fixer can't handle code fixes that create new files
            return null;
        }

        /// <inheritdoc/>
        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            foreach (var diagnostic in context.Diagnostics)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        MaintainabilityResources.SA1402CodeFix,
                        cancellationToken => GetTransformedSolutionAsync(context.Document, diagnostic, cancellationToken),
                        nameof(SA1402CodeFixProvider)),
                    diagnostic);
            }

            return SpecializedTasks.CompletedTask;
        }

        private static async Task<Solution> GetTransformedSolutionAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            SyntaxNode node = root.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true);
            if (!(node is MemberDeclarationSyntax memberDeclarationSyntax))
            {
                return document.Project.Solution;
            }

            DocumentId extractedDocumentId = DocumentId.CreateNewId(document.Project.Id);
            string suffix;
            FileNameHelpers.GetFileNameAndSuffix(document.Name, out suffix);
            var settings = document.Project.AnalyzerOptions.GetStyleCopSettingsInCodeFix(root.SyntaxTree, cancellationToken);
            string extractedDocumentName = FileNameHelpers.GetConventionalFileName(memberDeclarationSyntax, settings.DocumentationRules.FileNamingConvention) + suffix;

            List<SyntaxNode> nodesToRemoveFromExtracted = new List<SyntaxNode>();
            SyntaxNode previous = node;
            for (SyntaxNode current = node.Parent; current != null; previous = current, current = current.Parent)
            {
                foreach (SyntaxNode child in current.ChildNodes())
                {
                    if (child == previous)
                    {
                        continue;
                    }

                    switch (child.Kind())
                    {
                    case SyntaxKind.NamespaceDeclaration:
                    case SyntaxKind.ClassDeclaration:
                    case SyntaxKind.StructDeclaration:
                    case SyntaxKind.InterfaceDeclaration:
                    case SyntaxKind.EnumDeclaration:
                    case SyntaxKind.DelegateDeclaration:
                    case SyntaxKindEx.RecordDeclaration:
                    case SyntaxKindEx.RecordStructDeclaration:
                        nodesToRemoveFromExtracted.Add(child);
                        break;

                    case SyntaxKindEx.FileScopedNamespaceDeclaration:
                        // Only one file-scoped namespace is allowed per syntax tree
                        throw new InvalidOperationException("This location is not reachable");

                    default:
                        break;
                    }
                }
            }

            // Add the new file
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            var compilationUnit = (CompilationUnitSyntax)root;

            // These are usings outside of the namespace
            var allUsings = compilationUnit.Usings;

            // Usings from within namespace
            //foreach (var member in compilationUnit.Members.OfType<NamespaceDeclarationSyntax>())
            //{
            //    allUsings = allUsings.AddRange(member.Usings);
            //}

            // Gather all symbols referenced by the type being moved
            TypeDeclarationSyntax typeDeclaration =
                memberDeclarationSyntax as TypeDeclarationSyntax
                ?? memberDeclarationSyntax.DescendantNodes().OfType<TypeDeclarationSyntax>().FirstOrDefault();

            Debug.Assert(typeDeclaration.SyntaxTree == root.SyntaxTree, "The type declaration's syntax tree should match the root syntax tree.");

            if (typeDeclaration == null)
            {
                return document.Project.Solution;
            }

            var walker = new ReferencedSymbolsWalker(semanticModel);
            walker.Visit(typeDeclaration);
            var referencedSymbols = walker.ReferencedSymbols;

            var neededUsings = new List<UsingDirectiveSyntax>();
            foreach (var usingDirective in allUsings)
            {
                if (IsUsingNeeded(usingDirective, referencedSymbols, semanticModel, typeDeclaration))
                {
                    neededUsings.Add(usingDirective);
                }
            }

            // Build the new CompilationUnit for the extracted document
            var originalNamespace = typeDeclaration.Parent as NamespaceDeclarationSyntax;

            MemberDeclarationSyntax newTypeParented = typeDeclaration;
            if (originalNamespace != null)
            {
                // If the type is inside a namespace, we need to create a new namespace declaration
                newTypeParented = SyntaxFactory.NamespaceDeclaration(originalNamespace.Name)
                    .WithMembers(SyntaxFactory.SingletonList<MemberDeclarationSyntax>(typeDeclaration))
                    .WithLeadingTrivia(originalNamespace.GetLeadingTrivia())
                    .WithTrailingTrivia(originalNamespace.GetTrailingTrivia());
            }

            var newCompilationUnit = SyntaxFactory.CompilationUnit()
                    .WithUsings(SyntaxFactory.List(neededUsings))
                    .WithMembers(SyntaxFactory.SingletonList(newTypeParented))
                    .WithEndOfFileToken(compilationUnit.EndOfFileToken);

            Solution updatedSolution = document.Project.Solution.AddDocument(extractedDocumentId, extractedDocumentName, newCompilationUnit, document.Folders);

            // Make sure to also add the file to linked projects
            foreach (var linkedDocumentId in document.GetLinkedDocumentIds())
            {
                DocumentId linkedExtractedDocumentId = DocumentId.CreateNewId(linkedDocumentId.ProjectId);
                updatedSolution = updatedSolution.AddDocument(linkedExtractedDocumentId, extractedDocumentName, newCompilationUnit, document.Folders);
            }

            // Remove the type from its original location
            updatedSolution = updatedSolution.WithDocumentSyntaxRoot(document.Id, root.RemoveNode(node, SyntaxRemoveOptions.KeepUnbalancedDirectives));

            return updatedSolution;
        }

        private static bool IsUsingNeeded(UsingDirectiveSyntax usingDirective, IReadOnlyCollection<ISymbol> referencedSymbols, SemanticModel semanticModel, TypeDeclarationSyntax typeDeclaration)
        {
            // Normal using: check if any referenced symbol's namespace matches
            var namespaceSymbol = semanticModel.GetSymbolInfo(usingDirective.Name).Symbol as INamespaceSymbol;
            if (namespaceSymbol == null)
            {
                return false;
            }

            if (usingDirective.Alias != null)
            {
                // Check if the alias is used in the referenced symbols
                var aliasName = usingDirective.Alias.Name.Identifier.ValueText;
                return typeDeclaration.DescendantNodes()
                    .OfType<IdentifierNameSyntax>()
                    .Any(id => id.Identifier.ValueText == aliasName);
            }

            if (usingDirective.StaticKeyword.IsKind(SyntaxKind.StaticKeyword))
            {
                // Check if static members are used
                var staticSymbol = semanticModel.GetSymbolInfo(usingDirective.Name).Symbol;
                if (staticSymbol == null)
                {
                    return false;
                }

                // Check if any referenced symbol comes from this static type
                return referencedSymbols.Any(rs => rs.ContainingType?.Equals(staticSymbol) == true);
            }

            return referencedSymbols.Any(rs =>
            {
                var containingNamespace = rs.ContainingNamespace;
                while (containingNamespace != null && !containingNamespace.IsGlobalNamespace)
                {
                    if (containingNamespace.Equals(namespaceSymbol))
                    {
                        return true;
                    }

                    containingNamespace = containingNamespace.ContainingNamespace;
                }

                return false;
            });
        }

        private class ReferencedSymbolsWalker : CSharpSyntaxWalker
        {
            private readonly SemanticModel semanticModel;
            private readonly HashSet<ISymbol> referencedSymbols = new HashSet<ISymbol>();

            public ReferencedSymbolsWalker(SemanticModel semanticModel)
            {
                this.semanticModel = semanticModel;
            }

            public IReadOnlyCollection<ISymbol> ReferencedSymbols => this.referencedSymbols.ToList();

            public override void VisitIdentifierName(IdentifierNameSyntax node)
            {
                if (node.IsVar || node.IsMissing)
                {
                    return;
                }

                var symbol = this.semanticModel.GetSymbolInfo(node).Symbol;
                Debug.WriteLine($"Visiting: {node.Identifier.Text}, Symbol: {symbol}");
                Debug.Assert(symbol != null, $"Expected symbol for {node.Identifier.Text} to be found");
                if (symbol != null && !symbol.IsImplicitlyDeclared)
                {
                    this.referencedSymbols.Add(symbol.OriginalDefinition);
                }

                base.VisitIdentifierName(node);
            }

            public override void VisitQualifiedName(QualifiedNameSyntax node)
            {
                if (node.IsMissing)
                {
                    return;
                }

                var symbol = this.semanticModel.GetSymbolInfo(node).Symbol;
                if (symbol != null && !symbol.IsImplicitlyDeclared)
                {
                    this.referencedSymbols.Add(symbol.OriginalDefinition);
                }

                base.VisitQualifiedName(node);
            }

            public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
            {
                if (node.IsMissing)
                {
                    return;
                }

                var symbol = this.semanticModel.GetSymbolInfo(node).Symbol;
                if (symbol != null && !symbol.IsImplicitlyDeclared)
                {
                    this.referencedSymbols.Add(symbol.OriginalDefinition);
                }

                base.VisitMemberAccessExpression(node);
            }
        }
    }
}
