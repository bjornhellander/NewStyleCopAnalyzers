// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1131UseReadableConditions,
        StyleCop.Analyzers.ReadabilityRules.SA1131CodeFixProvider>;

    public partial class SA1131CSharp10UnitTests
    {
        /// <summary>
        /// Verifies that a constant interpolated string, which is allowed from C# 10, is treated as a constant.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestConstantInterpolatedStringAsync()
        {
            var testCode = @"public class TestClass
{
    private const string Prefix = ""a"";

    public bool TestMethod(string value)
    {
        return [|$""{Prefix}b"" == value|];
    }
}";

            var fixedCode = @"public class TestClass
{
    private const string Prefix = ""a"";

    public bool TestMethod(string value)
    {
        return value == $""{Prefix}b"";
    }
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
