// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    using static StyleCop.Analyzers.SpacingRules.SA1024ColonsMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1024ColonsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1024CSharp10UnitTests
    {
        [Fact]
        public async Task TestExtendedPropertyPatternAsync()
        {
            var testCode = @"public class Foo
{
    public Foo Inner { get; }

    public int Value { get; }

    public bool TestMethod(Foo value)
    {
        return value is { Inner.Value {|#0::|} 1 }
            || value is { Inner.Value{|#1::|}1 };
    }
}";

            var fixedCode = @"public class Foo
{
    public Foo Inner { get; }

    public int Value { get; }

    public bool TestMethod(Foo value)
    {
        return value is { Inner.Value: 1 }
            || value is { Inner.Value: 1 };
    }
}";

            var expected = new[]
            {
                Diagnostic(DescriptorNotPreceded).WithLocation(0),
                Diagnostic(DescriptorFollowed).WithLocation(1),
            };
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
