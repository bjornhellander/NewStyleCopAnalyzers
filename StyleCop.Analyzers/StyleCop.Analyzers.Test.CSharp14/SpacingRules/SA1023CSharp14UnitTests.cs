// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.SA1023DereferenceAndAccessOfSymbolsMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1023DereferenceAndAccessOfSymbolsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1023CSharp14UnitTests
    {
        [Fact]
        public async Task TestFieldPointerDereferenceAsync()
        {
            var testCode = @"
public unsafe class TestClass
{
    private int result;

    public int* Prop
    {
        get => field;
        set
        {
            field = value;
            this.result = {|#0:*|} field;
        }
    }
}
";

            var fixedCode = @"
public unsafe class TestClass
{
    private int result;

    public int* Prop
    {
        get => field;
        set
        {
            field = value;
            this.result = *field;
        }
    }
}
";

            var expected = Diagnostic(DescriptorNotFollowed).WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
