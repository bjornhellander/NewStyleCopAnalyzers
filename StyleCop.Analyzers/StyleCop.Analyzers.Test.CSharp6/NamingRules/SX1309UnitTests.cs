// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SX1309FieldNamesMustBeginWithUnderscore,
        StyleCop.Analyzers.NamingRules.SX1309CodeFixProvider>;

    public class SX1309UnitTests
    {
        public static TheoryData<string> CheckedModifiersData { get; } = new TheoryData<string>()
        {
            string.Empty,
            "private",
            "readonly",
            "private readonly",
        };

        public static TheoryData<string> UncheckedModifiersData { get; } = new TheoryData<string>()
        {
            "static",
            "public static",
            "protected static",
            "internal static",
            "protected internal static",
            "private static",
            "static readonly",
            "public static readonly",
            "protected static readonly",
            "internal static readonly",
            "protected internal static readonly",
            "private static readonly",
            "const",
            "public const",
            "protected const",
            "internal const",
            "protected internal const",
            "private const",
            "public",
            "protected",
            "internal",
            "protected internal",
            "public readonly",
            "protected readonly",
            "internal readonly",
            "protected internal readonly",
        };

        [Theory]
        [MemberData(nameof(CheckedModifiersData))]
        public async Task TestCheckedFieldNotStartingWithAnUnderscoreAsync(string modifiers)
        {
            var testCode = $@"public class ClassName
{{
    {modifiers} string bar = ""bar"";
}}";

            DiagnosticResult expected = Diagnostic().WithArguments("bar").WithLocation(3, 13 + modifiers.Length);

            var fixedCode = $@"public class ClassName
{{
    {modifiers} string _bar = ""bar"";
}}";

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(UncheckedModifiersData))]
        public async Task TestUncheckedFieldNotStartingWithAnUnderscoreAsync(string modifiers)
        {
            var testCode = $@"public class ClassName
{{
    {modifiers} string bar = ""bar"";
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CheckedModifiersData))]
        [MemberData(nameof(UncheckedModifiersData))]
        public async Task TestFieldNotStartingWithAnUnderscorePlacedInsideNativeMethodsClassAsync(string modifiers)
        {
            var testCode = $@"public class ClassNameNativeMethods
{{
    {modifiers} string bar = ""bar"";
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CheckedModifiersData))]
        public async Task TestCheckedFieldNotStartingWithAnUnderscorePlacedInsideNativeMethodsClassWithIncorrectNameAsync(string modifiers)
        {
            var testCode = $@"public class ClassNameNativeMethodsClass
{{
    {modifiers} string bar = ""bar"";
}}";

            DiagnosticResult expected = Diagnostic().WithArguments("bar").WithLocation(3, 13 + modifiers.Length);

            var fixedCode = $@"public class ClassNameNativeMethodsClass
{{
    {modifiers} string _bar = ""bar"";
}}";

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(UncheckedModifiersData))]
        public async Task TestUncheckedFieldNotStartingWithAnUnderscorePlacedInsideNativeMethodsClassWithIncorrectNameAsync(string modifiers)
        {
            var testCode = $@"public class ClassNameNativeMethodsClass
{{
    {modifiers} string bar = ""bar"";
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CheckedModifiersData))]
        [MemberData(nameof(UncheckedModifiersData))]
        public async Task TestFieldNotStartingWithAnUnderscorePlacedInsideOuterNativeMethodsClassAsync(string modifiers)
        {
            var testCode = $@"public class ClassNameNativeMethods
{{
    public class ClassName
    {{
        {modifiers} string bar = ""bar"";
    }}
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
