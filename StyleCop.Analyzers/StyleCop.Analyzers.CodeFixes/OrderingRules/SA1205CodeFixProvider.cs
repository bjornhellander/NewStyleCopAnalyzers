﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.OrderingRules
{
    using System.Collections.Immutable;
    using System.Composition;
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
    /// Implements code fixes for <see cref="SA1205PartialElementsMustDeclareAccess"/>.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SA1205CodeFixProvider))]
    [Shared]
    internal class SA1205CodeFixProvider : CodeFixProvider
    {
        private static readonly ImmutableArray<SyntaxKind> PublicAccessibilityKeywords = ImmutableArray.Create(SyntaxKind.PublicKeyword);
        private static readonly ImmutableArray<SyntaxKind> InternalAccessibilityKeywords = ImmutableArray.Create(SyntaxKind.InternalKeyword);
        private static readonly ImmutableArray<SyntaxKind> ProtectedAccessibilityKeywords = ImmutableArray.Create(SyntaxKind.ProtectedKeyword);
        private static readonly ImmutableArray<SyntaxKind> ProtectedOrInternalAccessibilityKeywords = ImmutableArray.Create(SyntaxKind.ProtectedKeyword, SyntaxKind.InternalKeyword);
        private static readonly ImmutableArray<SyntaxKind> ProtectedAndInternalAccessibilityKeywords = ImmutableArray.Create(SyntaxKind.PrivateKeyword, SyntaxKind.ProtectedKeyword);
        private static readonly ImmutableArray<SyntaxKind> PrivateAccessibilityKeywords = ImmutableArray.Create(SyntaxKind.PrivateKeyword);
        private static readonly ImmutableArray<SyntaxKind> UnexpectedAccessibilityKeywords = ImmutableArray.Create<SyntaxKind>();

        /// <inheritdoc/>
        public override ImmutableArray<string> FixableDiagnosticIds { get; } =
            ImmutableArray.Create(SA1205PartialElementsMustDeclareAccess.DiagnosticId);

        /// <inheritdoc/>
        public override FixAllProvider GetFixAllProvider()
        {
            return CustomFixAllProviders.BatchFixer;
        }

        /// <inheritdoc/>
        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            foreach (Diagnostic diagnostic in context.Diagnostics)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        OrderingResources.SA1205CodeFix,
                        cancellationToken => GetTransformedDocumentAsync(context.Document, diagnostic, cancellationToken),
                        nameof(SA1205CodeFixProvider)),
                    diagnostic);
            }

            return SpecializedTasks.CompletedTask;
        }

        private static async Task<Document> GetTransformedDocumentAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
        {
            var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            if (!(syntaxRoot.FindNode(diagnostic.Location.SourceSpan) is TypeDeclarationSyntax typeDeclarationNode))
            {
                return document;
            }

            var symbol = semanticModel.GetDeclaredSymbol(typeDeclarationNode);
            var accessModifierKinds = GetMissingAccessModifiers(symbol.DeclaredAccessibility);

            var keywordToken = typeDeclarationNode.Keyword;

            var replacementModifiers = DeclarationModifiersHelper.AddModifiers(typeDeclarationNode.Modifiers, ref keywordToken, accessModifierKinds);
            var replacementNode = ReplaceModifiers(typeDeclarationNode, replacementModifiers);
            replacementNode = ReplaceKeyword(replacementNode, keywordToken);
            var newSyntaxRoot = syntaxRoot.ReplaceNode(typeDeclarationNode, replacementNode);
            return document.WithSyntaxRoot(newSyntaxRoot);
        }

        private static ImmutableArray<SyntaxKind> GetMissingAccessModifiers(Accessibility accessibility)
        {
            switch (accessibility)
            {
            case Accessibility.Public:
                return PublicAccessibilityKeywords;
            case Accessibility.Internal:
                return InternalAccessibilityKeywords;
            case Accessibility.Protected:
                return ProtectedAccessibilityKeywords;
            case Accessibility.ProtectedOrInternal:
                return ProtectedOrInternalAccessibilityKeywords;
            case Accessibility.ProtectedAndInternal:
                return ProtectedAndInternalAccessibilityKeywords;
            case Accessibility.Private:
                return PrivateAccessibilityKeywords;
            default:
                // This should not happen!
                return UnexpectedAccessibilityKeywords;
            }
        }

        // This code was copied from the Roslyn code base (and slightly modified). It can be removed if
        // TypeDeclarationSyntaxExtensions.WithModifiers is made public (Roslyn issue #2186)
        private static TypeDeclarationSyntax ReplaceModifiers(TypeDeclarationSyntax node, SyntaxTokenList modifiers)
        {
            switch (node.Kind())
            {
            case SyntaxKind.ClassDeclaration:
                return ((ClassDeclarationSyntax)node).WithModifiers(modifiers);
            case SyntaxKind.InterfaceDeclaration:
                return ((InterfaceDeclarationSyntax)node).WithModifiers(modifiers);
            case SyntaxKind.StructDeclaration:
                return ((StructDeclarationSyntax)node).WithModifiers(modifiers);
            case SyntaxKindEx.RecordDeclaration:
            case SyntaxKindEx.RecordStructDeclaration:
                return ((RecordDeclarationSyntaxWrapper)node).WithModifiers(modifiers);
            }

            return node;
        }

        // This code was copied from the Roslyn code base (and slightly modified). It can be removed if
        // TypeDeclarationSyntaxExtensions.WithModifiers is made public (Roslyn issue #2186)
        private static TypeDeclarationSyntax ReplaceKeyword(TypeDeclarationSyntax node, SyntaxToken keyword)
        {
            switch (node.Kind())
            {
            case SyntaxKind.ClassDeclaration:
                return ((ClassDeclarationSyntax)node).WithKeyword(keyword);
            case SyntaxKind.InterfaceDeclaration:
                return ((InterfaceDeclarationSyntax)node).WithKeyword(keyword);
            case SyntaxKind.StructDeclaration:
                return ((StructDeclarationSyntax)node).WithKeyword(keyword);
            case SyntaxKindEx.RecordDeclaration:
            case SyntaxKindEx.RecordStructDeclaration:
                return ((RecordDeclarationSyntaxWrapper)node).WithKeyword(keyword);
            }

            return node;
        }
    }
}
