// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1101PrefixLocalCallsWithThis,
        StyleCop.Analyzers.ReadabilityRules.SA1101CodeFixProvider>;

    public partial class SA1101CSharp9UnitTests
    {
        [Fact]
        public async Task TestRecordWithExpressionAsync()
        {
            var testCode = @"public class Test
{
    public record A
    {
        public string Prop { get; init; }
    }

    public A UpdateA(A value)
    {
        return value with { Prop = ""newValue"" };
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a positional record parameter is not reported where it refers to the constructor parameter
        /// (initializers and base-type arguments), but is reported where it refers to the generated property.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestPositionalRecordParametersAsync()
        {
            var testCode = @"public record A(int X, string Y);

public record B(int X) : A(X, ""a"")
{
    public int Z { get; init; } = X;

    public int W => [|X|];

    public B M() => this with { X = [|Z|] };
}";

            var fixedCode = @"public record A(int X, string Y);

public record B(int X) : A(X, ""a"")
{
    public int Z { get; init; } = X;

    public int W => this.X;

    public B M() => this with { X = this.Z };
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
