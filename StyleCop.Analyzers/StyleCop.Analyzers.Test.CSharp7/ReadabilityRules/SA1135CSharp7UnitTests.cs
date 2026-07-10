// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp7.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp6;
    using StyleCop.Analyzers.Test.CSharp6.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1135UsingDirectivesMustBeQualified,
        StyleCop.Analyzers.ReadabilityRules.SA1135CodeFixProvider>;

    public partial class SA1135CSharp7UnitTests : SA1135UnitTests
    {
        [Fact]
        [WorkItem(2879, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2879")]
        public async Task TestTupleTypeInUsingAliasAsync()
        {
            var testCode = @"
namespace TestNamespace
{
    using Example = System.Collections.Generic.List<(int, int)>;
}
";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        [WorkItem(2879, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2879")]
        public async Task TestTupleTypeWithNamedElementsInUsingAliasAsync()
        {
            var testCode = @"
namespace TestNamespace
{
    using Example = System.Collections.Generic.List<(int x, int y)>;
}
";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
