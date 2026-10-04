// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1642ConstructorSummaryDocumentationMustBeginWithStandardText,
        StyleCop.Analyzers.DocumentationRules.SA1642SA1643CodeFixProvider>;

    public partial class SA1642CSharp10UnitTests
    {
        // TODO: A theory in a base test class might have made this unnecessary
        [Fact]
        public async Task TestParameterlessStructConstructorAsync()
        {
            var testCode = @"
public struct TestStruct
{
    /// [|<summary>
    /// Creates a new value.
    /// </summary>|]
    public TestStruct()
    {
    }
}
";

            var fixedCode = @"
public struct TestStruct
{
    /// <summary>
    /// Initializes a new instance of the <see cref=""TestStruct""/> struct.
    /// Creates a new value.
    /// </summary>
    public TestStruct()
    {
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
