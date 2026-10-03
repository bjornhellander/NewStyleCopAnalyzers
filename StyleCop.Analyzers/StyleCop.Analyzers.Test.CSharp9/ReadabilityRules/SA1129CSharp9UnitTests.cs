// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1129DoNotUseDefaultValueTypeConstructor,
        StyleCop.Analyzers.ReadabilityRules.SA1129CodeFixProvider>;

    public partial class SA1129CSharp9UnitTests
    {
        /// <summary>
        /// Verifies that target type new expressions for value types will generate diagnostics.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task VerifyValueTypeWithTargetTypeNewAsync()
        {
            var testCode = @"struct S
{
    internal static S F()
    {
        S s = [|new()|];
        return s;
    }
}
";

            var fixedTestCode = @"struct S
{
    internal static S F()
    {
        S s = default(S);
        return s;
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that the default constructor of a native-sized integer is replaced correctly. In C# 9 and 10,
        /// <c>nint</c> and <c>nuint</c> do not expose the members of <see cref="System.IntPtr"/> and
        /// <see cref="System.UIntPtr"/>, so <c>nint.Zero</c> cannot be used and a default value expression is used instead.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task VerifyNativeSizedIntegerAsync()
        {
            var testCode = @"class TestClass
{
    public void TestMethod()
    {
        nint a = [|new nint()|];
        nuint b = [|new nuint()|];
        nint c = [|new()|];
    }
}
";

            var fixedTestCode = this.GetExpectedFixedCodeVerifyNativeSizedInteger();
            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual string GetExpectedFixedCodeVerifyNativeSizedInteger()
        {
            return @"class TestClass
{
    public void TestMethod()
    {
        nint a = default(nint);
        nuint b = default(nuint);
        nint c = default(nint);
    }
}
";
        }
    }
}
