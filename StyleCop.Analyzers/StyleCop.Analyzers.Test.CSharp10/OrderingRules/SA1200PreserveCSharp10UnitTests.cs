// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp6;
    using StyleCop.Analyzers.Test.CSharp9.OrderingRules;
    using Xunit;

    public partial class SA1200PreserveCSharp10UnitTests : SA1200PreserveCSharp9UnitTests
    {
        [Fact]
        public async Task TestValidUsingDirectivesInFileScopedNamespaceAsync()
        {
            var testCode = @"namespace TestNamespace;

using System;
using System.Threading;
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that having using directives in the compilation unit will not produce diagnostics, even if they could be
        /// moved inside a file-scoped namespace.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestIgnoredUsingDirectivesInCompilationUnitWithFileScopedNamespaceAsync()
        {
            var testCode = @"using System;
using System.Threading;

namespace TestNamespace;
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [InlineData("")]
        [InlineData("\n")]
        [InlineData("// A comment.\n")]
        [WorkItem(3875, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3875")]
        public async Task TestOnlyGlobalUsingDirectiveInFileAsync(string leadingTrivia)
        {
            var testCode = $@"{leadingTrivia}global using System;";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
