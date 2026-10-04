// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1121UseBuiltInTypeAlias,
        StyleCop.Analyzers.ReadabilityRules.SA1121CodeFixProvider>;

    public partial class SA1121CSharp9UnitTests
    {
        /// <summary>
        /// Verifies that neither <c>nint</c>/<c>nuint</c> nor <see cref="System.IntPtr"/>/<see cref="System.UIntPtr"/>
        /// is reported. In C# 9 and 10 they are different types for the compiler, so <c>nint</c> is not an alias that
        /// should be used instead of <see cref="System.IntPtr"/>.
        /// </summary>
        /// <param name="typeName">The type name to use.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [InlineData("nint")]
        [InlineData("nuint")]
        [InlineData("IntPtr")]
        [InlineData("UIntPtr")]
        [InlineData("System.IntPtr")]
        [InlineData("System.UIntPtr")]
        public async Task TestNativeSizedIntegerAsync(string typeName)
        {
            var testCode = $@"using System;

class TestClass
{{
    private {typeName} field;
}}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
