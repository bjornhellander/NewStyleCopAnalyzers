// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp14.OrderingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1213EventAccessorsMustFollowOrder,
        StyleCop.Analyzers.OrderingRules.SA1212SA1213CodeFixProvider>;

    public partial class SA1213CSharp15UnitTests : SA1213CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionEventWithWrongOrderAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static event System.EventHandler backingEvent;

    public static event System.EventHandler MyEvent
    {
        {|#0:remove|} { backingEvent -= value; }
        add { backingEvent += value; }
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    private static event System.EventHandler backingEvent;

    public static event System.EventHandler MyEvent
    {
        add { backingEvent += value; }
        remove { backingEvent -= value; }
    }
}
";

            // TODO: Report bug - The compiler calls the registered event declaration action three times
            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
