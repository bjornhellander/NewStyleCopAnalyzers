// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.SpacingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1000KeywordsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1000CSharp15UnitTests : SA1000CSharp14UnitTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task TestCollectionExpressionWithElementAsync(string spaces)
        {
            var testCode = $@"
using System.Collections.Generic;

public class Foo
{{
    public void Bar()
    {{
        List<string> names = [with{spaces}(capacity: 10), ""a""];
    }}
}}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public async Task TestUnionKeywordAsync(string spaces)
        {
            var testCode = $@"
public union{spaces}@TestUnion(string, int);
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
