// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1600ElementsMustBeDocumented>;

    public partial class SA1600CSharp13UnitTests
    {
        [Fact]
        public async Task TestRefStructExplicitInterfaceImplementationWithDocumentationAsync()
        {
            var testCode = @"
/// <summary>
/// A summary.
/// </summary>
public interface IInterface
{
    /// <summary>
    /// A summary.
    /// </summary>
    void TestMethod();
}

/// <summary>
/// A summary.
/// </summary>
public ref struct TestRefStruct : IInterface
{
    /// <summary>
    /// A summary.
    /// </summary>
    void IInterface.TestMethod()
    {
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestRefStructExplicitInterfaceImplementationMissingDocumentationAsync()
        {
            var testCode = @"
/// <summary>
/// A summary.
/// </summary>
public interface IInterface
{
    /// <summary>
    /// A summary.
    /// </summary>
    void TestMethod();
}

/// <summary>
/// A summary.
/// </summary>
public ref struct TestRefStruct : IInterface
{
    void IInterface.[|TestMethod|]()
    {
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
