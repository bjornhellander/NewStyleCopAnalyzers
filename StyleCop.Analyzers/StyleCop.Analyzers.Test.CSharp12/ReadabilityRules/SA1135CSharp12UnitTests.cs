// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp12.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1135UsingDirectivesMustBeQualified,
        StyleCop.Analyzers.ReadabilityRules.SA1135CodeFixProvider>;

    public partial class SA1135CSharp12UnitTests
    {
        public static TheoryData<string> CorrectAliasableTypes { get; } = new TheoryData<string>()
        {
            "string",
            "(string, int)",
            "(System.String, System.Int32)",
            "bool[]",
            "System.Boolean[]",
        };

        [Theory]
        [MemberData(nameof(CorrectAliasableTypes))]
        public async Task TestAliasAnyTypeOutsideNamespaceAsync(string type)
        {
            var testCode = $@"
using MyType = {type};

namespace TestNamespace
{{
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CorrectAliasableTypes))]
        public async Task TestAliasAnyTypeInsideNamespaceAsync(string type)
        {
            var testCode = $@"
namespace TestNamespace
{{
    using MyType = {type};
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
