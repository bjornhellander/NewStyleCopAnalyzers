// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1101PrefixLocalCallsWithThis,
        StyleCop.Analyzers.ReadabilityRules.SA1101CodeFixProvider>;

    public partial class SA1101CSharp10UnitTests
    {
        [Fact]
        public async Task TestExtendedPropertyPatternAsync()
        {
            var testCode = @"public class Test
{
    public Test Inner;
    public string Value;

    public bool Method(Test arg)
    {
        return arg is { Inner.Value: """" };
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestStructWithExpressionAsync()
        {
            var testCode = @"public struct S
{
    public int X { get; init; }

    public int Y { get; init; }

    public S M() => this with { X = [|Y|] };
}";

            var fixedCode = @"public struct S
{
    public int X { get; init; }

    public int Y { get; init; }

    public S M() => this with { X = this.Y };
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestAnonymousTypeWithExpressionAsync()
        {
            var testCode = @"public class Test
{
    public int X { get; set; }

    public object M()
    {
        var a = new { X = 1 };
        return a with { X = [|X|] };
    }
}";

            var fixedCode = @"public class Test
{
    public int X { get; set; }

    public object M()
    {
        var a = new { X = 1 };
        return a with { X = this.X };
    }
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
