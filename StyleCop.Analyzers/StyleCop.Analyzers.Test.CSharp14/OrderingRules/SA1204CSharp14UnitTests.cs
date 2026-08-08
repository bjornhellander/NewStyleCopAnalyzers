// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1204StaticElementsMustAppearBeforeInstanceElements,
        StyleCop.Analyzers.OrderingRules.ElementOrderCodeFixProvider>;

    public partial class SA1204CSharp14UnitTests
    {
        [Fact]
        public async Task TestStaticOperatorAfterInstanceCompoundAssignmentOperatorAsync()
        {
            var testCode = @"
public class TestClass
{
    public int Value;

    public void operator +=(int x)
    {
        this.Value += x;
    }

    public static TestClass operator +(TestClass a, int b)
    {
        return a;
    }
}
";

            var fixedCode = @"
public class TestClass
{
    public int Value;

    public static TestClass operator +(TestClass a, int b)
    {
        return a;
    }

    public void operator +=(int x)
    {
        this.Value += x;
    }
}
";

            var expected = Diagnostic().WithLocation(11, 5);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
