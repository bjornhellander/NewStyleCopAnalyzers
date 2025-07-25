﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.CSharp7.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopDiagnosticVerifier<StyleCop.Analyzers.ReadabilityRules.SA1115ParameterMustFollowComma>;

    public partial class SA1115CSharp7UnitTests : SA1115UnitTests
    {
        [Fact]
        public async Task TestLocalFunctionDeclarationEmptyLinesBetweenParametersAsync()
        {
            var testCode = @"
class Foo
{
    public void Method()
    {
        void Bar(int i, int z,

string s,

int j,
int k)
        {
        }
    }
}";

            DiagnosticResult expected1 = Diagnostic().WithLocation(8, 1);
            DiagnosticResult expected2 = Diagnostic().WithLocation(10, 1);

            await VerifyCSharpDiagnosticAsync(testCode, new[] { expected1, expected2 }, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestLocalFunctionDeclarationSecondParameterOnTheNextLineAsync()
        {
            var testCode = @"
class Foo
{
    public void Method()
    {
        void Bar(int i,
string s)
        {
        }
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestLocalFunctionDeclarationParametersAtTheSameLineAsync()
        {
            var testCode = @"
class Foo
{
    public void Method()
    {
        void Bar(int i, string s)
        {
        }
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
