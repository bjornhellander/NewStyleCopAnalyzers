// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1027UseTabsCorrectly,
        StyleCop.Analyzers.SpacingRules.SA1027CodeFixProvider>;

    public partial class SA1027CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that a tab inside a nullable directive, which C# 8 introduced, is reported.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestTabInNullableDirectiveAsync()
        {
            // Written with escapes rather than a verbatim string because the test code contains a tab.
            var testCode =
                "#nullable[|\t|]enable\r\n" +
                "public class TestClass\r\n" +
                "{\r\n" +
                "}\r\n";

            var fixedCode =
                "#nullable   enable\r\n" +
                "public class TestClass\r\n" +
                "{\r\n" +
                "}\r\n";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
