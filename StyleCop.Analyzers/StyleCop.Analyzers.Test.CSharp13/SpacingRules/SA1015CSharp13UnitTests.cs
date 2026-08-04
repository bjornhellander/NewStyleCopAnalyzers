// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.SpacingRules
{
    using Microsoft.CodeAnalysis.Testing;

    public partial class SA1015CSharp13UnitTests
    {
        protected override DiagnosticResult[] GetExpectedResultMissingToken()
        {
            return new[]
            {
                DiagnosticResult.CompilerError("CS1003").WithLocation(7, 35).WithArguments(">"),
            };
        }
    }
}
