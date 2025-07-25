﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1131UseReadableConditions,
        StyleCop.Analyzers.ReadabilityRules.SA1131CodeFixProvider>;

    public class SA1131UnitTests
    {
        [Theory]
        [InlineData("==", "==")]
        [InlineData("!=", "!=")]
        [InlineData(">=", "<=")]
        [InlineData("<=", ">=")]
        [InlineData(">", "<")]
        [InlineData("<", ">")]
        public async Task TestYodaComparismAsync(string oldOperator, string newOperator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        int i = 5;
        const int j = 6;
        if (j {oldOperator} i) {{ }}
    }}
}}";
            var fixedCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        int i = 5;
        const int j = 6;
        if (i {newOperator} j) {{ }}
    }}
}}";

            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(9, 13),
            };
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("==", "==")]
        [InlineData("!=", "!=")]
        [InlineData(">=", "<=")]
        [InlineData("<=", ">=")]
        [InlineData(">", "<")]
        [InlineData("<", ">")]
        public async Task TestYodaComparismAsAnArgumentAsync(string oldOperator, string newOperator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        int i = 5;
        const int j = 6;
        Test(j {oldOperator} i);
    }}
    public void Test(bool argument) {{ }}
}}";
            var fixedCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        int i = 5;
        const int j = 6;
        Test(i {newOperator} j);
    }}
    public void Test(bool argument) {{ }}
}}";
            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(9, 14),
            };
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("==", "==")]
        [InlineData("!=", "!=")]
        [InlineData(">=", "<=")]
        [InlineData("<=", ">=")]
        [InlineData(">", "<")]
        [InlineData("<", ">")]
        public async Task TestYodaComparismOutsideIfAsync(string oldOperator, string newOperator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        int i = 5;
        const int j = 6;
        bool b = j {oldOperator} i;
    }}
}}";
            var fixedCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        int i = 5;
        const int j = 6;
        bool b = i {newOperator} j;
    }}
}}";
            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(9, 18),
            };
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("==", "==")]
        [InlineData("!=", "!=")]
        [InlineData(">=", "<=")]
        [InlineData("<=", ">=")]
        [InlineData(">", "<")]
        [InlineData("<", ">")]
        public async Task TestDefaultComparismOutsideIfAsync(string oldOperator, string newOperator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        int i = 5;
        bool b = default(int) {oldOperator} i;
    }}
}}";
            var fixedCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        int i = 5;
        bool b = i {newOperator} default(int);
    }}
}}";
            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(8, 18),
            };
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("==", "==")]
        [InlineData("!=", "!=")]
        [InlineData(">=", "<=")]
        [InlineData("<=", ">=")]
        [InlineData(">", "<")]
        [InlineData("<", ">")]
        public async Task TestStaticReadOnlyAsync(string oldOperator, string newOperator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    static readonly int j = 5;
    public void Test()
    {{
        int i = 5;
        bool b = j {oldOperator} i;
    }}
}}";
            var fixedCode = $@"
using System;
public class TypeName
{{
    static readonly int j = 5;
    public void Test()
    {{
        int i = 5;
        bool b = i {newOperator} j;
    }}
}}";
            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(9, 18),
            };
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("==", "==")]
        [InlineData("!=", "!=")]
        public async Task TestDefaultStringComparismOutsideIfAsync(string oldOperator, string newOperator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        string i = ""x"";
        bool b = default(string) {oldOperator} i;
    }}
}}";
            var fixedCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        string i = ""x"";
        bool b = i {newOperator} default(string);
    }}
}}";
            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(8, 18),
            };
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("==", "==")]
        [InlineData("!=", "!=")]
        public async Task TestNullComparismOutsideIfAsync(string oldOperator, string newOperator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        string i = ""x"";
        bool b = null {oldOperator} i;
    }}
}}";
            var fixedCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        string i = ""x"";
        bool b = i {newOperator} null;
    }}
}}";
            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(8, 18),
            };
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("==", "==")]
        [InlineData("!=", "!=")]
        public async Task TestDefaultStructComparismOutsideIfAsync(string oldOperator, string newOperator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        TestStruct i = default(TestStruct);
        bool b = default(TestStruct) {oldOperator} i;
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
        TestStruct i = default(TestStruct);
        bool b = i {newOperator} default(TestStruct);
    }}
}}

