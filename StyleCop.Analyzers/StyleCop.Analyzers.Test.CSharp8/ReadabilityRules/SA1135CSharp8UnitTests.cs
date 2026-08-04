// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1135UsingDirectivesMustBeQualified,
        StyleCop.Analyzers.ReadabilityRules.SA1135CodeFixProvider>;

    public partial class SA1135CSharp8UnitTests
    {
        [Theory]
        [InlineData("System.Collections.Generic.KeyValuePair<string, object?>")]
        [InlineData("System.Tuple<string, System.Tuple<string, bool>?>")]
        public async Task TestAliasTypeGenericNullableReferenceTypeAsync(string type)
        {
            var testCode = $@"
namespace TestNamespace
{{
    using TestAlias = {type};
}}
";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
