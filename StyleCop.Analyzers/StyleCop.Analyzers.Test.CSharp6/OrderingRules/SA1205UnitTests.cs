// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Lightup;
    using StyleCop.Analyzers.OrderingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1205PartialElementsMustDeclareAccess,
        StyleCop.Analyzers.OrderingRules.SA1205CodeFixProvider>;

    /// <summary>
    /// Unit tests for the <see cref="SA1205PartialElementsMustDeclareAccess"/> class.
    /// </summary>
    public class SA1205UnitTests
    {
        private const string TestCodeTemplate = @"$$ Foo
{
}
";

        private const string FixedTestCodeTemplate = @"## $$ Foo
{
}
";

        public static TheoryData<string> ValidDeclarations
        {
            get
            {
                var data = new TheoryData<string>()
                {
                    "public partial class",
                    "internal partial class",
                    "public static partial class",
                    "internal static partial class",
                    "public sealed partial class",
                    "internal sealed partial class",
                    "public partial struct",
                    "internal partial struct",
                    "public partial interface",
                    "internal partial interface",
                    "class",
                    "struct",
                    "interface",
                };

                if (LightupHelpers.SupportsCSharp9)
                {
                    data.Add("public partial record");
                    data.Add("internal partial record");
                    data.Add("public sealed partial record");
                    data.Add("internal sealed partial record");
                    data.Add("record");
                }

                if (LightupHelpers.SupportsCSharp10)
                {
                    data.Add("public partial record class");
                    data.Add("internal partial record class");
                    data.Add("public sealed partial record class");
                    data.Add("internal sealed partial record class");
                    data.Add("record class");

                    data.Add("public partial record struct");
                    data.Add("internal partial record struct");
                    data.Add("record struct");
                }

                return data;
            }
        }

        public static TheoryData<string> InvalidDeclarations
        {
            get
            {
                var data = new TheoryData<string>()
                {
                    "partial class",
                    "sealed partial class",
                    "static partial class",
                    "partial struct",
                    "partial interface",
                };

                if (LightupHelpers.SupportsCSharp9)
                {
                    data.Add("partial record");
                    data.Add("sealed partial record");
                }

                if (LightupHelpers.SupportsCSharp10)
                {
                    data.Add("partial record class");
                    data.Add("sealed partial record class");

                    data.Add("partial record struct");
                }

                return data;
            }
        }

        public static TheoryData<string, string> ValidNestedDeclarations
        {
            get
            {
                var data = new TheoryData<string, string>()
                {
                    { "public", "class" },
                    { "protected", "class" },
                    { "internal", "class" },
                    { "protected internal", "class" },
                    { "private", "class" },
                    { "public", "struct" },
                    { "protected", "struct" },
                    { "internal", "struct" },
                    { "protected internal", "struct" },
                    { "private", "struct" },
                    { "public", "interface" },
                    { "protected", "interface" },
                    { "internal", "interface" },
                    { "protected internal", "interface" },
                    { "private", "interface" },
                };

                if (LightupHelpers.SupportsCSharp72)
                {
                    data.Add("private protected", "class");
                    data.Add("private protected", "struct");
                    data.Add("private protected", "interface");
                }

                if (LightupHelpers.SupportsCSharp9)
                {
                    data.Add("public", "record");
                    data.Add("protected", "record");
                    data.Add("internal", "record");
                    data.Add("protected internal", "record");
                    data.Add("private", "record");
                    data.Add("private protected", "record");
                }

                if (LightupHelpers.SupportsCSharp10)
                {
                    data.Add("public", "record class");
                    data.Add("protected", "record class");
                    data.Add("internal", "record class");
                    data.Add("protected internal", "record class");
                    data.Add("private", "record class");
                    data.Add("private protected", "record class");

                    data.Add("public", "record struct");
                    data.Add("protected", "record struct");
                    data.Add("internal", "record struct");
                    data.Add("protected internal", "record struct");
                    data.Add("private", "record struct");
                    data.Add("private protected", "record struct");
                }

                return data;
            }
        }

        /// <summary>
        /// Verifies that a valid declaration (with an access modifier or not a partial type) will not produce a diagnostic.
        /// </summary>
        /// <param name="declaration">The declaration to verify.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [MemberData(nameof(ValidDeclarations))]
        public async Task TestValidDeclarationAsync(string declaration)
        {
            var testCode = TestCodeTemplate.Replace("$$", declaration);
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that an invalid type declaration will produce a diagnostic.
        /// </summary>
        /// <param name="declaration">The declaration to verify.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [MemberData(nameof(InvalidDeclarations))]
        public async Task TestInvalidDeclarationAsync(string declaration)
        {
            var testCode = TestCodeTemplate.Replace("$$", declaration);
            var fixedTestCode = FixedTestCodeTemplate.Replace("##", "internal").Replace("$$", declaration);

            await VerifyCSharpFixAsync(testCode, Diagnostic().WithLocation(1, 2 + declaration.Length), fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that the code fix will properly copy over the access modifier defined in another fragment of the partial element.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestProperAccessModifierPropagationAsync()
        {
            var testCode = @"public partial class Foo
{
    private int field1;
}

partial class Foo
{
    private int field2;
}
";

            var fixedTestCode = @"public partial class Foo
{
    private int field1;
}

public partial class Foo
{
    private int field2;
}
";

            await VerifyCSharpFixAsync(testCode, Diagnostic().WithLocation(6, 15), fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that the code fix will properly copy over the access modifier defined in another fragment of the partial element.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestCodeFixWithXmlDocumentationAsync()
        {
            var testCode = @"public partial class Foo
{
    private int field1;
}

/// <summary>
/// This is a summary
/// </summary>
partial class Foo
{
    private int field2;
}
";

            var fixedTestCode = @"public partial class Foo
{
    private int field1;
}

/// <summary>
/// This is a summary
/// </summary>
public partial class Foo
{
    private int field2;
}
";

            await VerifyCSharpFixAsync(testCode, Diagnostic().WithLocation(9, 15), fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that all 5 access modifiers are accepted for nested types.
        /// This is a regression test for issue #2040.
        /// </summary>
        /// <param name="accessModifier">The access modifier to use for the nested type.</param>
        /// <param name="typeKeyword">The type keyword to use.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [MemberData(nameof(ValidNestedDeclarations))]
        public async Task TestNestedTypeAccessModifiersAsync(string accessModifier, string typeKeyword)
        {
            var testCode = $@"
internal static partial class TestPartial
{{
    {accessModifier} partial {typeKeyword} PartialInner
    {{
    }}
}}
";

            var languageVersion = (LightupHelpers.SupportsCSharp8, LightupHelpers.SupportsCSharp72) switch
            {
                // Make sure to use C# 7.2 if supported, unless we are going to default to something greater
                (false, true) => LanguageVersionEx.CSharp7_2,
                _ => (LanguageVersion?)null,
            };

            await VerifyCSharpDiagnosticAsync(languageVersion, testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a nested type without access modifiers will produce a diagnostic and can be fixed correctly.
        /// </summary>
        /// <param name="declaration">The declaration to verify.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [MemberData(nameof(InvalidDeclarations))]
        public async Task TestNestedTypeWithoutAccessModifierAsync(string declaration)
        {
            var testCode = $@"
public class Foo
{{
    {declaration} Bar
    {{
    }}
}}
";

            var fixedTestCode = $@"
public class Foo
{{
    private {declaration} Bar
    {{
    }}
}}
";

            await VerifyCSharpFixAsync(testCode, Diagnostic().WithLocation(4, 6 + declaration.Length), fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that the code fix will properly copy over the access modifier defined in another fragment of the nested partial element.
        /// </summary>
        /// <param name="accessModifier">The access modifier to use for the nested type.</param>
        /// <param name="typeKeyword">The type keyword to use.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [MemberData(nameof(ValidNestedDeclarations))]
        public async Task TestProperNestedAccessModifierPropagationAsync(string accessModifier, string typeKeyword)
        {
            var testCode = $@"
public class Foo
{{
    {accessModifier} partial {typeKeyword} Bar
    {{
    }}

    partial {typeKeyword} Bar
    {{
    }}
}}
";

            var fixedTestCode = $@"
public class Foo
{{
    {accessModifier} partial {typeKeyword} Bar
    {{
    }}

    {accessModifier} partial {typeKeyword} Bar
    {{
    }}
}}
";

            var languageVersion = (LightupHelpers.SupportsCSharp8, LightupHelpers.SupportsCSharp72) switch
            {
                // Make sure to use C# 7.2 if supported, unless we are going to default to something greater
                (false, true) => LanguageVersionEx.CSharp7_2,
                _ => (LanguageVersion?)null,
            };

            await VerifyCSharpFixAsync(languageVersion, testCode, Diagnostic().WithLocation(8, 14 + typeKeyword.Length), fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
