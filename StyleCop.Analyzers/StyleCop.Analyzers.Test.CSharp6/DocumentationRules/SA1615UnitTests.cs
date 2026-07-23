// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.DocumentationRules;
    using StyleCop.Analyzers.Test.CSharp6.Verifiers;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.CustomDiagnosticVerifier<StyleCop.Analyzers.DocumentationRules.SA1615ElementReturnValueMustBeDocumented>;

    /// <summary>
    /// This class contains unit tests for <see cref="SA1615ElementReturnValueMustBeDocumented"/>.
    /// </summary>
    public class SA1615UnitTests
    {
        public static TheoryData<string> WithReturnValue { get; } = new TheoryData<string>()
        {
            "    public          ClassName Method(string foo, string bar) { return null; }",
            "    public delegate ClassName Method(string foo, string bar);",
        };

        public static TheoryData<string> AsynchronousWithReturnValue { get; } = new TheoryData<string>()
        {
            "    public          Task      MethodAsync(string foo, string bar) { return null; }",
            "    public          Task<int> MethodAsync(string foo, string bar) { return null; }",
            "    public          TASK      MethodAsync(string foo, string bar) { return null; }",
            "    public delegate Task      MethodAsync(string foo, string bar);",
            "    public delegate Task<int> MethodAsync(string foo, string bar);",
            "    public delegate TASK      MethodAsync(string foo, string bar);",
        };

        public static TheoryData<string, string> AsynchronousUnitTestWithReturnValue { get; } = new TheoryData<string, string>()
        {
            { "    public          Task      MethodAsync(string foo, string bar) { return null; }", "TestMethod" },
            { "    public          Task      MethodAsync(string foo, string bar) { return null; }", "Fact" },
            { "    public          Task      MethodAsync(string foo, string bar) { return null; }", "Theory" },
            { "    public          Task      MethodAsync(string foo, string bar) { return null; }", "Test" },
            { "    public          Task<int> MethodAsync(string foo, string bar) { return null; }", "TestMethod" },
            { "    public          Task<int> MethodAsync(string foo, string bar) { return null; }", "Fact" },
            { "    public          Task<int> MethodAsync(string foo, string bar) { return null; }", "Theory" },
            { "    public          Task<int> MethodAsync(string foo, string bar) { return null; }", "Test" },
            { "    public          TASK      MethodAsync(string foo, string bar) { return null; }", "TestMethod" },
            { "    public          TASK      MethodAsync(string foo, string bar) { return null; }", "Fact" },
            { "    public          TASK      MethodAsync(string foo, string bar) { return null; }", "Theory" },
            { "    public          TASK      MethodAsync(string foo, string bar) { return null; }", "Test" },
        };

        public static TheoryData<string> WithoutReturnValue { get; } = new TheoryData<string>()
        {
            "    public void Method(string foo, string bar) { }",
            "    public delegate void Method(string foo, string bar);",
        };

        [Theory]
        [MemberData(nameof(WithReturnValue))]
        [MemberData(nameof(WithoutReturnValue))]
        public async Task TestMethodWithoutDocumentationAsync(string declaration)
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
public class ClassName
{
$$
}";
            await VerifyCSharpDiagnosticAsync(testCode.Replace("$$", declaration), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(WithoutReturnValue))]
        public async Task TestMethodWithoutReturnTypeWithoutReturnTypeDocumentationAsync(string declaration)
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
public class ClassName
{
    /// <summary>
    /// Foo
    /// </summary>
$$
}";
            await VerifyCSharpDiagnosticAsync(testCode.Replace("$$", declaration), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(WithoutReturnValue))]
        public async Task TestMethodWithVoidWithDocumentationAsync(string declaration)
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
public class ClassName
{
    /// <summary>
    /// Foo
    /// </summary>
    /// <returns>Foo</returns>
$$
}";
            await VerifyCSharpDiagnosticAsync(testCode.Replace("$$", declaration), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(WithReturnValue))]
        public async Task TestMethodWithReturnTypeWithoutReturnTypeDocumentationAsync(string declaration)
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
public class ClassName
{
    /// <summary>
    /// Foo
    /// </summary>
$$
}";
            var fixedCode = @"
