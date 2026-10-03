// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.ReadabilityRules
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// The C# code contains a region within the body of a code element.
    /// </summary>
    /// <remarks>
    /// <para>A violation of this rule occurs whenever a region is placed within the body of a code element. In many
    /// editors, including Visual Studio, the region will appear collapsed by default, hiding the code within the
    /// region. It is generally a bad practice to hide code within the body of an element, as this can lead to bad
    /// decisions as the code is maintained over time.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SA1123DoNotPlaceRegionsWithinElements : DiagnosticAnalyzerBase
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1123DoNotPlaceRegionsWithinElements"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1123";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(ReadabilityResources.SA1123Title), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(ReadabilityResources.SA1123MessageFormat), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(ReadabilityResources.SA1123Description), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));

        private static readonly DiagnosticDescriptor Descriptor =
            CreateDiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.ReadabilityRules, Description);

        private static readonly Action<SyntaxNodeAnalysisContext> RegionDirectiveTriviaAction = HandleRegionDirectiveTrivia;

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        protected override void HandleCompilationStart(CompilationStartAnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(RegionDirectiveTriviaAction, SyntaxKind.RegionDirectiveTrivia);
        }

        /// <summary>
        /// Checks if a region is completely part of a body. That means that the <c>#region</c> and <c>#endregion</c>
        /// tags both have to have a common <see cref="BlockSyntax"/> as one of their ancestors.
        /// </summary>
        /// <param name="regionSyntax">The <see cref="RegionDirectiveTriviaSyntax"/> that should be analyzed.</param>
        /// <returns><see langword="true"/>, if both tags have a common <see cref="BlockSyntax"/> as one of their
        /// ancestors; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="regionSyntax"/> is <see langword="null"/>.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:Elements should be ordered by access", Justification = "Ok")]
        internal static bool IsCompletelyContainedInBody(RegionDirectiveTriviaSyntax regionSyntax)
        {
            if (regionSyntax == null)
            {
                throw new ArgumentNullException(nameof(regionSyntax));
            }

            SyntaxNode? syntax = null;
            foreach (var directive in regionSyntax.GetRelatedDirectives())
            {
                SyntaxNode? blockOrCompilationUnitSyntax = (SyntaxNode?)directive.AncestorsAndSelf().OfType<BlockSyntax>().LastOrDefault()
                    ?? GetTopLevelStatementsBody(directive);
                if (blockOrCompilationUnitSyntax == null)
                {
                    return false;
                }
                else if (syntax == null)
                {
                    syntax = blockOrCompilationUnitSyntax;
                }
                else if (blockOrCompilationUnitSyntax != syntax)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the compilation unit if the directive is located among top-level statements, which form the body of the
        /// implicit entry point.
        /// </summary>
        /// <param name="directive">The directive.</param>
        /// <returns>The compilation unit, or <see langword="null"/> if the directive is not located among top-level
        /// statements.</returns>
        private static CompilationUnitSyntax? GetTopLevelStatementsBody(DirectiveTriviaSyntax directive)
        {
            var token = directive.ParentTrivia.Token;
            var compilationUnit = token.Parent?.FirstAncestorOrSelf<CompilationUnitSyntax>();
            if (compilationUnit == null)
            {
                // Should never happen
                return null;
            }

            // A directive before a top-level statement belongs to the first token of that statement. A directive within
            // a top-level statement (without a block) is not located among the top-level statements, just like a
            // directive within an expression-bodied method is not located within a body.
            var globalStatement = token.Parent!.FirstAncestorOrSelf<GlobalStatementSyntax>();
            if (globalStatement != null && globalStatement.GetFirstToken() == token)
            {
                return compilationUnit;
            }

            // A directive after the last top-level statement in the file belongs to the end-of-file token
            if (token.IsKind(SyntaxKind.EndOfFileToken)
                && compilationUnit.Members.LastOrDefault() is { } lastMember
                && lastMember.IsKind(SyntaxKind.GlobalStatement))
            {
                return compilationUnit;
            }

            return null;
        }

        private static void HandleRegionDirectiveTrivia(SyntaxNodeAnalysisContext context)
        {
            RegionDirectiveTriviaSyntax regionSyntax = (RegionDirectiveTriviaSyntax)context.Node;

            if (IsCompletelyContainedInBody(regionSyntax))
            {
                // Region should not be located within a code element.
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, regionSyntax.GetLocation()));
            }
        }
    }
}
