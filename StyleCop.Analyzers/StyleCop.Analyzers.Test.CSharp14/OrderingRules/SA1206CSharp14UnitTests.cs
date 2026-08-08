// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1206DeclarationKeywordsMustFollowOrder,
        StyleCop.Analyzers.OrderingRules.SA1206CodeFixProvider>;

    public partial class SA1206CSharp14UnitTests
    {
        [Fact]
        public async Task TestOverrideBeforeAccessModifierOnCompoundAssignmentOperatorAsync()
        {
            var testCode = @"
public class Base
{
    public int Value;

    public virtual void operator +=(int x)
    {
        this.Value += x;
    }
}

public class Derived : Base
{
    override {|#0:public|} void operator +=(int x)
    {
        this.Value += x * 2;
    }
}
";
            var fixedCode = @"
public class Base
{
    public int Value;

    public virtual void operator +=(int x)
    {
        this.Value += x;
    }
}

public class Derived : Base
{
    public override void operator +=(int x)
    {
        this.Value += x * 2;
    }
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("public", "override");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
