// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.SA1010OpeningSquareBracketsMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1010OpeningSquareBracketsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1010CSharp14UnitTests
    {
        [Fact]
        public async Task TestFieldIndexerAsync()
        {
            var testCode = @"
public class TestClass
{
    private int result;

    public int[] Items
    {
        get => field;
        set
        {
            field = value;
            this.result = field {|#0:[|}0];
        }
    }
}
";

            var fixedCode = @"
public class TestClass
{
    private int result;

    public int[] Items
    {
        get => field;
        set
        {
            field = value;
            this.result = field[0];
        }
    }
}
";

            var expected = Diagnostic(DescriptorNotPreceded).WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestFieldInCollectionExpressionAsync()
        {
            var testCode = @"
public class TestClass
{
    private int[] items;

    public int Value
    {
        get => field;
        set
        {
            field = value;
            this.items = {|#0:[|} field ];
        }
    }
}
";

            var fixedCode = @"
public class TestClass
{
    private int[] items;

    public int Value
    {
        get => field;
        set
        {
            field = value;
            this.items = [field ];
        }
    }
}
";

            var expected = Diagnostic(DescriptorNotFollowed).WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