struct TestStruct 
{{
    public static bool operator == (TestStruct a, TestStruct b) {{ return true; }}
    public static bool operator != (TestStruct a, TestStruct b) {{ return false; }}
}}
";
            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(8, 18),
            };
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCorrectComparismAsync()
        {
            var testCode = @"
using System;
public class TypeName
{
    public void Test()
    {
        const int i = 5;
        int j = 6;
        if (j == i) { }
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("==")]
        [InlineData("!=")]
        [InlineData(">=")]
        [InlineData("<=")]
        [InlineData(">")]
        [InlineData("<")]
        public async Task TestCorrectComparismOutsideIfAsync(string @operator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        const int i = 5;
        int j = 6;
        bool b = j {@operator} i;
    }}
}}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("==")]
        [InlineData("!=")]
        [InlineData(">=")]
        [InlineData("<=")]
        [InlineData(">")]
        [InlineData("<")]
        public async Task TestStaticFieldAsync(string @operator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    static int i = 5;
    public void Test()
    {{
        int j = 6;
        bool b = i {@operator} j;
    }}
}}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("==")]
        [InlineData("!=")]
        [InlineData(">=")]
        [InlineData("<=")]
        [InlineData(">")]
        [InlineData("<")]
        public async Task TestReadOnlyFieldAsync(string @operator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    readonly int i = 5;
    public void Test()
    {{
        int j = 6;
        bool b = i {@operator} j;
    }}
}}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("==")]
        [InlineData("!=")]
        [InlineData(">=")]
        [InlineData("<=")]
        [InlineData(">")]
        [InlineData("<")]
        public async Task TestNormalFieldAsync(string @operator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    int i = 5;
    public void Test()
    {{
        int j = 6;
        bool b = i {@operator} j;
    }}
}}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("==")]
        [InlineData("!=")]
        [InlineData(">=")]
        [InlineData("<=")]
        [InlineData(">")]
        [InlineData("<")]
        public async Task TestCorrectComparismNoConstantAsync(string @operator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        int i = 5;
        int j = 6;
        if (j {@operator} i) {{ }}
    }}
}}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("==")]
        [InlineData("!=")]
        [InlineData(">=")]
        [InlineData("<=")]
        [InlineData(">")]
        [InlineData("<")]
        public async Task TestCorrectComparismOutsideIfNoConstantAsync(string @operator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        int i = 5;
        int j = 6;
        bool b = j {@operator} i;
    }}
}}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("==", "==")]
        [InlineData("!=", "!=")]
        [InlineData(">=", "<=")]
        [InlineData("<=", ">=")]
        [InlineData(">", "<")]
        [InlineData("<", ">")]
        public async Task TestCommentsAsync(string oldOperator, string newOperator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        int i = 5;
        const int j = 6;
        bool b = /*1*/j/*2*/{oldOperator}/*3*/i/*4*/;
    }}
}}";
            var fixedCode = $@"
using System;
public class TypeName
{{
    public void Test()
    {{
        int i = 5;
        const int j = 6;
        bool b = /*1*/i/*2*/{newOperator}/*3*/j/*4*/;
    }}
}}";
            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(9, 23),
            };
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("Method1", "arg", true)]
        [InlineData("Method2", "arg", true)]
        [InlineData("Method1", "field1", true)]
        [InlineData("Method2", "field1", true)]
        [InlineData("Method1", "field2", true)]
        [InlineData("Method2", "field2", true)]
        [InlineData("Const1", "Method1", false)]
        [InlineData("Const1", "Method2", false)]
        [InlineData("Method1", "Const1", false)]
        [InlineData("Method2", "Const1", false)]
        [WorkItem(3677, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3677")]
        public async Task TestSimpleMethodsAsync(string expr1, string expr2, bool shouldTrigger)
        {
            await this.TestMethodAsync(expr1, expr2, shouldTrigger).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("TestClass.Method1", "arg", true)]
        [InlineData("this.Method2", "arg", true)]
        [InlineData("TestClass.Method1", "field1", true)]
        [InlineData("this.Method2", "field1", true)]
        [InlineData("TestClass.Method1", "field2", true)]
        [InlineData("this.Method2", "field2", true)]
        [InlineData("Const1", "TestClass.Method1", false)]
        [InlineData("Const1", "this.Method2", false)]
        [InlineData("TestClass.Method1", "Const1", false)]
        [InlineData("this.Method2", "Const1", false)]
        [InlineData("Method3<int>", "arg", true)]
        [InlineData("TestClass.Method3<int>", "arg", true)]
        [WorkItem(3759, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3759")]
        public async Task TestComplexMethodsAsync(string expr1, string expr2, bool shouldTrigger)
        {
            await this.TestMethodAsync(expr1, expr2, shouldTrigger).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("==")]
        [InlineData("!=")]
        [InlineData(">=")]
        [InlineData("<=")]
        [InlineData(">")]
        [InlineData("<")]
        [WorkItem(3759, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3759")]
        public async Task TestComplexLeftHandSideExpressionAsync(string @operator)
        {
            var testCode = $@"
using System;
public class TypeName
{{
    public void Test(int x, int y, Func<int> a)
    {{
        var r1 = x + 1 {@operator} y;
        var r2 = -x {@operator} y;
        var r3 = a() {@operator} y;
    }}
}}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        private async Task TestMethodAsync(string expr1, string expr2, bool shouldTrigger)
        {
            var testExpr = $"{expr1} == {expr2}";
            var testCode = $@"
using System;

public class TestClass
{{
    private static readonly Action Const1 = Method1;

    private Action field1 = Method1;
    private readonly Action field2 = Method1;

    public bool TestMethod(Action arg)
    {{
        return {(shouldTrigger ? $"[|{testExpr}|]" : testExpr)};
    }}

    private static void Method1()
    {{
    }}

    private void Method2()
    {{
    }}

    private static void Method3<T>()
    {{
    }}
}}
";

            var fixedExpr = $"{expr2} == {expr1}";
            var fixedCode = $@"
using System;

public class TestClass
{{
    private static readonly Action Const1 = Method1;

    private Action field1 = Method1;
    private readonly Action field2 = Method1;

    public bool TestMethod(Action arg)
    {{
        return {fixedExpr};
    }}

    private static void Method1()
    {{
    }}

    private void Method2()
    {{
    }}

    private static void Method3<T>()
    {{
    }}
}}
";

            if (shouldTrigger)
            {
                await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(false);
            }
            else
            {
                await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            }
        }
    }
}
