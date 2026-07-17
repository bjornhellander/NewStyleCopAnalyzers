// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.MaintainabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1413UseTrailingCommasInMultiLineInitializers,
        StyleCop.Analyzers.MaintainabilityRules.SA1413CodeFixProvider>;

    public partial class SA1413CSharp15UnitTests : SA1413CSharp14UnitTests
    {
        [Theory]
        [InlineData("with(capacity: 10), \"a\", \"b\"")]
        [InlineData("with(capacity: 10), \"a\", \"b\",")]
        public async Task VerifySingleLineCollectionExpressionWithElementAsync(string elements)
        {
            var testCode = $@"
using System.Collections.Generic;

namespace TestNamespace
{{
    public class TestClass
    {{
        private List<string> values = [ {elements} ];
    }}
}}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task VerifyMultiLineCollectionExpressionWithElementAsync()
        {
            var testCode = @"
using System.Collections.Generic;

namespace TestNamespace
{
    public class TestClass
    {
        private List<string> values =
        [
            with(capacity: 10),
            ""a"",
            [|""b""|]
        ];
    }
}
";

            var fixedCode = @"
using System.Collections.Generic;

namespace TestNamespace
{
    public class TestClass
    {
        private List<string> values =
        [
            with(capacity: 10),
            ""a"",
            ""b"",
        ];
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
