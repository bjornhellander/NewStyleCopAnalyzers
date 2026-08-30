// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1028CodeMustNotContainTrailingWhitespace,
        StyleCop.Analyzers.SpacingRules.SA1028CodeFixProvider>;

    public partial class SA1028CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that trailing whitespace after a nullable directive, which C# 8 introduced, is reported.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestTrailingWhitespaceAfterNullableDirectiveAsync()
        {
            var testCode = @"#nullable enable[| |]
public class TestClass
{
}
";

            var fixedCode = @"#nullable enable
public class TestClass
{
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
