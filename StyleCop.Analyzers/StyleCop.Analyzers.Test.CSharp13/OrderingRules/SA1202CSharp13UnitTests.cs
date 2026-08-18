// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.OrderingRules.SA1202ElementsMustBeOrderedByAccess>;

    public partial class SA1202CSharp13UnitTests
    {
        [Fact]
        public async Task TestRefStructExplicitInterfaceImplementationAfterInternalMemberAsync()
        {
            var testCode = @"
public interface IInterface
{
    void TestMethod();
}

public ref struct TestRefStruct : IInterface
{
    internal void TestMethod2() { }

    void IInterface.{|#0:TestMethod|}() { }
}";

            var expected = Diagnostic().WithLocation(0).WithArguments("public", "internal");

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestPartialPublicPropertyAfterInternalPropertyAsync()
        {
            var testCode = @"
public partial class TypeName
{
    internal int OtherProperty { get; set; }

    public partial int {|#0:Test|} { get; set; }
}

public partial class TypeName
{
    public partial int Test
    {
        get => 0;
        set { }
    }
}";

            var expected = Diagnostic().WithLocation(0).WithArguments("public", "internal");

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
