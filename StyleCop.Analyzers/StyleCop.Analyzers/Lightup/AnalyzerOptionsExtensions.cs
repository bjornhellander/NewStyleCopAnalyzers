// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Lightup
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Diagnostics.Lightup;

    internal static class AnalyzerOptionsExtensions
    {
        public static AnalyzerConfigOptionsWrapper? GetAnalyzerConfigOptions(this AnalyzerOptions analyzerOptions, SyntaxTree tree)
        {
            if (!LightupHelpers.SupportsCSharp9) // Needs only 3.1.0
            {
                return null;
            }

            return analyzerOptions.AnalyzerConfigOptionsProvider().GetOptions(tree);
        }
    }
}
