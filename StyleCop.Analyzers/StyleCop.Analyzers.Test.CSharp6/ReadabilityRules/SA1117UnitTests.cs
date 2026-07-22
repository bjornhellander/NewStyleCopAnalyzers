// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<StyleCop.Analyzers.ReadabilityRules.SA1117ParametersMustBeOnSameLineOrSeparateLines>;

    public class SA1117UnitTests
    {
        public static TheoryData<string> GetTestDeclarations(string delimiter)
        {
            return new TheoryData<string>()
            {
                $"public Foo(int a, int b,{delimiter} {{|#0:string s|}}) {{ }}",
                $"public object Bar(int a, int b,{delimiter} {{|#0:string s|}}) => null;",
                $"public object this[int a, int b,{delimiter} {{|#0:string s|}}] => null;",
                $"public delegate void Bar(int a, int b,{delimiter} {{|#0:string s|}});",
            };
        }

        public static TheoryData<string> GetMultilineTestDeclarations(string delimiter)
        {
            return new TheoryData<string>()
            {
                $"public Foo(int a,{delimiter} string\r\ns) {{ }}",
                $"public object Bar(int a,{delimiter} string\r\ns) => null;",
                $"public object this[int a,{delimiter} string\r\ns] => null;",
                $"public delegate void Bar(int a,{delimiter} string\r\ns);",
            };
        }

        public static TheoryData<string> GetTestConstructorInitializers(string delimiter)
        {
            return new TheoryData<string>()
            {
                $"this(42, 43, {delimiter} {{|#0:\"hello\"|}})",
                $"base(42, 43, {delimiter} {{|#0:\"hello\"|}})",
            };
        }

        public static TheoryData<string> GetMultilineTestConstructorInitializers(string delimiter)
        {
            return new TheoryData<string>()
            {
                $"this(42\r\n+ 1, {delimiter} {{|#0:43|}}, {delimiter} \"hello\")",
                $"base(42\r\n+ 1, {delimiter} {{|#0:43|}}, {delimiter} \"hello\")",
            };
        }

        public static TheoryData<string> GetTestExpressions(string delimiter)
        {
            return new TheoryData<string>()
            {
                $"Bar(1, 2, {delimiter} {{|#0:2|}})",
                $"System.Action<int, int, int> func = (int x, int y, {delimiter} {{|#0:int z|}}) => Bar(x, y, z)",
                $"System.Action<int, int, int> func = delegate(int x, int y, {delimiter} {{|#0:int z|}}) {{ Bar(x, y, z); }}",
                $"new System.DateTime(2015, 9, {delimiter} {{|#0:14|}})",
                $"var arr = new string[2, 2, {delimiter} {{|#0:2|}}];",
                $"char cc = (new char[3, 3, 3])[2, 2,{delimiter} {{|#0:2|}}];",
                $"char? c = (new char[3, 3, 3])?[2, 2,{delimiter} {{|#0:2|}}];",
                $"long ll = this[2, 2,{delimiter} {{|#0:2|}}];",
            };
        }

        public static TheoryData<string> GetTrailingMultilineTestExpressions(string delimiter)
        {
            return new TheoryData<string>()
            {
                $"System.Action<int, int, int> func = (int x, {delimiter} int y, {delimiter} int\r\nz) => Bar(x, y, z)",
                $"System.Action<int, int, int> func = delegate(int x, {delimiter} int y, {delimiter} int\r\nz) {{ Bar(x, y, z); }}",
                $"var arr = new string[2, {delimiter} 2\r\n+ 2];",
                $"char cc = (new char[3, 3])[2, {delimiter} 2\r\n+ 2];",
                $"char? c = (new char[3, 3])?[2, {delimiter} 2\r\n+ 2];",
                $"long ll = this[2,{delimiter} 2,{delimiter} 2\r\n+ 1];",
                $"var str = string.Join(\r\n\"def\",{delimiter}\"abc\"\r\n + \"cba\");",
            };
        }

        public static TheoryData<string> GetLeadingMultilineTestExpressions(string delimiter)
        {
            return new TheoryData<string>()
            {
                $"var str = string.Join(\r\n\"abc\"\r\n + \"cba\",{delimiter}{{|#0:\"def\"|}});",
                $"Bar(\r\n1\r\n + 2,{delimiter}{{|#0:3|}},\r\n 4);",
            };
        }

        public static TheoryData<string> GetTestAttributes(string delimiter)
        {
            return new TheoryData<string>()
            {
                $"[MyAttribute(1, {delimiter}2, {{|#0:3|}})]",
            };
        }

        public static TheoryData<string> GetMultilineTestAttributes(string delimiter)
        {
            return new TheoryData<string>()
            {
                $"[MyAttribute(1, {delimiter}2, {delimiter}3\r\n+ 5)]",
            };
        }

        public static TheoryData<string> ValidTestExpressions() => new TheoryData<string>()
        {
            $"System.Action func = () => Bar(0, 2, 3)",
            $"System.Action<int> func = x => Bar(x, 2, 3)",
            $"System.Action func = delegate {{ Bar(0, 0, 0); }}",
            "var weird = new int[10][,,,];",
        };

        public static TheoryData<string> ValidTestDeclarations() => new TheoryData<string>()
        {
            $@"public Foo(
    int a, int b, string s) {{ }}",
            $@"public Foo(
    int a,
    int b,
    string s) {{ }}",
        };

        // This is a regression test for https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/1211
        public static TheoryData<string> ValidTestAttribute() => new TheoryData<string>()
        {
            @"[System.Obsolete]",
            @"[MyAttribute(
    1, 2, 3)]",
            @"[MyAttribute(
    1,
    2,
    3)]",
        };

        [Theory]
        [MemberData(nameof(GetTestDeclarations), "")]
        [MemberData(nameof(GetMultilineTestDeclarations), "\r\n")]
        [MemberData(nameof(GetMultilineTestDeclarations), "")]
        [MemberData(nameof(ValidTestDeclarations))]
        public async Task TestValidDeclarationAsync(string declaration)
        {
            var testCode = $@"
class Foo
{{
    {declaration}
}}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(GetTestDeclarations), "\r\n")]
        public async Task TestInvalidDeclarationAsync(string declaration)
        {
            var testCode = $@"
class Foo
{{
    {declaration}
}}";

            DiagnosticResult expected = Diagnostic().WithLocation(0);
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(GetTestConstructorInitializers), "")]
        [MemberData(nameof(GetMultilineTestConstructorInitializers), "\r\n")]
        public async Task TestValidConstructorInitializerAsync(string initializer)
        {
            var testCode = $@"
class Base
{{
    public Base(int a, int b, string s)
    {{
    }}
}}

class Derived : Base
{{
    public Derived()
        : {initializer}
    {{
    }}

    public Derived(int i, int j, string z)
        : base(i, j, z)
    {{
    }}
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(GetTestConstructorInitializers), "\r\n")]
        [MemberData(nameof(GetMultilineTestConstructorInitializers), "")]
        public async Task TestInvalidConstructorInitializerAsync(string initializer)
        {
            var testCode = $@"
class Base
{{
    public Base(int a, int b, string s)
    {{
    }}
}}

class Derived : Base
{{
    public Derived()
        : {initializer}
    {{
    }}

    public Derived(int i, int j, string z)
        : base(i, j, z)
    {{
    }}
}}";

            DiagnosticResult expected = Diagnostic().WithLocation(0);
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(GetTestExpressions), "")]
        [MemberData(nameof(GetLeadingMultilineTestExpressions), "\r\n")]
        [MemberData(nameof(GetTrailingMultilineTestExpressions), "\r\n")]
        [MemberData(nameof(GetTrailingMultilineTestExpressions), "")]
        [MemberData(nameof(ValidTestExpressions))]
        public async Task TestValidExpressionAsync(string expression)
        {
            var testCode = $@"
class Foo
{{
    public void Bar(int i, int j, int k)
    {{
    }}

    public void Baz()
    {{
        {expression};
    }}

    public long this[int a, int b, int s] => a + b + s;
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(GetTestExpressions), "\r\n")]
        [MemberData(nameof(GetLeadingMultilineTestExpressions), "")]
        public async Task TestInvalidExpressionAsync(string expression)
        {
            var testCode = $@"
class Foo
{{
    public void Bar(int i, int j, int k)
    {{
    }}

    public void Baz()
    {{
        {expression};
    }}

    public long this[int a, int b, int s] => a + b + s;
}}";

            DiagnosticResult expected = Diagnostic().WithLocation(0);
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(GetTestAttributes), "")]
        [MemberData(nameof(GetMultilineTestAttributes), "\r\n")]
        [MemberData(nameof(GetMultilineTestAttributes), "")]
        [MemberData(nameof(ValidTestAttribute))]
        public async Task TestValidAttributeAsync(string attribute)
        {
            var testCode = $@"
[System.AttributeUsage(System.AttributeTargets.Class)]
public class MyAttribute : System.Attribute
{{
    public MyAttribute(int a, int b, int c)
    {{
    }}
}}

{attribute}
class Foo
{{
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(GetTestAttributes), "\r\n")]
        public async Task TestInvalidAttributeAsync(string attribute)
        {
            var testCode = $@"
[System.AttributeUsage(System.AttributeTargets.Class)]
public class MyAttribute : System.Attribute
{{
    public MyAttribute(int a, int b, int c)
    {{
    }}
}}

{attribute}
class Foo
{{
}}";

            DiagnosticResult expected = Diagnostic().WithLocation(0);

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
