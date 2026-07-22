// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.DocumentationRules
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.DocumentationRules;
    using StyleCop.Analyzers.Lightup;
    using StyleCop.Analyzers.Test.CSharp6.Verifiers;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.CustomDiagnosticVerifier<StyleCop.Analyzers.DocumentationRules.SA1619GenericTypeParametersMustBeDocumentedPartialClass>;

    /// <summary>
    /// This class contains unit tests for <see cref="SA1619GenericTypeParametersMustBeDocumentedPartialClass"/>.
    /// </summary>
    public class SA1619UnitTests
    {
        public static TheoryData<string> Types
        {
            get
            {
                var data = new TheoryData<string>
                {
                    "class Foo<{|#0:Ta|}, {|#1:Tb|}> { }",
                    "struct Foo<{|#0:Ta|}, {|#1:Tb|}> { }",
                    "interface Foo<{|#0:Ta|}, {|#1:Tb|}> { }",
                    "class Foo<{|#0:Ta|}, {|#1:T\\u0062|}> { }",
                    "struct Foo<{|#0:Ta|}, {|#1:T\\u0062|}> { }",
                    "interface Foo<{|#0:Ta|}, {|#1:T\\u0062|}> { }",
                };

                if (LightupHelpers.SupportsCSharp9)
                {
                    data.Add("record Foo<{|#0:Ta|}, {|#1:Tb|}> { }");
                    data.Add("record Foo<{|#0:Ta|}, {|#1:T\\u0062|}> { }");
                }

                if (LightupHelpers.SupportsCSharp10)
                {
                    data.Add("record class Foo<{|#0:Ta|}, {|#1:Tb|}> { }");
                    data.Add("record struct Foo<{|#0:Ta|}, {|#1:T\\u0062|}> { }");
                }

                if (LightupHelpers.SupportsCSharp15)
                {
                    data.Add("union Foo<{|#0:Ta|}, {|#1:Tb|}>(string, int) { }");
                }

                return data;
            }
        }

        [Fact]
        public async Task TestTypesWithoutTypeParametersAsync()
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
public partial class Foo { }";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(Types))]
        public async Task TestPartialTypesWithAllDocumentationAsync(string p)
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
/// <typeparam name=""Ta"">Param 1</param>
/// <typeparam name=""Tb"">Param 2</param>
public partial ##";
            await VerifyCSharpDiagnosticAsync(testCode.Replace("##", p), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(Types))]
        public async Task TestPartialTypesWithAllDocumentationAlternativeSyntaxAsync(string p)
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
/// <typeparam name=""T&#97;"">Param 1</param>
/// <typeparam name=""T&#x62;"">Param 2</param>
public partial ##";
            await VerifyCSharpDiagnosticAsync(testCode.Replace("##", p), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(Types))]
        public async Task TestPartialTypesWithAllDocumentationWrongOrderAsync(string p)
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
/// <typeparam name=""Tb"">Param 2</param>
/// <typeparam name=""Ta"">Param 1</param>
public partial ##";
            await VerifyCSharpDiagnosticAsync(testCode.Replace("##", p), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(Types))]
        public async Task TestPartialTypesWithNoDocumentationAsync(string p)
        {
            var testCode = @"
public partial ##";
            await VerifyCSharpDiagnosticAsync(testCode.Replace("##", p), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(Types))]
        public async Task TestPartialTypesInheritDocAsync(string p)
        {
            var testCode = @"
/// <inheritdoc/>
public partial ##";
            await VerifyCSharpDiagnosticAsync(testCode.Replace("##", p), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(Types))]
        public async Task TestPartialTypesWithMissingDocumentationAsync(string p)
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
public partial ##";

            var expected = new[]
            {
                Diagnostic().WithLocation(0).WithArguments("Ta"),
                Diagnostic().WithLocation(1).WithArguments("Tb"),
            };

            await VerifyCSharpDiagnosticAsync(testCode.Replace("##", p), expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(Types))]
        [WorkItem(2453, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2453")]
        public async Task TestPartialTypesWithMissingButNotRequiredDocumentationAsync(string p)
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
public partial ##";

            // This situation is allowed if 'documentExposedElements' and 'documentInterfaces' is false
            string interfaceSettingName = p.StartsWith("interface ") ? "documentInterfaces" : "ignoredProperty";
            var testSettings = $@"
{{
  ""settings"": {{
    ""documentationRules"": {{
      ""documentExposedElements"": false,
      ""{interfaceSettingName}"": false
    }}
  }}
}}
";
            await VerifyCSharpDiagnosticAsync(testCode.Replace("##", p), testSettings, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(Types))]
        public async Task TestNonPartialTypesWithMissingDocumentationAsync(string p)
        {
            var testCode = @"
/// <summary>
/// Foo
/// </summary>
public ##";

            await VerifyCSharpDiagnosticAsync(testCode.Replace("##", p), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(Types))]
        public async Task TestPartialTypesWithContentDocumentationAsync(string p)
        {
            var testCode = @"
/// <content>
/// Foo
/// </content>
public partial ##";

            await VerifyCSharpDiagnosticAsync(testCode.Replace("##", p), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a generic partial type with included documentation will work.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestGenericPartialTypeWithIncludedDocumentationAsync()
        {
            var testCode = @"
/// <include file='ClassWithTypeparamDoc.xml' path='/TestClass/*'/>
public partial class TestClass<T>
{
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a generic partial type without a summary tag in the included documentation will not produce diagnostics.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestGenericPartialTypeWithoutSummaryInIncludedDocumentationAsync()
        {
            var testCode = @"
/// <include file='ClassWithoutSummary.xml' path='/TestClass/*'/>
public partial class TestClass<T>
{
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a generic partial type without a typeparam in included documentation will flag.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestGenericPartialTypeWithoutTypeparamInIncludedDocumentationAsync()
        {
            var testCode = @"
/// <include file='ClassWithoutTypeparamDoc.xml' path='/TestClass/*'/>
public partial class TestClass<T>
{
}
";

            var expected = Diagnostic().WithLocation(3, 32).WithArguments("T");
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        [WorkItem(2453, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2453")]
        public async Task TestGenericPartialTypeWithoutTypeparamInIncludedButNotRequiredDocumentationAsync()
        {
            var testCode = @"
/// <include file='ClassWithoutTypeparamDoc.xml' path='/TestClass/*'/>
public partial class TestClass<T>
{
}
";

            // The situation is allowed if 'documentExposedElements' false
            var testSettings = @"
{
  ""settings"": {
    ""documentationRules"": {
      ""documentExposedElements"": false
    }
  }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, testSettings, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a generic partial type with &lt;inheritdoc&gt; in included documentation will work.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestGenericPartialTypeWithInheritdocInIncludedDocumentationAsync()
        {
            var testCode = @"
/// <include file='ClassWithIneheritdoc.xml' path='/TestClass/*'/>
public partial class TestClass<T>
{
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        protected static Task VerifyCSharpDiagnosticAsync(string source, DiagnosticResult expected, CancellationToken cancellationToken)
            => VerifyCSharpDiagnosticAsync(source, testSettings: null, new[] { expected }, cancellationToken);

        protected static Task VerifyCSharpDiagnosticAsync(string source, DiagnosticResult[] expected, CancellationToken cancellationToken)
            => VerifyCSharpDiagnosticAsync(source, testSettings: null, expected, cancellationToken);

        protected static Task VerifyCSharpDiagnosticAsync(string source, string? testSettings, DiagnosticResult[] expected, CancellationToken cancellationToken)
        {
            string contentClassWithTypeparamDoc = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<TestClass>
  <summary>Test class</summary>
  <typeparam name=""T"">Param 1</typeparam>
</TestClass>
";
            string contentClassWithoutSummary = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<TestClass>
</TestClass>
";
            string contentClassWithoutTypeparamDoc = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<TestClass>
  <summary>Test class</summary>
</TestClass>
";
            string contentClassInheritdoc = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<TestClass>
  <inheritdoc/>
</TestClass>
";

            var test = new StyleCopDiagnosticVerifier<SA1619GenericTypeParametersMustBeDocumentedPartialClass>.CSharpTest
            {
                TestCode = source,
                Settings = testSettings,
                XmlReferences =
                {
                    { "ClassWithTypeparamDoc.xml", contentClassWithTypeparamDoc },
                    { "ClassWithoutSummary.xml", contentClassWithoutSummary },
                    { "ClassWithoutTypeparamDoc.xml", contentClassWithoutTypeparamDoc },
                    { "ClassWithIneheritdoc.xml", contentClassInheritdoc },
                },
            };

            test.ExpectedDiagnostics.AddRange(expected);
            return test.RunAsync(cancellationToken);
        }
    }
}
