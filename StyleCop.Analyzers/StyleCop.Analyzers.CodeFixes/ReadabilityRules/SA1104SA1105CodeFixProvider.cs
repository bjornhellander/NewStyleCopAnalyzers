﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.ReadabilityRules
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Composition;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeActions;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.CSharp;
    using StyleCop.Analyzers.Helpers;

    /// <summary>
    /// This class provides a code fix for the SA1104 diagnostic.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SA1104SA1105CodeFixProvider))]
    [Shared]
    internal class SA1104SA1105CodeFixProvider : CodeFixProvider
    {
        /// <inheritdoc/>
        public override ImmutableArray<string> FixableDiagnosticIds { get; } =
            ImmutableArray.Create(
                SA110xQueryClauses.SA1104Descriptor.Id,
                SA110xQueryClauses.SA1105Descriptor.Id);

        /// <inheritdoc/>
        public override FixAllProvider GetFixAllProvider()
        {
            return CustomFixAllProviders.BatchFixer;
        }

        /// <inheritdoc/>
        public override Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            foreach (var diagnostic in context.Diagnostics)
            {
                context.RegisterCodeFix(
                    CodeAction.Create(
                        ReadabilityResources.SA1104SA1105CodeFix,
                        cancellationToken => GetTransformedDocumentAsync(context.Document, diagnostic, cancellationToken),
                        nameof(SA1104SA1105CodeFixProvider)),
                    diagnostic);
            }

            return SpecializedTasks.CompletedTask;
        }

        private static async Task<Document> GetTransformedDocumentAsync(Document document, Diagnostic diagnostic, CancellationToken cancellationToken)
        {
            var syntaxRoot = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            var token = syntaxRoot.FindToken(diagnostic.Location.SourceSpan.Start);

            var settings = SettingsHelper.GetStyleCopSettingsInCodeFix(document.Project.AnalyzerOptions, syntaxRoot.SyntaxTree, cancellationToken);
            var indentationTrivia = QueryIndentationHelpers.GetQueryIndentationTrivia(settings.Indentation, token);

            var precedingToken = token.GetPreviousToken();
            var triviaList = precedingToken.TrailingTrivia.AddRange(token.LeadingTrivia);
            var processedTriviaList = triviaList.WithoutTrailingWhitespace().Add(SyntaxFactory.CarriageReturnLineFeed);

            var replaceMap = new Dictionary<SyntaxToken, SyntaxToken>()
            {
                [precedingToken] = precedingToken.WithTrailingTrivia(processedTriviaList),
                [token] = token.WithLeadingTrivia(indentationTrivia),
            };

            var newSyntaxRoot = syntaxRoot.ReplaceTokens(replaceMap.Keys, (t1, t2) => replaceMap[t1]).WithoutFormatting();
            return document.WithSyntaxRoot(newSyntaxRoot);
        }
    }
}
