// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.NamingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.NamingRules.SA1300ElementMustBeginWithUpperCaseLetter>;

    public partial class SA1300CSharp15UnitTests : SA1300CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionStartingWithLowerCaseLetterAsync()
        {
            var testCode = @"
public union {|#0:testUnion|}(string, int)
{
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("testUnion");

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionStartingWithUpperCaseLetterAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
