// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.SA1008OpeningParenthesisMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1008OpeningParenthesisMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1008CSharp14UnitTests
    {
        [Fact]
        public async Task TestExtensionBlockDeclarationWithSpaceBeforeParenthesisAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension {|#0:(|}string source)
    {
    }
}
";

            var fixedCode = @"
public static class TestClass
{
    extension(string source)
    {
    }
}
";

            var expected = Diagnostic(DescriptorNotPreceded).WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestExtensionBlockDeclarationWithSpaceAfterParenthesisAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension{|#0:(|} string source)
    {
    }
}
";

            var fixedCode = @"
public static class TestClass
{
    extension(string source)
    {
    }
}
";

            var expected = Diagnostic(DescriptorNotFollowed).WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInstanceIncrementOperatorDeclarationWithSpaceBeforeParenthesisAsync()
        {
            var testCode = @"
public class TestClass
{
    private int value;

    public void operator ++ {|#0:(|})
    {
        this.value++;
    }
}
";

            var fixedCode = @"
public class TestClass
{
    private int value;

    public void operator ++()
    {
        this.value++;
    }
}
";

            var expected = Diagnostic(DescriptorNotPreceded).WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestFieldKeywordInvocationAsync()
        {
            var testCode = @"
public class TestClass
{
    private int result;

    public System.Func<int, int> Handler
    {
        get => field;
        set
        {
            field = value;
            this.result = field {|#0:(|}9);
        }
    }
}
";

            var fixedCode = @"
public class TestClass
{
    private int result;

    public System.Func<int, int> Handler
    {
        get => field;
        set
        {
            field = value;
            this.result = field(9);
        }
    }
}
";

            var expected = Diagnostic(DescriptorNotPreceded).WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestSimpleLambdaParameterWithRefModifierAsync()
        {
            var testCode = @"
public delegate void RefIntAction(ref int value);

public class TestClass
{
    public void Method()
    {
        RefIntAction action = {|#0:(|} ref @x) => { x = 1; };
    }
}
";

            var fixedCode = @"
public delegate void RefIntAction(ref int value);

public class TestClass
{
    public void Method()
    {
        RefIntAction action = (ref @x) => { x = 1; };
    }
}
";

            var expected = Diagnostic(DescriptorNotFollowed).WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
