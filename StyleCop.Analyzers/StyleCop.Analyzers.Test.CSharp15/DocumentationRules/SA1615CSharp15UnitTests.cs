// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.DocumentationRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1615ElementReturnValueMustBeDocumented,
        StyleCop.Analyzers.DocumentationRules.SA1615SA1616CodeFixProvider>;

    public partial class SA1615CSharp15UnitTests : SA1615CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionMethodReturnValueNotDocumentedAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// A summary.
    /// </summary>
    public static {|#0:int|} TestMethod()
    {
        return 0;
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// A summary.
    /// </summary>
    /// <returns></returns>
    public static int TestMethod()
    {
        return 0;
    }
}
";

            // TODO: Report bug - The compiler calls the registered method declaration action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
