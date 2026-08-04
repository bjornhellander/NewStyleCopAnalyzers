// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1413UseTrailingCommasInMultiLineInitializers,
        StyleCop.Analyzers.MaintainabilityRules.SA1413CodeFixProvider>;

    public partial class SA1413CSharp8UnitTests
    {
        [Fact]
        public async Task TestSwitchExpressionAsync()
        {
            var testCode = @"namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod(int input)
        {
            int data1 = input switch { 0 => 1, 1 => 2 };
            int data2 = input switch { 0 => 1, 1 => 2, };
            int data3 = input switch
            {
                0 => 1,
                [|1 => 2|]
            };
        }
    }
}
";

            var fixedCode = @"namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod(int input)
        {
            int data1 = input switch { 0 => 1, 1 => 2 };
            int data2 = input switch { 0 => 1, 1 => 2, };
            int data3 = input switch
            {
                0 => 1,
                1 => 2,
            };
        }
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
