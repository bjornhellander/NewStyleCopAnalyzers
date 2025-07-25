﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp7.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.Helpers;
    using StyleCop.Analyzers.Test.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1131UseReadableConditions,
        StyleCop.Analyzers.ReadabilityRules.SA1131CodeFixProvider>;

    public partial class SA1131CSharp7UnitTests : SA1131UnitTests
    {
        [Theory]
        [InlineData("==", "==")]
        [InlineData("!=", "!=")]
        [WorkItem(2675, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2675")]
        public async Task TestDefaultLiteralStructComparismOutsideIfAsync(string oldOperator, string newOperator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        TestStruct i = default;
        bool b = default {oldOperator} i;
    }}
}}

struct TestStruct 
{{
    public static bool operator == (TestStruct a, TestStruct b) {{ return true; }}
    public static bool operator != (TestStruct a, TestStruct b) {{ return false; }}
}}
";
            var fixedCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        TestStruct i = default;
        bool b = i {newOperator} default;
    }}
}}

struct TestStruct 
{{
    public static bool operator == (TestStruct a, TestStruct b) {{ return true; }}
    public static bool operator != (TestStruct a, TestStruct b) {{ return false; }}
}}
";
            DiagnosticResult expected = Diagnostic().WithLocation(8, 18);
            await VerifyCSharpFixAsync(LanguageVersion.CSharp7_1.OrLaterDefault(), testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
