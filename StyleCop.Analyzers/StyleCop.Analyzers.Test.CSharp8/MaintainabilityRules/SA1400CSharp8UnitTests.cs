// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1400AccessModifierMustBeDeclared,
        StyleCop.Analyzers.MaintainabilityRules.SA1400CodeFixProvider>;

    public partial class SA1400CSharp8UnitTests
    {
        /// <summary>
        /// Verifies that no access modifier is required on an interface member, including the kinds of member that
        /// C# 8 added: a method with a default implementation, a static method and a static field. Interface
        /// members are implicitly public, so the rule deliberately leaves all of them alone.
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
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
