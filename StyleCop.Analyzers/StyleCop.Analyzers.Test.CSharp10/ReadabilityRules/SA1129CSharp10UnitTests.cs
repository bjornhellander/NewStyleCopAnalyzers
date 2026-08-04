// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1129DoNotUseDefaultValueTypeConstructor,
        StyleCop.Analyzers.ReadabilityRules.SA1129CodeFixProvider>;

    public partial class SA1129CSharp10UnitTests
    {
        [Fact]
        public async Task VerifyParameterlessStructConstructorAsync()
        {
            var testCode = @"struct S
{
    public S() { }

    internal static S F1()
    {
        S s = new S();
        return s;
    }

    internal static S F2()
    {
        S s = new();
        return s;
    }

    internal static S F3() => new S();

    internal static S F4() => new();
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, testCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task VerifyParameterlessStructConstructorInMetadataAsync()
        {
            await new CSharpTest
            {
                TestState =
                {
                    Sources =
                    {
                        @"class B
{
    internal static S F1()
    {
        S s = new S();
        return s;
    }

    internal static S F2()
    {
        S s = new();
        return s;
    }

    internal static S F3() => new S();

    internal static S F4() => new();
}
",
                    },
                    AdditionalProjects =
                    {
                        ["Reference"] =
                        {
                            Sources =
                            {
                                @"public struct S { public S() { } }",
                            },
                        },
                    },
                    AdditionalProjectReferences = { "Reference" },
                },
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }
    }
}
