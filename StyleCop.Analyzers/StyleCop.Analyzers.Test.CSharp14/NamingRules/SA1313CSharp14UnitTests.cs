// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp13.NamingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1313ParameterNamesMustBeginWithLowerCaseLetter,
        StyleCop.Analyzers.NamingRules.RenameToLowerCaseCodeFixProvider>;

    public partial class SA1313CSharp14UnitTests : SA1313CSharp13UnitTests
    {
        [Fact]
        public async Task TestExtensionDeclarationWithParameterNameAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension(string {|#0:Param|})
    {
    }
}
";

            var fixedCode = @"
public static class TestClass
{
    extension(string param)
    {
    }
}
";

            // TODO: Syntax node actions seems to be triggered twice
            // Reported in https://github.com/dotnet/roslyn/issues/80319
            var expected = new[]
            {
                Diagnostic().WithLocation(0).WithArguments("Param"),
                Diagnostic().WithLocation(0).WithArguments("Param"),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestExtensionDeclarationWithoutParameterNameAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension(string)
    {
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
