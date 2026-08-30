// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1206DeclarationKeywordsMustFollowOrder,
        StyleCop.Analyzers.OrderingRules.SA1206CodeFixProvider>;

    public partial class SA1206CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that an access modifier must precede the readonly keyword of a readonly instance member, which C# 8 introduced.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestReadonlyInstanceMemberAsync()
        {
            var testCode = @"public struct TestStruct
{
    readonly {|#0:public|} int Method() => 0;
}
";

            var fixedCode = @"public struct TestStruct
{
    public readonly int Method() => 0;
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("public", "readonly");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
