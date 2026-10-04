// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1133DoNotCombineAttributes,
        StyleCop.Analyzers.ReadabilityRules.SA1133CodeFixProvider>;

    public partial class SA1133CSharp9UnitTests
    {
        [Fact]
        public async Task TestLocalFunctionWithCombinedAttributesAsync()
        {
            var testCode = @"using System;

public class TestClass
{
    public void TestMethod()
    {
        [Obsolete, [|CLSCompliant|](false)]
        static void LocalFunction()
        {
        }
    }
}
";

            var fixedCode = @"using System;

public class TestClass
{
    public void TestMethod()
    {
        [Obsolete]
        [CLSCompliant(false)]
        static void LocalFunction()
        {
        }
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
