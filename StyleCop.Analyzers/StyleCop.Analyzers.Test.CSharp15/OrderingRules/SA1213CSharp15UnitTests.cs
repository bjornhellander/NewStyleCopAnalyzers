// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1213EventAccessorsMustFollowOrder,
        StyleCop.Analyzers.OrderingRules.SA1212SA1213CodeFixProvider>;

    public partial class SA1213CSharp15UnitTests
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
        [|remove|] { backingEvent -= value; }
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

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
