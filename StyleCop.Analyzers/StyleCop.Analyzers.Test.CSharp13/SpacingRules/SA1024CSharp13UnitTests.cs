// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.SA1024ColonsMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1024ColonsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1024CSharp13UnitTests
    {
        [Fact]
        public async Task TestAllowsRefStructConstraintColonMissingSpaceBeforeAsync()
        {
            var testCode = @"
class Foo<T>
    where T{|#0::|}allows ref struct
{
}";
            var fixedCode = @"
class Foo<T>
    where T : allows ref struct
{
}";

            var expected = new[]
            {
                Diagnostic(DescriptorPreceded).WithLocation(0),
                Diagnostic(DescriptorFollowed).WithLocation(0),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
