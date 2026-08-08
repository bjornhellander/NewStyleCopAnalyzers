// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.SpacingRules.SA1019MemberAccessSymbolsMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1019MemberAccessSymbolsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1019CSharp14UnitTests
    {
        [Fact]
        public async Task TestFieldMemberAccessAsync()
        {
            var testCode = @"
public class TestClass
{
    private string result;

    public string Prop
    {
        get => field;
        set
        {
            field = value;
            this.result = field {|#0:.|}ToUpperInvariant();
        }
    }
}
";

            var fixedCode = @"
public class TestClass
{
    private string result;

    public string Prop
    {
        get => field;
        set
        {
            field = value;
            this.result = field.ToUpperInvariant();
        }
    }
}
";

            var expected = Diagnostic(DescriptorNotPreceded).WithLocation(0).WithArguments(".");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
