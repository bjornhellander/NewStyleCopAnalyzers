// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.OrderingRules
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Composition;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using StyleCop.Analyzers.Helpers;
    using StyleCop.Analyzers.Lightup;
    using static StyleCop.Analyzers.OrderingRules.ModifierOrderHelper;

    /// <summary>
    /// Implements code fixes for element ordering rules.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SA1206CodeFixProvider))]
    [Shared]
    internal sealed class SA1206CodeFixProvider : CodeFixProvider
    {
        /// <inheritdoc/>
        public override ImmutableArray<string> FixableDiagnosticIds { get; } =
            ImmutableArray.Create(SA1206DeclarationKeywordsMustFollowOrder.DiagnosticId);

        /// <inheritdoc/>
        public override FixAllProvider GetFixAllProvider()
        {
            return FixAll.Instance;
        }

        /// <inheritdoc/>
        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            foreach (Diagnostic diagnostic in context.Diagnostics)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        OrderingResources.ModifierOrderCodeFix,
                        cancellationToken => GetTransformedDocumentAsync(context.Document, diagnostic, cancellationToken),
                        nameof(SA1206CodeFixProvider)),
                    diagnostic);
            }

            return SpecializedTasks.CompletedTask;
        }

        private static async Task<Document> GetTransformedDocumentAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
        {
            var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            var declaration = FindDeclaration(syntaxRoot, diagnostic);
            if (declaration == null)
            {
                return document;
            }

            var modifierTokenToFix = declaration.FindToken(diagnostic.Location.SourceSpan.Start);
            if (GetModifierType(modifierTokenToFix) == ModifierType.None)
            {
                return document;
            }

            var newModifierList = PartiallySortModifiers(DeclarationModifiersHelper.GetModifiers(declaration), modifierTokenToFix);
            syntaxRoot = UpdateSyntaxRoot(declaration, newModifierList, syntaxRoot);

            return document.WithSyntaxRoot(syntaxRoot);
        }

        /// <summary>
        /// Finds the declaration a diagnostic was reported on. A local function is a statement rather than a member
        /// declaration, so it cannot be found by looking for a <see cref="MemberDeclarationSyntax"/> alone.
        /// </summary>
        /// <param name="syntaxRoot">The root of the syntax tree.</param>
        /// <param name="diagnostic">The diagnostic to find the declaration for.</param>
        /// <returns>The declaration, or <see langword="null"/> if none was found.</returns>
        private static SyntaxNode FindDeclaration(SyntaxNode syntaxRoot, Diagnostic diagnostic)
        {
            return syntaxRoot.FindNode(diagnostic.Location.SourceSpan)
                .AncestorsAndSelf()
                .FirstOrDefault(node => node is MemberDeclarationSyntax || LocalFunctionStatementSyntaxWrapper.IsInstance(node));
        }

        private static SyntaxNode UpdateSyntaxRoot(SyntaxNode declaration, SyntaxTokenList newModifiers, SyntaxNode syntaxRoot)
        {
            var newDeclaration = DeclarationModifiersHelper.WithModifiers(declaration, newModifiers);
            return syntaxRoot.ReplaceNode(declaration, newDeclaration);
        }

        /// <summary>
        /// Sorts the complete modifier list to fix all issues.
        /// The trivia will be maintained positionally.
        /// The relative order within the different kinds <seealso cref="ModifierType"/> will not be
        /// changed.
        /// </summary>
        /// <param name="modifiers">All modifiers from the declaration.</param>
        /// <returns>A fully sorted modifier list.</returns>
        private static SyntaxTokenList FullySortModifiers(SyntaxTokenList modifiers)
        {
            var accessModifiers = modifiers.Where(modifier => GetModifierType(modifier) == ModifierType.Access);
            var staticModifiers = modifiers.Where(modifier => GetModifierType(modifier) == ModifierType.Static);
            var otherModifiers = modifiers.Where(modifier => GetModifierType(modifier) == ModifierType.Other);

            return AdjustTrivia(
                accessModifiers
                    .Concat(staticModifiers)
                    .Concat(otherModifiers),
                modifiers);
        }

        /// <summary>
        /// Sorts the modifier list to fix all issues before <paramref name="modifierToFix"/>
        /// and keep the remaining modifiers untouched.
        /// The trivia will be maintained positionally.
        /// The relative order within the different kinds <seealso cref="ModifierType"/> will not be
        /// changed.
        /// </summary>
        /// <param name="modifiers">All modifiers from the declaration.</param>
        /// <param name="modifierToFix">The modifier with diagnostics.</param>
        /// <returns>A partially sorted modifier list (sorted up to <paramref name="modifierToFix"/>).</returns>
        private static SyntaxTokenList PartiallySortModifiers(SyntaxTokenList modifiers, SyntaxToken modifierToFix)
        {
            var accessModifiers = modifiers.Where(modifier => GetModifierType(modifier) == ModifierType.Access);
            var staticModifiers = modifiers.Where(modifier => GetModifierType(modifier) == ModifierType.Static);
            var otherModifiers = modifiers.Where(modifier => GetModifierType(modifier) == ModifierType.Other);

            IEnumerable<SyntaxToken> beforeIncluding;

            // the modifier to fix is of type other, so we need to sort the whole list of
            // modifier list
            if (GetModifierType(modifierToFix) == ModifierType.Other)
            {
                beforeIncluding = accessModifiers
                    .Concat(staticModifiers)
                    .Concat(otherModifiers);
            }
            else if (GetModifierType(modifierToFix) == ModifierType.Static)
            {
                beforeIncluding = accessModifiers
                    .Concat(staticModifiers.TakeWhile(modifier => modifier != modifierToFix))
                    .Concat(new[] { modifierToFix });
            }
            else
            {
                beforeIncluding = accessModifiers
                    .TakeWhile(modifier => modifier != modifierToFix)
                    .Concat(new[] { modifierToFix });
            }

            var after = modifiers.Where(modifier => !beforeIncluding.Contains(modifier));

            return AdjustTrivia(beforeIncluding.Concat(after), modifiers);
        }

        /// <summary>
        /// Positionally apply the trivia from the old modifier list to the new one.
        /// </summary>
        /// <param name="newModifiers">The new modifiers.</param>
        /// <param name="oldModifiers">The old modifiers.</param>
        /// <returns>New modifier list with trivia from the old one.</returns>
        private static SyntaxTokenList AdjustTrivia(IEnumerable<SyntaxToken> newModifiers, SyntaxTokenList oldModifiers)
        {
            var newTokenList = default(SyntaxTokenList);
            return newTokenList.AddRange(
                newModifiers.Zip(oldModifiers, (m1, m2) => m1.WithTriviaFrom(m2)));
        }

        private class FixAll : DocumentBasedFixAllProvider
        {
            public static FixAllProvider Instance { get; } = new FixAll();

            protected override string CodeActionTitle => OrderingResources.ModifierOrderCodeFix;

            protected override async Task<SyntaxNode?> FixAllInDocumentAsync(FixAllContext fixAllContext, Document document, ImmutableArray<Diagnostic> diagnostics)
            {
                if (diagnostics.IsEmpty)
                {
                    return null;
                }

                var syntaxRoot = await document.GetSyntaxRootAsync().ConfigureAwait(false);

                // because all modifiers can be fixed in one run, we
                // only need to store each declaration once
                var trackedDiagnosticMembers = new HashSet<SyntaxNode>();
                foreach (var diagnostic in diagnostics)
                {
                    var declaration = FindDeclaration(syntaxRoot, diagnostic);
                    if (declaration == null)
                    {
                        continue;
                    }

                    var modifierToken = declaration.FindToken(diagnostic.Location.SourceSpan.Start);
                    if (GetModifierType(modifierToken) == ModifierType.None)
                    {
                        continue;
                    }

                    trackedDiagnosticMembers.Add(declaration);
                }

                syntaxRoot = syntaxRoot.TrackNodes(trackedDiagnosticMembers);

                foreach (var member in trackedDiagnosticMembers)
                {
                    var declaration = syntaxRoot.GetCurrentNode(member);
                    var newModifierList = FullySortModifiers(DeclarationModifiersHelper.GetModifiers(declaration));
                    syntaxRoot = UpdateSyntaxRoot(declaration, newModifierList, syntaxRoot);
                }

                return syntaxRoot;
            }
        }
    }
}
