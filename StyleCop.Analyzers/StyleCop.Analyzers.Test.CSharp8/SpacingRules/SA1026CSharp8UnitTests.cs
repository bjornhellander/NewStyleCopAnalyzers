// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1026CodeMustNotContainSpaceAfterNewKeywordInImplicitlyTypedArrayAllocation,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1026CSharp8UnitTests
    {
        /// <summary>
        /// Verifies the handling of an implicitly typed stackalloc in a nested expression, which C# 8 allows.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestImplicitStackAllocInNestedExpressionAsync()
        {
            var testCode = @"using System;

public class TestClass
{
    public void TestMethod()
    {
        Bar({|#0:stackalloc|} [] { 1, 2, 3 });
    }

    public void Bar(Span<int> value)
    {
    }
}
";

            var fixedCode = @"using System;

public class TestClass
{
    public void TestMethod()
    {
        Bar(stackalloc[] { 1, 2, 3 });
    }

    public void Bar(Span<int> value)
    {
    }
}
";

            DiagnosticResult expected = Diagnostic().WithLocation(0).WithArguments("stackalloc");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
