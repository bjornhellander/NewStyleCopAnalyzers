// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp6.Helpers;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1601PartialElementsMustBeDocumented>;

    public partial class SA1601CSharp9UnitTests
    {
        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestPartialTypeWithPrimaryConstructorWithoutDocumentationAsync(string typeKeyword)
        {
            var testCode = $@"public partial {typeKeyword} {{|#0:TestType|}}(int X);";

            var expected = this.GetExpectedResultTestPartialTypeWithPrimaryConstructorWithoutDocumentation();
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that both parts of an undocumented partial method with an access modifier are reported.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestPartialMethodWithAccessModifierWithoutDocumentationAsync()
        {
            var testCode = @"/// <summary>
/// Summary.
/// </summary>
public partial class TestClass
{
    public partial int [|TestMethod|](out int x);
}

/// <summary>
/// Summary.
/// </summary>
public partial class TestClass
{
    public partial int [|TestMethod|](out int x)
    {
        x = 0;
        return 0;
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a private partial method with an explicit access modifier is handled like other private
        /// elements, i.e. only reported when <c>documentPrivateElements</c> is enabled.
        /// </summary>
        /// <param name="settingEnabled">The value of the <c>documentPrivateElements</c> setting.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public async Task TestPrivatePartialMethodWithAccessModifierAsync(bool settingEnabled)
        {
            var testCode = @"/// <summary>
/// Summary.
/// </summary>
public partial class TestClass
{
    private partial bool {|#0:TestMethod|}();
}

/// <summary>
/// Summary.
/// </summary>
public partial class TestClass
{
    private partial bool {|#1:TestMethod|}() => true;
}
";

            var settings = $@"
{{
  ""settings"": {{
    ""documentationRules"": {{
      ""documentPrivateElements"": {(settingEnabled ? "true" : "false")}
    }}
  }}
}}
";

            var expected = settingEnabled
                ? new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(1) }
                : DiagnosticResult.EmptyDiagnosticResults;

            await VerifyCSharpDiagnosticAsync(languageVersion: null, testCode, settings, expected, CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestPartialTypeWithPrimaryConstructorWithoutDocumentation()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/53136
                Diagnostic().WithLocation(0),
                Diagnostic().WithLocation(0),
            };
        }
    }
}
