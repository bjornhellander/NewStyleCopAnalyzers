// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp8.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<StyleCop.Analyzers.ReadabilityRules.SA1118ParameterMustNotSpanMultipleLines>;

    public partial class SA1118CSharp9UnitTests : SA1118CSharp8UnitTests
    {
        [Fact]
        public async Task TestWithExpressionAsync()
        {
            var testCode = @"
class Foo
{
    public record R(int X, int Y);

    public void FunA(params object[] j)
    {
    }

    public void FunB(R r)
    {
        FunA(
            1,
            r with
            {
                X = 1,
            });
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestWithExpression2Async()
        {
            var testCode = @"
class Foo
{
    public record R(int X, int Y);

    public void FunA(params object[] j)
    {
    }

    public void FunB(R r)
    {
        FunA(
            1,
            r with
            {
                X = 1,
            },
            2);
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestNewExpressionAsync()
        {
            var testCode = @"
     public class MyClass
    {
        public class MyObject
        {
            public string MyValue { get; init; }
        }

        public void MyTestFunction()
        {
            MyCallingFunction(0, new()
            {
                MyValue = ""Test""
            });
        }

        public void MyCallingFunction(int index, MyObject myObject)
        {
        }
    }";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
