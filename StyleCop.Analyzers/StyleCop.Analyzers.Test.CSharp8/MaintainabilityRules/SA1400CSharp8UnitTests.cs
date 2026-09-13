// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp6.Helpers;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1400AccessModifierMustBeDeclared,
        StyleCop.Analyzers.MaintainabilityRules.SA1400CodeFixProvider>;

    public partial class SA1400CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that no access modifier is required on an interface member, including the kinds of member that
        /// C# 8 added: a method with a default implementation, a static method, a static field and a delegate.
        /// Interface members are implicitly public and the rule deliberately leaves all of them alone.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestInterfaceMembersAsync()
        {
            var testCode = @"public interface ITest
{
    static int Field;

    static ITest() { }

    int Property { get; set; }

    event System.EventHandler Event;

    void Method();

    void DefaultMethod()
    {
    }

    static void StaticMethod()
    {
    }

    delegate void Handler();
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that no access modifier is required on a type declared inside an interface.
        /// </summary>
        /// <param name="typeKind">The keyword introducing the nested type declaration.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [MemberData(nameof(CommonMemberData.BaseTypeDeclarationKeywords), MemberType = typeof(CommonMemberData))]
        public async Task TestTypeDeclarationInsideInterfaceAsync(string typeKind)
        {
            var testCode = $@"
public interface ITest
{{
    {typeKind} NestedType
    {{
    }}
}}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
