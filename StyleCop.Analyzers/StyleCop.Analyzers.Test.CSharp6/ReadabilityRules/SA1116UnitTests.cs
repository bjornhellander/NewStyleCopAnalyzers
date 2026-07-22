// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1116SplitParametersMustStartOnLineAfterDeclaration,
        StyleCop.Analyzers.ReadabilityRules.SA1116CodeFixProvider>;

    public class SA1116UnitTests
    {
        public static TheoryData<string, string, int> GetTestDeclarations(string delimiter, string fixDelimiter)
        {
            return new TheoryData<string, string, int>()
            {
                { $"public Foo(int a,{delimiter} string s) {{ }}", $"public Foo({fixDelimiter}int a,{delimiter} string s) {{ }}", 16 },
                { $"public object Bar(int a,{delimiter} string s) => null;", $"public object Bar({fixDelimiter}int a,{delimiter} string s) => null;", 23 },
                { $"public static Foo operator + (Foo a,{delimiter} Foo b) => null;", $"public static Foo operator + ({fixDelimiter}Foo a,{delimiter} Foo b) => null;", 35 },
                { $"public object this[int a,{delimiter} string s] => null;", $"public object this[{fixDelimiter}int a,{delimiter} string s] => null;", 24 },
                { $"public delegate void Bar(int a,{delimiter} string s);", $"public delegate void Bar({fixDelimiter}int a,{delimiter} string s);", 30 },
            };
        }

        public static TheoryData<string, string> GetTestConstructorInitializers(string delimiter, string fixDelimiter)
        {
            return new TheoryData<string, string>()
            {
                { $"this(42,{delimiter} \"hello\")", $"this({fixDelimiter}42,{delimiter} \"hello\")" },
                { $"base(42,{delimiter} \"hello\")", $"base({fixDelimiter}42,{delimiter} \"hello\")" },
            };
        }

        public static TheoryData<string, string, int> GetTestExpressions(string delimiter, string fixDelimiter)
        {
            return new TheoryData<string, string, int>()
            {
                { $"Bar(1,{delimiter} 2)", $"Bar({fixDelimiter}1,{delimiter} 2)", 13 },
                { $"System.Action<int, int> func = (int x,{delimiter} int y) => Bar(x, y)", $"System.Action<int, int> func = ({fixDelimiter}int x,{delimiter} int y) => Bar(x, y)", 41 },
                { $"System.Action<int, int> func = delegate(int x,{delimiter} int y) {{ Bar(x, y); }}", $"System.Action<int, int> func = delegate({fixDelimiter}int x,{delimiter} int y) {{ Bar(x, y); }}", 49 },
                { $"new string('a',{delimiter} 2)", $"new string({fixDelimiter}'a',{delimiter} 2)", 20 },
                { $"var arr = new string[2,{delimiter} 2];", $"var arr = new string[{fixDelimiter}2,{delimiter} 2];", 30 },
                { $"char cc = (new char[3, 3])[2,{delimiter} 2];", $"char cc = (new char[3, 3])[{fixDelimiter}2,{delimiter} 2];", 36 },
                { $"char? c = (new char[3, 3])?[2,{delimiter} 2];", $"char? c = (new char[3, 3])?[{fixDelimiter}2,{delimiter} 2];", 37 },
                { $"long ll = this[2,{delimiter} 2];", $"long ll = this[{fixDelimiter}2,{delimiter} 2];", 24 },
            };
        }

        public static TheoryData<string, string?, int> ValidTestExpressions()
        {
            return new TheoryData<string, string?, int>()
            {
                { $"System.Action func = () => Bar(0, 3)", null, 0 },
                { $"System.Action<int> func = x => Bar(x, 3)", null, 0 },
                { $"System.Action func = delegate {{ Bar(0, 0); }}", null, 0 },
            };
        }

        [Theory]
        [MemberData(nameof(GetTestDeclarations), "", "")]
        public async Task TestValidDeclarationAsync(string declaration, string fixedDeclaration, int column)
        {
            // Not needed for this test
            _ = fixedDeclaration;
            _ = column;

            var testCode = $@"
class Foo
{{
    {declaration}
}}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(GetTestDeclarations), "\r\n", "\r\n        ")]
        public async Task TestInvalidDeclarationAsync(string declaration, string fixedDeclaration, int column)
        {
            var testCode = $@"
class Foo
{{
    {declaration}
}}";
            var fixedCode = $@"
class Foo
{{
    {fixedDeclaration}
}}";
            DiagnosticResult expected = Diagnostic().WithLocation(4, column);
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(GetTestConstructorInitializers), "", "")]
        public async Task TestValidConstructorInitializerAsync(string initializer, string fixedInitializer)
        {
            // Not needed for this test
            _ = fixedInitializer;

            var testCode = $@"
class Base
{{
    public Base(int a, string s)
    {{
    }}
}}

class Derived : Base
{{
    public Derived()
        : {initializer}
    {{
    }}

    public Derived(int i, string z)
        : base(i, z)
    {{
    }}
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(GetTestConstructorInitializers), "\r\n", "\r\n            ")]
        public async Task TestInvalidConstructorInitializerAsync(string initializer, string fixedInitializer)
        {
            var testCode = $@"
class Base
{{
    public Base(int a, string s)
    {{
    }}
}}

class Derived : Base
{{
    public Derived()
        : {initializer}
    {{
    }}

    public Derived(int i, string z)
        : base(i, z)
    {{
    }}
}}";
            var fixedCode = $@"
class Base
{{
    public Base(int a, string s)
    {{
    }}
}}

class Derived : Base
{{
    public Derived()
        : {fixedInitializer}
    {{
    }}

    public Derived(int i, string z)
        : base(i, z)
    {{
    }}
}}";

            DiagnosticResult expected = Diagnostic().WithLocation(12, 16);
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(GetTestExpressions), "", "")]
        [MemberData(nameof(ValidTestExpressions))]
        public async Task TestValidExpressionAsync(string expression, string? fixedExpression, int column)
        {
            // Not needed for this test
            _ = fixedExpression;
            _ = column;

            var testCode = $@"
class Foo
{{
    public void Bar(int i, int z)
    {{
    }}

    public void Baz()
    {{
        {expression};
    }}

    public long this[int a, int s] => a + s;
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(GetTestExpressions), "\r\n", "\r\n            ")]
        public async Task TestInvalidExpressionAsync(string expression, string fixedExpression, int column)
        {
            var testCode = $@"
class Foo
{{
    public void Bar(int i, int z)
    {{
    }}

    public void Baz()
    {{
        {expression};
    }}

    public long this[int a, int s] => a + s;
}}";
            var fixedCode = $@"
class Foo
{{
    public void Bar(int i, int z)
    {{
    }}

    public void Baz()
    {{
        {fixedExpression};
    }}

    public long this[int a, int s] => a + s;
}}";

            DiagnosticResult expected = Diagnostic().WithLocation(10, column);
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestImplicitElementAccessAsync()
        {
            var testCode = @"
class Foo
{
    private System.Collections.Generic.Dictionary<int, string> dict = new System.Collections.Generic.Dictionary<int, string>
    {
        [1] = ""foo""
    };
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestValidAttributeAsync()
        {
            var testCode = @"
[System.AttributeUsage(System.AttributeTargets.Class)]
public class MyAttribute : System.Attribute
{
    public MyAttribute(int a, int b)
    {
    }
}

[MyAttribute(1, 2)]
class Foo
{
}

// This is a regression test for https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/1211
[System.Obsolete]
class ObsoleteType
{
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInvalidAttributeAsync()
        {
            var testCode = @"
[System.AttributeUsage(System.AttributeTargets.Class)]
public class MyAttribute : System.Attribute
{
    public MyAttribute(int a, int b)
    {
    }
}

[MyAttribute(1,
      2)]
class Foo
{
}";
            var fixedCode = @"
[System.AttributeUsage(System.AttributeTargets.Class)]
public class MyAttribute : System.Attribute
{
    public MyAttribute(int a, int b)
    {
    }
}

[MyAttribute(
    1,
      2)]
class Foo
{
}";

            DiagnosticResult expected = Diagnostic().WithLocation(10, 14);
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
