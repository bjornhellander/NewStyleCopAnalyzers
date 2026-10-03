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
        StyleCop.Analyzers.DocumentationRules.SA1618GenericTypeParametersMustBeDocumented>;

    public partial class SA1618CSharp9UnitTests
    {
        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestGenericTypeWithPrimaryConstructorWithoutTypeParameterDocumentationAsync(string typeKeyword)
        {
            var testCode = $@"/// <summary>The type.</summary>
public {typeKeyword} TestType<{{|#0:T|}}>(T X);";

            var expected = this.GetExpectedResultTestGenericTypeWithPrimaryConstructorWithoutTypeParameterDocumentation();
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestGenericTypeWithPrimaryConstructorWithoutTypeParameterDocumentation()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/53136
                Diagnostic().WithLocation(0).WithArguments("T"),
                Diagnostic().WithLocation(0).WithArguments("T"),
            };
        }
    }
}
