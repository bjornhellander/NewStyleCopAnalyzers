// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1002SemicolonsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1002CSharp8UnitTests
    {
        [Fact]
        public async Task TestClosingSquareBracketFollowedByExclamationAsync()
        {
            var testCode = @"namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod(object?[] arguments)
        {
            object o1 = arguments[0] !;
            object o2 = arguments[0]! ;
            object o3 = arguments[0] ! ;
        }
    }
}
";

            var fixedCode = @"namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod(object?[] arguments)
        {
            object o1 = arguments[0] !;
            object o2 = arguments[0]!;
            object o3 = arguments[0] !;
        }
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic().WithArguments(" not", "preceded").WithLocation(8, 39),
                Diagnostic().WithArguments(" not", "preceded").WithLocation(9, 40),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies the handling of the semicolon of a using declaration, which C# 8 introduced.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestUsingDeclarationAsync()
        {
            var testCode = @"namespace TestNamespace
{
    using System;

    public class TestClass
    {
        public void TestMethod()
        {
            using IDisposable resource = null [|;|]
        }
    }
}
";

            var fixedCode = @"namespace TestNamespace
{
    using System;

    public class TestClass
    {
        public void TestMethod()
        {
            using IDisposable resource = null;
        }
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
