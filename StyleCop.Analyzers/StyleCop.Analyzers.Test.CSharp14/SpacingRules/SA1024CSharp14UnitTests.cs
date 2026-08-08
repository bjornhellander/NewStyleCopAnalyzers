// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.SA1024ColonsMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1024ColonsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1024CSharp14UnitTests
    {
        [Fact]
        public async Task TestFieldTernaryColonAsync()
        {
            var testCode = @"
public class TestClass
{
    public int Prop
    {
        get => field > 0 ? field{|#0::|}field;
    }
}
";

            var fixedCode = @"
public class TestClass
{
    public int Prop
    {
        get => field > 0 ? field : field;
    }
}
";

            var expected = new[]
            {
                Diagnostic(DescriptorPreceded).WithLocation(0),
                Diagnostic(DescriptorFollowed).WithLocation(0),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
