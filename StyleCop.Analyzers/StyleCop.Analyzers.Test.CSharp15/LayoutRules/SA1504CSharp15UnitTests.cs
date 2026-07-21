// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.LayoutRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1504AllAccessorsMustBeSingleLineOrMultiLine,
        StyleCop.Analyzers.LayoutRules.SA1504CodeFixProvider>;

    public partial class SA1504CSharp15UnitTests : SA1504CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionPropertyWithMixedAccessorStyleAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static int backingField;

    public static int TestProperty
    {
        {|#0:get|} { return backingField; }

        set
        {
            backingField = value;
        }
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    private static int backingField;

    public static int TestProperty
    {
        get { return backingField; }
        set { backingField = value; }
    }
}
";

            // TODO: Report bug - The compiler calls the registered property declaration action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
