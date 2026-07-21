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
        StyleCop.Analyzers.SpacingRules.SA1004DocumentationLinesMustBeginWithSingleSpace,
        StyleCop.Analyzers.SpacingRules.SA1004CodeFixProvider>;

    public partial class SA1004CSharp15UnitTests : SA1004CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionDocumentationLineWithoutSpaceAsync()
        {
            var testCode = @"
/// <summary>
///[|Missing space.|]
/// </summary>
public union TestUnion(string, int);
";

            var fixedCode = @"
/// <summary>
/// Missing space.
/// </summary>
public union TestUnion(string, int);
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
