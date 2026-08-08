// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1313ParameterNamesMustBeginWithLowerCaseLetter,
        StyleCop.Analyzers.NamingRules.RenameToLowerCaseCodeFixProvider>;

    public partial class SA1313CSharp11UnitTests
    {
        [Fact]
        public async Task TestScopedParameterStartingWithUpperCaseLetterAsync()
        {
            var testCode = @"
public class TestClass
{
    public void Bar(scoped System.Span<int> {|#0:Value|})
    {
    }
}
";

            var fixedCode = @"
public class TestClass
{
    public void Bar(scoped System.Span<int> value)
    {
    }
}
";

            var expected = Diagnostic().WithArguments("Value").WithLocation(0);

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
