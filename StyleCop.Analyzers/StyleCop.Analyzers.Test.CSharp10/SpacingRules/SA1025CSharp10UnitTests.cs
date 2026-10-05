// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1025CodeMustNotContainMultipleWhitespaceInARow,
        StyleCop.Analyzers.SpacingRules.SA1025CodeFixProvider>;

    public partial class SA1025CSharp10UnitTests
    {
        [Fact]
        public async Task TestMappedLineDirectiveDoesNotAffectIndentationAsync()
        {
            var testCode = @"#line (5,5)-(5,10) ""File.cs""
class TestClass
{
    public void Test()
    {
        int value = 0;
    }
}
#line default";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
