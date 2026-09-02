// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SX1101DoNotPrefixLocalMembersWithThis,
        StyleCop.Analyzers.ReadabilityRules.SX1101CodeFixProvider>;

    public partial class SX1101CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that a this prefix in the default implementation of an interface member, which C# 8 introduced,
        /// is detected and removed.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestDefaultInterfaceMethodAsync()
        {
            var testCode = @"public interface ITest
{
    void Method();

    void DefaultMethod()
    {
        [|this|].Method();
    }
}";

            var fixedCode = @"public interface ITest
{
    void Method();

    void DefaultMethod()
    {
        Method();
    }
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
