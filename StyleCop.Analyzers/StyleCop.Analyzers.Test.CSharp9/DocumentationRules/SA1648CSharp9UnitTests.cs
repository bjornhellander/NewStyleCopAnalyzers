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
        StyleCop.Analyzers.DocumentationRules.SA1648InheritDocMustBeUsedWithInheritingClass>;

    public partial class SA1648CSharp9UnitTests
    {
        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestTypeWithPrimaryConstructorWithInvalidInheritDocAsync(string typeKeyword)
        {
            var testCode = $@"/// {{|#0:<inheritdoc/>|}}
public {typeKeyword} TestType(int X);";

            var expected = this.GetExpectedResultTestTypeWithPrimaryConstructorWithInvalidInheritDoc();
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestOverrideWithCovariantReturnTypeAsync()
        {
            var testCode = @"/// <summary>Base type.</summary>
public class BaseType
{
    /// <summary>Gets a value.</summary>
    /// <returns>The value.</returns>
    public virtual object Get() => null;
}

/// <summary>Derived type.</summary>
public class DerivedType : BaseType
{
    /// <inheritdoc/>
    public override string Get() => null;
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestTypeWithPrimaryConstructorWithInvalidInheritDoc()
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
