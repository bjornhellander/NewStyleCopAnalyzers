// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.LayoutRules
{
    using Microsoft.CodeAnalysis.Testing;

    public partial class SA1504CSharp13UnitTests
    {
        protected override DiagnosticResult[] GetExpectedResultAccessorWithoutBody()
        {
            return new DiagnosticResult[]
            {
                DiagnosticResult.CompilerError("CS8652").WithMessage("The feature 'field keyword' is currently in Preview and *unsupported*. To use Preview features, use the 'preview' language version.").WithLocation(4, 16),
            };
        }
    }
}
