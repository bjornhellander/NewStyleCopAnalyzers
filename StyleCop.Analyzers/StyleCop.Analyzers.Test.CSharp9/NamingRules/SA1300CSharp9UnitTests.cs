// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1300ElementMustBeginWithUpperCaseLetter,
        StyleCop.Analyzers.NamingRules.RenameToUpperCaseCodeFixProvider>;

    public partial class SA1300CSharp9UnitTests
    {
        // TODO: A theory in a base test class might have made this unnecessary. Check if possible to simplify.
        [Fact]
        public async Task TestPositionalRecord1Async()
        {
            var testCode = @"
public record {|#0:r|}(int A)
{
    public r(int a, int b)
        : this(A: a)
    {
    }
}
";

            var fixedCode = @"
public record R(int A)
{
    public R(int a, int b)
        : this(A: a)
    {
    }
}
";

            await VerifyCSharpFixAsync(testCode, this.GetExpectedResultTestPositionalRecord1(), fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestPositionalRecord2Async()
        {
            var testCode = @"
public record R(int [|a|])
{
    public R(int a, int b)
        : this(a: a)
    {
    }
}
";

            var fixedCode = @"
public record R(int A)
{
    public R(int a, int b)
        : this(A: a)
    {
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestLocalFunctionInTopLevelProgramAsync()
        {
            var testCode = @"localFunction();

void {|#0:localFunction|}()
{
}
";

            var fixedCode = @"LocalFunction();

void LocalFunction()
{
}
";

            await new CSharpTest()
            {
                TestState =
                {
                    OutputKind = OutputKind.ConsoleApplication,
                    Sources = { testCode },
                },
                ExpectedDiagnostics = { Diagnostic().WithLocation(0).WithArguments("localFunction") },
                FixedCode = fixedCode,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestPositionalRecord1()
        {
            // NOTE: Seems like a Roslyn bug made diagnostics be reported twice. Fixed in a later version.
            return new[]
            {
                // /0/Test0.cs(2,15): warning SA1300: Element 'r' should begin with an uppercase letter
                Diagnostic().WithLocation(0).WithArguments("r"),

                // /0/Test0.cs(2,15): warning SA1300: Element 'r' should begin with an uppercase letter
                Diagnostic().WithLocation(0).WithArguments("r"),
            };
        }
    }
}
