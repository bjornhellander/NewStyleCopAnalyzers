// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1600ElementsMustBeDocumented,
        StyleCop.Analyzers.DocumentationRules.SA1600CodeFixProvider>;

    public partial class SA1600CSharp8UnitTests
    {
        // Using 'Default' here makes sure that later test projects also run these tests with their own language version, without having to override this property
        protected override LanguageVersion LanguageVersion => LanguageVersion.Default;

        /// <summary>
        /// Verifies that public default interface members (including static ones) need documentation just like any
        /// other interface member, while private default interface members follow
        /// <see cref="StyleCop.Analyzers.Settings.ObjectModel.DocumentationSettings.DocumentPrivateElements"/> instead.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestInterfaceMembersWithoutDocumentationAsync()
        {
            var testCode = @"/// <summary>
/// A summary.
/// </summary>
public interface ITest
{
    void [|TestMethod1|]()
    {
    }

    private void TestMethod2()
    {
    }

    static void [|TestMethod3|]()
    {
    }

    private static void TestMethod4()
    {
    }
}
";

            // Only the diagnostic is verified, as in every other SA1600 test: undocumented members also produce
            // CS1591 warnings, which the code fix verification would require to be declared here as well.
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that interface members are treated as implicitly public under "exposed" documentation mode.
        /// </summary>
        /// <param name="modifier">The interface's own accessibility modifier.</param>
        /// <param name="requiresDocumentation">A value indicating whether the members are expected to require documentation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [InlineData("public", true)]
        [InlineData("internal", false)]
        public async Task TestInterfaceExposedDocumentationModeCSharp8MembersAsync(string modifier, bool requiresDocumentation)
        {
            var testCode = $@"
{modifier} interface {{|#0:IInterface|}}
{{
    event System.Action {{|#1:MemberEvent|}}
    {{
        add {{ }}

        remove {{ }}
    }}

    static int {{|#2:MemberField|}};
}}
";

            var settings = @"
{
  ""settings"": {
    ""documentationRules"": {
      ""documentInterfaces"": ""exposed"",
      ""documentInternalElements"": false
    }
  }
}
";

            DiagnosticResult[] expected = requiresDocumentation
                ? new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(1), Diagnostic().WithLocation(2) }
                : DiagnosticResult.EmptyDiagnosticResults;

            await VerifyCSharpDiagnosticAsync(this.LanguageVersion, testCode, settings, expected, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a static field declared inside an interface requires documentation,
        /// whether or not it carries an explicit <c>public</c> modifier.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestInterfaceStaticFieldRequiresDocumentationAsync()
        {
            var testCode = @"
/// <summary>Summary.</summary>
public interface ITest
{
    public static int [|Field1|];

    static int [|Field2|];
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a delegate nested directly inside an interface requires documentation,
        /// whether or not it carries an explicit <c>public</c> modifier.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestInterfaceNestedDelegateRequiresDocumentationAsync()
        {
            var testCode = @"
/// <summary>Summary.</summary>
public interface ITest
{
    public delegate void [|Del1|]();

    delegate void [|Del2|]();
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a class, struct, or interface nested directly inside an interface requires documentation,
        /// whether or not it carries an explicit <c>public</c> modifier.
        /// </summary>
        /// <param name="typeKeyword">The type keyword to use.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [InlineData("class")]
        [InlineData("struct")]
        [InlineData("interface")]
        public async Task TestInterfaceNestedTypeRequiresDocumentationAsync(string typeKeyword)
        {
            var testCode = $@"
/// <summary>Summary.</summary>
public interface ITest
{{
    public {typeKeyword} [|Type1|] {{ }}

    {typeKeyword} [|Type2|] {{ }}
}}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a static constructor declared inside an interface does not require documentation. Unlike
        /// other unmarked interface members, a static constructor is not treated as implicitly public: it can never
        /// have an access modifier and can never be referenced from source regardless of its declaring type, so
        /// there is nothing for documentation to require.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestInterfaceStaticConstructorDoesNotRequireDocumentationAsync()
        {
            var testCode = @"
/// <summary>Summary.</summary>
public interface ITest
{
    static ITest() { }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a private default interface member requires documentation when
        /// <c>documentPrivateElements</c> is enabled, even though it would not by default.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestPrivateDefaultInterfaceMethodHonorsDocumentPrivateElementsAsync()
        {
            var testCode = @"
/// <summary>Summary.</summary>
public interface ITest
{
    private void [|M|]()
    {
    }
}
";

            var settings = @"
{
  ""settings"": {
    ""documentationRules"": {
      ""documentPrivateElements"": true
    }
  }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, settings, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
