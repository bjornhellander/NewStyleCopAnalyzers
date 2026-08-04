// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    public partial class SA1611CSharp14UnitTests
    {
        [Fact]
        public async Task TestExtensionBlockDeclarationAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension(string source)
    {
    };
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
