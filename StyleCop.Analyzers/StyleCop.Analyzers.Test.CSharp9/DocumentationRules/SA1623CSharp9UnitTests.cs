// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.DocumentationRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.DocumentationRules.PropertySummaryDocumentationAnalyzer,
        StyleCop.Analyzers.DocumentationRules.PropertySummaryDocumentationCodeFixProvider>;

    public partial class SA1623CSharp9UnitTests
    {
        /// <summary>
        /// Verifies that an <c>init</c> accessor is not treated as a <c>set</c> accessor, so a property with a
        /// <c>get</c> and an <c>init</c> accessor is documented like a get-only property.
        /// </summary>
        /// <param name="accessors">The accessors for the property.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [InlineData("{ get; init; }")]
        [InlineData("{ get; private init; }")]
        public async Task TestGetAndInitAccessorsAsync(string accessors)
        {
            var testCode = $@"
public class TestClass
{{
    /// <summary>
    /// Gets or sets the first test value.
    /// </summary>
    public int {{|#0:TestProperty|}} {accessors}
}}
";

            var fixedCode = $@"
public class TestClass
{{
    /// <summary>
    /// Gets the first test value.
    /// </summary>
    public int TestProperty {accessors}
}}
";

            var expected = Diagnostic(PropertySummaryDocumentationAnalyzer.SA1623Descriptor).WithLocation(0).WithArguments("Gets");
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that the summary of a property with only an <c>init</c> accessor is not checked.
        /// </summary>
        /// <param name="summary">The summary text.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [InlineData("Sets the first test value.")]
        [InlineData("Gets the first test value.")]
        [InlineData("Gets or sets the first test value.")]
        [InlineData("The first test value.")]
        public async Task TestInitAccessorOnlyAsync(string summary)
        {
            var testCode = $@"
public class TestClass
{{
    /// <summary>
    /// {summary}
    /// </summary>
    public int TestProperty {{ init {{ }} }}
}}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
