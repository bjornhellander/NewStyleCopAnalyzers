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
        StyleCop.Analyzers.SpacingRules.SA1007OperatorKeywordMustBeFollowedBySpace,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1007CSharp15UnitTests : SA1007CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionOperatorKeywordNotFollowedBySpaceAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    public static TestUnion [|operator|]+(TestUnion a, TestUnion b) => a;
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    public static TestUnion operator +(TestUnion a, TestUnion b) => a;
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
