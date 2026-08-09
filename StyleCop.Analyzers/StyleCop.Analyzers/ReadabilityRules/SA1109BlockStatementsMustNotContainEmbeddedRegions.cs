// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.ReadabilityRules
{
    using System.Collections.Immutable;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// A C# statement contains a region tag between the declaration of the statement and the opening brace of the
    /// statement.
    /// </summary>
    /// <remarks>
    /// <para>This diagnostic is not implemented in StyleCopAnalyzers.</para>
    ///
    /// <para>A violation of this rule occurs when the code contains a region tag in between the declaration and the
    /// opening brace. For example:</para>
    /// <code language="csharp">
    /// if (x != y)
    /// #region
    /// {
    /// }
    /// #endregion
    /// </code>
    /// <para>This will result in the body of the statement being hidden when the region is collapsed.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    [NoDiagnostic("This diagnostic is rarely-occurring specialization of SA1123; the latter is now preferred in all cases.")]
    internal class SA1109BlockStatementsMustNotContainEmbeddedRegions : DiagnosticAnalyzerBase
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1109BlockStatementsMustNotContainEmbeddedRegions"/>
        /// analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1109";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(ReadabilityResources.SA1109Title), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(ReadabilityResources.SA1109MessageFormat), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(ReadabilityResources.SA1109Description), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));

        private static readonly DiagnosticDescriptor Descriptor =
            CreateDiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.ReadabilityRules, Description, isEnabledByDefault: false, customTags: new[] { WellKnownDiagnosticTags.NotConfigurable });

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        [SuppressMessage("MicrosoftCodeAnalysisPerformance", "RS1012:Start action has no registered actions", Justification = "Not implemented")]
        protected override void HandleCompilationStart(CompilationStartAnalysisContext context)
        {
            // This diagnostic is not implemented (by design) in StyleCopAnalyzers.
        }
    }
}
