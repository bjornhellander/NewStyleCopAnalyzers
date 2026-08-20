// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<StyleCop.Analyzers.DocumentationRules.SA1615ElementReturnValueMustBeDocumented>;

    public partial class SA1615CSharp13UnitTests
    {
        [Fact]
        public async Task TestMethodWithParamsCollectionParameterMissingReturnsDocumentationAsync()
        {
            var testCode = @"
using System.Collections.Generic;

/// <summary>
/// Foo
/// </summary>
public class ClassName
{
    /// <summary>
    /// Foo
    /// </summary>
    /// <param name=""values"">The values.</param>
    public [|int|] TestMethod(params IEnumerable<int> values) { return 0; }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
