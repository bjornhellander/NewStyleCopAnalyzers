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
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1110OpeningParenthesisMustBeOnDeclarationLine,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1110CSharp7UnitTests : SA1110UnitTests
    {
        [Fact]
        public async Task TestLocalFunctionDeclarationOpeningParenthesisInTheNextLineAsync()
        {
            var testCode = @"
class Foo
{
    public void Method()
    {
        void Bar
                        ()
        {

        }
    }
}";
            var fixedCode = @"
class Foo
{
    public void Method()
    {
        void Bar()
        {

        }
    }
}";

            DiagnosticResult expected = Diagnostic().WithLocation(7, 25);
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestLocalFunctionDeclarationOpeningParenthesisInTheSameLineAsync()
        {
            var testCode = @"
class Foo
{
    public void Method()
    {
        void 
                        Bar()
        {

        }
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
