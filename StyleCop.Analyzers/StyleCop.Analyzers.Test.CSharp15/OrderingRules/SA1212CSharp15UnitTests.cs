// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.OrderingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1212PropertyAccessorsMustFollowOrder,
        StyleCop.Analyzers.OrderingRules.SA1212SA1213CodeFixProvider>;

    public partial class SA1212CSharp15UnitTests : SA1212CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionPropertyWithWrongOrderAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static int backingValue;

    public static int MyValue
    {
        {|#0:set { backingValue = value; }|}
        get { return backingValue; }
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    private static int backingValue;

    public static int MyValue
    {
        get { return backingValue; }
        set { backingValue = value; }
    }
}
";

            // TODO: Report bug - The compiler calls the registered property declaration action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
