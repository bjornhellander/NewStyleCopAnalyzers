﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.ReadabilityRules
{
    using System.Collections.Immutable;
    using System.Composition;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using StyleCop.Analyzers.Helpers;

    /// <summary>
    /// Implements a code fix for <see cref="SA1131UseReadableConditions"/>.
    /// </summary>
    /// <remarks>
    /// <para>To fix a violation of this rule, switch the arguments of the comparison.</para>
    /// </remarks>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SA1131CodeFixProvider))]
    [Shared]
    internal class SA1131CodeFixProvider : CodeFixProvider
    {
        /// <inheritdoc/>
        public override ImmutableArray<string> FixableDiagnosticIds { get; } =
            ImmutableArray.Create(SA1131UseReadableConditions.DiagnosticId);

        /// <inheritdoc/>
        public override FixAllProvider GetFixAllProvider()
        {
            return FixAll.Instance;
        }

        /// <inheritdoc/>
        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            foreach (var diagnostic in context.Diagnostics)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        ReadabilityResources.SA1131CodeFix,
                        cancellationToken => GetTransformedDocumentAsync(context.Document, diagnostic, cancellationToken),
                        nameof(SA1131CodeFixProvider)),
                    diagnostic);
            }

            return SpecializedTasks.CompletedTask;
        }

        private static async Task<Document> GetTransformedDocumentAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
        {
            var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);

            var binaryExpression = (BinaryExpressionSyntax)syntaxRoot.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true);

            var newBinaryExpression = TransformExpression(binaryExpression);

            return document.WithSyntaxRoot(syntaxRoot.ReplaceNode(binaryExpression, newBinaryExpression));
        }

        private static BinaryExpressionSyntax TransformExpression(BinaryExpressionSyntax binaryExpression)
        {
            // NOTE: This code also changes the syntax node kind, besides the operator token. The modified source code would
            // have been the same without this, but we do this to make tests pass with the default CodeActionValidationMode.
            var newLeft = binaryExpression.Right.WithTriviaFrom(binaryExpression.Left);
            var newRight = binaryExpression.Left.WithTriviaFrom(binaryExpression.Right);
            GetReplacementInfo(binaryExpression.OperatorToken, out var newOperatorToken, out var newNodeKind);
            return SyntaxFactory.BinaryExpression(newNodeKind, newLeft, newOperatorToken, newRight);
        }

        private static void GetReplacementInfo(SyntaxToken operatorToken, out SyntaxToken newToken, out SyntaxKind newNodeKind)
        {
            switch (operatorToken.Kind())
            {
            case SyntaxKind.EqualsEqualsToken:
            case SyntaxKind.ExclamationEqualsToken:
                newToken = operatorToken;
                newNodeKind = operatorToken.Parent.Kind();
                break;

            case SyntaxKind.GreaterThanToken:
                newToken = SyntaxFactory.Token(operatorToken.LeadingTrivia, SyntaxKind.LessThanToken, operatorToken.TrailingTrivia);
                newNodeKind = SyntaxKind.LessThanExpression;
                break;

            case SyntaxKind.GreaterThanEqualsToken:
                newToken = SyntaxFactory.Token(operatorToken.LeadingTrivia, SyntaxKind.LessThanEqualsToken, operatorToken.TrailingTrivia);
                newNodeKind = SyntaxKind.LessThanOrEqualExpression;
                break;

            case SyntaxKind.LessThanToken:
                newToken = SyntaxFactory.Token(operatorToken.LeadingTrivia, SyntaxKind.GreaterThanToken, operatorToken.TrailingTrivia);
                newNodeKind = SyntaxKind.GreaterThanExpression;
                break;

            case SyntaxKind.LessThanEqualsToken:
                newToken = SyntaxFactory.Token(operatorToken.LeadingTrivia, SyntaxKind.GreaterThanEqualsToken, operatorToken.TrailingTrivia);
                newNodeKind = SyntaxKind.GreaterThanOrEqualExpression;
                break;

            default:
                newToken = SyntaxFactory.Token(SyntaxKind.None);
                newNodeKind = (SyntaxKind)operatorToken.Parent.RawKind;
                break;
            }
        }

        private class FixAll : DocumentBasedFixAllProvider
        {
            public static FixAllProvider Instance { get; } =
                new FixAll();

            protected override string CodeActionTitle =>
                ReadabilityResources.SA1131CodeFix;

            protected override async Task<SyntaxNode> FixAllInDocumentAsync(FixAllContext fixAllContext, Document document, ImmutableArray<Diagnostic> diagnostics)
            {
                if (diagnostics.IsEmpty)
                {
                    return null;
                }

                var syntaxRoot = await document.GetSyntaxRootAsync(fixAllContext.CancellationToken).ConfigureAwait(false);

                var nodes = diagnostics.Select(diagnostic => (BinaryExpressionSyntax)syntaxRoot.FindNode(diagnostic.Location.SourceSpan, getInnermostNodeForTie: true));

                return syntaxRoot.ReplaceNodes(nodes, (originalNode, rewrittenNode) => TransformExpression(rewrittenNode));
            }
        }
    }
}
