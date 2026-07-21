// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.DocumentationRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1629DocumentationTextMustEndWithAPeriod,
        StyleCop.Analyzers.DocumentationRules.SA1629CodeFixProvider>;

    public partial class SA1629CSharp15UnitTests : SA1629CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionWithoutPeriodAsync()
        {
            var testCode = @"
/// <summary>
/// A summary without a period$$
/// </summary>
public union
TestUnion(string, int);";

            var fixedCode = @"
/// <summary>
/// A summary without a period.
/// </summary>
public union
TestUnion(string, int);";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