/// <summary>
/// Foo
/// </summary>
public class ClassName
{
    /// <summary>
    /// Foo
    /// </summary>
    /// <returns></returns>
$$
}";

            var expected = Diagnostic().WithLocation(10, 21);
            await VerifyCSharpFixAsync(testCode.Replace("$$", declaration), expected, fixedCode.Replace("$$", declaration), CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(AsynchronousWithReturnValue))]
        public async Task TestAsynchronousMethodWithReturnTypeWithoutReturnTypeDocumentationAsync(string declaration)
        {
            var testCode = @"
using System.Threading.Tasks;
using TASK = System.Threading.Tasks.Task<int>;
/// <summary>
/// Foo
/// </summary>
public class ClassName
{
    /// <summary>
    /// Foo
    /// </summary>
$$
}";
            var fixedCode = @"
using System.Threading.Tasks;
using TASK = System.Threading.Tasks.Task<int>;
/// <summary>
/// Foo
/// </summary>
public class ClassName
{
    /// <summary>
    /// Foo
    /// </summary>
    /// <returns><placeholder>A <see cref=""Task""/> representing the asynchronous operation.</placeholder></returns>
$$
}";

            var expected = Diagnostic().WithLocation(12, 21);
            await VerifyCSharpFixAsync(testCode.Replace("$$", declaration), expected, fixedCode.Replace("$$", declaration), CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(AsynchronousUnitTestWithReturnValue))]
        public async Task TestAsynchronousUnitTestMethodWithReturnTypeWithoutReturnTypeDocumentationAsync(string declaration, string testAttribute)
        {
            var testCode = @"
using System.Threading.Tasks;
using TASK = System.Threading.Tasks.Task<int>;
/// <summary>
/// Foo
/// </summary>
public class ClassName
{
    /// <summary>
    /// Foo
    /// </summary>
    [##]
$$
}
internal sealed class ##Attribute : System.Attribute { }
";
            var fixedCode = @"
using System.Threading.Tasks;
using TASK = System.Threading.Tasks.Task<int>;
/// <summary>
/// Foo
/// </summary>
public class ClassName
{
    /// <summary>
    /// Foo
    /// </summary>
    /// <returns><placeholder>A <see cref=""Task""/> representing the asynchronous unit test.</placeholder></returns>
    [##]
$$
}
internal sealed class ##Attribute : System.Attribute { }
";

            var expected = Diagnostic().WithLocation(13, 21);
            await VerifyCSharpFixAsync(testCode.Replace("$$", declaration).Replace("##", testAttribute), expected, fixedCode.Replace("$$", declaration).Replace("##", testAttribute), CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(WithReturnValue))]
        public async Task TestMethodWithReturnTypeWithDocumentationAsync(string declaration)
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
public class ClassName
{
    /// <summary>
    /// Foo
    /// </summary>
    /// <returns>Foo</returns>
$$
}";
            await VerifyCSharpDiagnosticAsync(testCode.Replace("$$", declaration), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(WithReturnValue))]
        [MemberData(nameof(WithoutReturnValue))]
        public async Task TestMethodWithInheritedDocumentationAsync(string declaration)
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
public class ClassName
{
    /// <inheritdoc/>
    public ClassName Test() { return null; }
}";
            await VerifyCSharpDiagnosticAsync(testCode.Replace("$$", declaration), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestIncludedDocumentationAsync()
        {
            var testCode = @"
class Class1
{
    /// <include file='MethodWithoutReturns.xml' path='/Class1/MethodName/*'/>
    public int MethodName()
    {
        return 0;
    }
}
";

            DiagnosticResult expected = Diagnostic().WithLocation(5, 12);

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestIncludedInheritedDocumentationAsync()
        {
            var testCode = @"
class Class1
{
    /// <include file='MethodWithInheritedReturns.xml' path='/Class1/MethodName/*'/>
    public int MethodName()
    {
        return 0;
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestIncludedIncompleteDocumentationAsync()
        {
            var testCode = @"
class Class1
{
    /// <include file='MethodWithReturns.xml' path='/Class1/MethodName/*'/>
    public int MethodName()
    {
        return 0;
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        [WorkItem(2445, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2445")]
        public async Task TestPrivateMethodMissingReturnsAsync()
        {
            var testCode = @"
internal class ClassName
{
    ///
    private int Test1(int arg) { throw new System.NotImplementedException(); }

    /**
     *
     */
    private int Test2(int arg) { throw new System.NotImplementedException(); }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        private static Task VerifyCSharpDiagnosticAsync(string source, DiagnosticResult expected, CancellationToken cancellationToken)
            => VerifyCSharpFixAsync(source, new[] { expected }, fixedSource: null, cancellationToken);

        private static Task VerifyCSharpDiagnosticAsync(string source, DiagnosticResult[] expected, CancellationToken cancellationToken)
            => VerifyCSharpFixAsync(source, expected, fixedSource: null, cancellationToken);

        private static Task VerifyCSharpFixAsync(string source, DiagnosticResult expected, string fixedSource, CancellationToken cancellationToken)
            => VerifyCSharpFixAsync(source, new[] { expected }, fixedSource, cancellationToken);

        private static Task VerifyCSharpFixAsync(string source, DiagnosticResult[] expected, string? fixedSource, CancellationToken cancellationToken)
        {
            string contentWithReturns = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Class1>
  <MethodName>
    <summary>
      Sample method.
    </summary>
    <returns>
      A <see cref=""Task""/> representing the asynchronous operation.
    </returns>
  </MethodName>
</Class1>
";
            string contentWithInheritedReturns = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Class1>
  <MethodName>
    <inheritdoc/>
  </MethodName>
</Class1>
";
            string contentWithoutReturns = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<Class1>
  <MethodName>
    <summary>
      Sample method.
    </summary>
  </MethodName>
</Class1>
";

            var test = new StyleCopCodeFixVerifier<SA1615ElementReturnValueMustBeDocumented, SA1615SA1616CodeFixProvider>.CSharpTest
            {
                TestCode = source,
                FixedCode = fixedSource!,
                XmlReferences =
                {
                    { "MethodWithReturns.xml", contentWithReturns },
                    { "MethodWithInheritedReturns.xml", contentWithInheritedReturns },
                    { "MethodWithoutReturns.xml", contentWithoutReturns },
                },
            };

            test.ExpectedDiagnostics.AddRange(expected);
            return test.RunAsync(cancellationToken);
        }
    }
}
