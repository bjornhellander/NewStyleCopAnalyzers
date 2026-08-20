// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1127GenericTypeConstraintsMustBeOnOwnLine,
        StyleCop.Analyzers.ReadabilityRules.SA1127CodeFixProvider>;

    public partial class SA1127CSharp13UnitTests
    {
        [Fact]
        public async Task TestViolationWithAllowsRefStructConstraintAsync()
        {
            var testCode = @"
class Foo<T> [|where T : allows ref struct|] {}";

            var fixedCode = @"
class Foo<T>
    where T : allows ref struct
{}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestViolationWithMultiConstraintAllowsRefStructAsync()
        {
            var testCode = @"
interface ISomeInterface
{
}

class Foo<T> [|where T : ISomeInterface, allows ref struct|] {}";
            var fixedCode = @"
interface ISomeInterface
{
}

class Foo<T>
    where T : ISomeInterface, allows ref struct
{}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
