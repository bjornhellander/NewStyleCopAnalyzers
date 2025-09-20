// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp13.DocumentationRules;
    using Xunit;

    public partial class SA1619CSharp14UnitTests : SA1619CSharp13UnitTests
    {
        [Fact]
        public async Task TestExtensionDeclarationWithoutTypeParametersAsync()
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
public static partial class TestClass
{
    /// <summary>
    /// Foo
    /// </summary>
    extension(string obj)
    {
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestExtensionDeclarationWithMissingDocumentationAsync()
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
public static partial class TestClass
{
    /// <summary>
    /// Foo
    /// </summary>
    extension<T>(T obj)
    {
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestExtensionDeclarationWithDocumentationAsync()
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
public static partial class TestClass
{
    /// <summary>
    /// Foo
    /// </summary>
    /// <typeparam name=""T"">Param 1</param>
    extension<T>(T obj)
    {
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
