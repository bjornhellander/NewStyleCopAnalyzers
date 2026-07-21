// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.MaintainabilityRules;
    using Xunit;

    public partial class SA1405CSharp15UnitTests : SA1405CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionMethodDebugAssertWithoutMessageAsync()
        {
            var testCode = @"
using System.Diagnostics;
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        {|#0:Debug.Assert(true)|};
    }
}
";

            // TODO: Report bug - The compiler calls the registered invocation expression action three times
            var expected = new[] { this.Diagnostic().WithLocation(0), this.Diagnostic().WithLocation(0), this.Diagnostic().WithLocation(0) };

            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionMethodDebugAssertWithMessageAsync()
        {
            var testCode = @"
using System.Diagnostics;
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        Debug.Assert(true, ""message"");
    }
}
";

            await this.VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
