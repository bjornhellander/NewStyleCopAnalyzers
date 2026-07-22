// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.OrderingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.OrderingRules.SA1202ElementsMustBeOrderedByAccess>;

    public partial class SA1202CSharp15UnitTests : SA1202CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionMembersOrderedByAccessAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public int Count => 0;

    private string Name => """";
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionMembersNotOrderedByAccessAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private string Name => """";

    public int {|#0:Count|} => 0;
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("public", "private");

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
