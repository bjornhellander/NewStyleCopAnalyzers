// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.DocumentationRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1617VoidReturnValueMustNotBeDocumented,
        StyleCop.Analyzers.DocumentationRules.SA1617CodeFixProvider>;

    public partial class SA1617CSharp15UnitTests : SA1617CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionVoidMethodWithReturnsDocumentedAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// A summary.
    /// </summary>
    /// {|#0:<returns>A value.</returns>|}
    public static void TestMethod()
    {
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// A summary.
    /// </summary>
    public static void TestMethod()
    {
    }
}
";

            // TODO: Report bug - The compiler calls the registered method declaration action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
