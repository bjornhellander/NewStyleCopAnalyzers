// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1001CommasMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1001CSharp13UnitTests
    {
        [Fact]
        public async Task TestNoSpaceAfterCommaBeforeAllowsRefStructConstraintAsync()
        {
            var testCode = @"
interface ISomeInterface
{
}

class Foo<T>
    where T : ISomeInterface{|#0:,|}allows ref struct
{
}";
            var fixedCode = @"
interface ISomeInterface
{
}

class Foo<T>
    where T : ISomeInterface, allows ref struct
{
}";

            var expected = Diagnostic().WithLocation(0).WithArguments(string.Empty, "followed");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
