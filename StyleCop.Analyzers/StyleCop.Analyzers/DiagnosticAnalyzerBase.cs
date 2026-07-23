// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers
{
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
