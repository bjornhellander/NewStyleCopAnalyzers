// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;

    internal abstract class DiagnosticAnalyzerBase : DiagnosticAnalyzer
    {
        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            if (ShouldEnableConcurrentExecution())
            {
                context.EnableConcurrentExecution();
            }

            context.RegisterCompilationStartAction(this.HandleCompilationStart);
        }

        protected static DiagnosticDescriptor CreateDiagnosticDescriptor(string id, LocalizableString title, LocalizableString messageFormat, string category, LocalizableString description, DiagnosticSeverity defaultSeverity = DiagnosticSeverity.Warning, bool isEnabledByDefault = true, params string[] customTags)
        {
            var helpLinkUri = $"https://github.com/bjornhellander/NewStyleCopAnalyzers/blob/master/documentation/{id}.md";

            return new DiagnosticDescriptor(
                id,
                title,
                messageFormat,
                category,
                defaultSeverity,
                isEnabledByDefault,
                description,
                helpLinkUri,
                customTags);
        }

        /// <summary>
        /// Called at the start of each compilation.
        /// </summary>
        /// <param name="context">The context.</param>
        protected abstract void HandleCompilationStart(CompilationStartAnalysisContext context);

        private static bool ShouldEnableConcurrentExecution()
        {
#if DEBUG
            return false;
#else
            return true;
#endif
        }
    }
}
