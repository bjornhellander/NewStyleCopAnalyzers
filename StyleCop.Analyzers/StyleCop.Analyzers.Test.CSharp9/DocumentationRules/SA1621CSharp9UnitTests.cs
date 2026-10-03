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
        StyleCop.Analyzers.DocumentationRules.GenericTypeParameterDocumentationAnalyzer>;

    public partial class SA1621CSharp9UnitTests
    {
        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestGenericTypeWithPrimaryConstructorTypeParameterWithoutNameAsync(string typeKeyword)
        {
            var testCode = $@"/// <summary>The type.</summary>
/// {{|#0:<typeparam>The type.</typeparam>|}}
public {typeKeyword} TestType<T>(T X);";

            var expected = this.GetExpectedResultTestGenericTypeWithPrimaryConstructorTypeParameterWithoutName();
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestGenericTypeWithPrimaryConstructorTypeParameterWithoutName()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/53136
                Diagnostic(StyleCop.Analyzers.DocumentationRules.GenericTypeParameterDocumentationAnalyzer.SA1621Descriptor).WithLocation(0),
                Diagnostic(StyleCop.Analyzers.DocumentationRules.GenericTypeParameterDocumentationAnalyzer.SA1621Descriptor).WithLocation(0),
            };
        }
    }
}
