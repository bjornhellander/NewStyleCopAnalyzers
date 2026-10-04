// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1410RemoveDelegateParenthesisWhenPossible,
        StyleCop.Analyzers.MaintainabilityRules.SA1410SA1411CodeFixProvider>;

    public partial class SA1410CSharp9UnitTests
    {
        [Fact]
        public async Task TestStaticAnonymousMethodWithUnnecessaryParenthesisAsync()
        {
            var testCode = @"
public class TestClass
{
    public void TestMethod()
    {
        System.Func<int> getNumber = static delegate[|()|] { return 3; };
    }
}
";

            var fixedCode = @"
public class TestClass
{
    public void TestMethod()
    {
        System.Func<int> getNumber = static delegate { return 3; };
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
