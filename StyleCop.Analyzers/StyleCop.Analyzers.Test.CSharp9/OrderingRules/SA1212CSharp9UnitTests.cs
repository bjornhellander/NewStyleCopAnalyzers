// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1212PropertyAccessorsMustFollowOrder,
        StyleCop.Analyzers.OrderingRules.SA1212SA1213CodeFixProvider>;

    public partial class SA1212CSharp9UnitTests
    {
        [Fact]
        public async Task TestAutoPropertyDeclarationInitBeforeGetterAsync()
        {
            var testCode = @"
public class Foo
{
    public int Prop { [|init;|] get; }
}";

            var fixedCode = @"
public class Foo
{
    public int Prop { get; init; }
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestPropertyWithBackingFieldDeclarationInitBeforeGetterAsync()
        {
            var testCode = @"
public class Foo
{
    private int i = 0;

    public int Prop
    {
        [|init
        {
            i = value;
        }|]

        get
        {
            return i;
        }
    }
}";

            var fixedCode = @"
public class Foo
{
    private int i = 0;

    public int Prop
    {
        get
        {
            return i;
        }

        init
        {
            i = value;
        }
    }
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
