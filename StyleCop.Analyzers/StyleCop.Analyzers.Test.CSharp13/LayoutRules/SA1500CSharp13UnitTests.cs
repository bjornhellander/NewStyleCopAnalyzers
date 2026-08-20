// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1500BracesForMultiLineStatementsMustNotShareLine,
        StyleCop.Analyzers.LayoutRules.SA1500CodeFixProvider>;

    public partial class SA1500CSharp13UnitTests
    {
        [Fact]
        public async Task TestImplicitIndexInitializerAsync()
        {
            var testCode = @"
public class TimerRemaining
{
    public int[] Buffer { get; set; } = new int[10];
}

public class TestClass
{
    public void TestMethod()
    {
        var countdown = new TimerRemaining()
        {
            Buffer = [|{|]
                [^1] = 0,
            },
        };
    }
}
";

            var fixedCode = @"
public class TimerRemaining
{
    public int[] Buffer { get; set; } = new int[10];
}

public class TestClass
{
    public void TestMethod()
    {
        var countdown = new TimerRemaining()
        {
            Buffer =
            {
                [^1] = 0,
            },
        };
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
