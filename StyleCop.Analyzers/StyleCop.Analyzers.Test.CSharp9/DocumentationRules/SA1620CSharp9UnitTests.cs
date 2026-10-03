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

    public partial class SA1620CSharp9UnitTests
    {
        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestGenericTypeWithPrimaryConstructorWithWrongTypeParameterNameAsync(string typeKeyword)
        {
            var testCode = $@"/// <summary>The type.</summary>
/// <typeparam name=""T"">The type.</typeparam>
/// <typeparam name=""{{|#0:U|}}"">Another type.</typeparam>
public {typeKeyword} TestType<T>(T X);";

            var expected = this.GetExpectedResultTestGenericTypeWithPrimaryConstructorWithWrongTypeParameterName();
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestGenericTypeWithPrimaryConstructorWithWrongTypeParameterName()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/53136
                Diagnostic(StyleCop.Analyzers.DocumentationRules.GenericTypeParameterDocumentationAnalyzer.SA1620MissingTypeParameterDescriptor).WithLocation(0).WithArguments("U"),
                Diagnostic(StyleCop.Analyzers.DocumentationRules.GenericTypeParameterDocumentationAnalyzer.SA1620MissingTypeParameterDescriptor).WithLocation(0).WithArguments("U"),
            };
        }
    }
}
