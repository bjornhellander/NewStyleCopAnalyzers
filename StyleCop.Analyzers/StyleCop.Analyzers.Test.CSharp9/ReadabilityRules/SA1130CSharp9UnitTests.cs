// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1130UseLambdaSyntax,
        StyleCop.Analyzers.ReadabilityRules.SA1130CodeFixProvider>;

    public partial class SA1130CSharp9UnitTests
    {
        [Fact]
        public async Task TestStaticAnonymousMethodAsync()
        {
            var testCode = @"using System;
public class TestClass
{
    public void TestMethod()
    {
        Func<int, int> a = static [|delegate|] (int x) { return x; };
        Action b = static [|delegate|] { };
    }
}";

            var fixedCode = @"using System;
public class TestClass
{
    public void TestMethod()
    {
        Func<int, int> a = static x => { return x; };
        Action b = static () => { };
    }
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
