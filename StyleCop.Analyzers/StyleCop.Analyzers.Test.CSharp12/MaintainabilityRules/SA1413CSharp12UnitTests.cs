// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp12.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp11.MaintainabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1413UseTrailingCommasInMultiLineInitializers,
        StyleCop.Analyzers.MaintainabilityRules.SA1413CodeFixProvider>;

    public partial class SA1413CSharp12UnitTests : SA1413CSharp11UnitTests
    {
        [Theory]
        [InlineData("1, 2")]
        [InlineData("1, 2,")]
        public async Task VerifySingleLineCollectionExpressionAsync(string elements)
        {
            var testCode = $@"
namespace TestNamespace
{{
    public class TestClass
    {{
        private int[] values = [ {elements} ];
    }}
}}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task VerifyMultiLineCollectionExpressionAsync()
        {
            var testCode = @"
namespace TestNamespace
{
    public class TestClass
    {
        private int[] values =
        [
            1,
            [|2|]
        ];
    }
}
";

            var fixedCode = @"
namespace TestNamespace
{
    public class TestClass
    {
        private int[] values =
        [
            1,
            2,
        ];
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
