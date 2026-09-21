// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1501StatementMustNotBeOnASingleLine,
        StyleCop.Analyzers.LayoutRules.SA1501CodeFixProvider>;

    public partial class SA1501CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionMethodLockStatementOnSingleLineAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        lock (new object()) [|{|] System.Diagnostics.Debug.Assert(true); }
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static void TestMethod()
    {
        lock (new object())
        {
            System.Diagnostics.Debug.Assert(true);
        }
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
