// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.OrderingRules.SA1201ElementsMustAppearInTheCorrectOrder>;

    public partial class SA1201CSharp13UnitTests
    {
        [Fact]
        public async Task TestRefStructImplementingInterfaceFieldAfterMethodAsync()
        {
            var testCode = @"
public interface IInterface
{
    void TestMethod();
}

public ref struct TestRefStruct : IInterface
{
    public void TestMethod() { }

    public int {|#0:TestField|};
}";

            var expected = Diagnostic().WithLocation(0).WithArguments("field", "method");

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestFieldAfterPartialPropertyAsync()
        {
            var testCode = @"
public partial class TypeName
{
    public partial int Test { get; set; }

    public int {|#0:TestField|};
}

public partial class TypeName
{
    public partial int Test
    {
        get => 0;
        set { }
    }
}";

            var expected = Diagnostic().WithLocation(0).WithArguments("field", "property");

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
