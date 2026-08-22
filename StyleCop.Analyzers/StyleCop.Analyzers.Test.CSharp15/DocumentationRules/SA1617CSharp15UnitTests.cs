// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1617VoidReturnValueMustNotBeDocumented,
        StyleCop.Analyzers.DocumentationRules.SA1617CodeFixProvider>;

    public partial class SA1617CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionVoidMethodWithReturnsDocumentedAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// A summary.
    /// </summary>
    /// [|<returns>A value.</returns>|]
    public static void TestMethod()
    {
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// A summary.
    /// </summary>
    public static void TestMethod()
    {
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
