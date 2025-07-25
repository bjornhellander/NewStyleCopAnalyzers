﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.MaintainabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1404CodeAnalysisSuppressionMustHaveJustification,
        StyleCop.Analyzers.MaintainabilityRules.SA1404CodeFixProvider>;

    public class SA1404UnitTests
    {
        [Theory]
        [InlineData("SuppressMessage")]
        [InlineData("SuppressMessageAttribute")]
        public async Task TestSuppressionWithStringLiteralAsync(string attributeName)
        {
            var testCode = $@"public class Foo
{{
    [System.Diagnostics.CodeAnalysis.{attributeName}(null, null, Justification = ""a justification"")]
    public void Bar()
    {{

    }}
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestSuppressionWithStringLiteralAndUsingAliasDirectiveAsync()
        {
            var testCode = @"using SuppressMessageAttribute = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute;
public class Foo
{
    [SuppressMessage(null, null, Justification = ""a justification"")]
    public void Bar()
    {

    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestSuppressionWithPlaceholderValueAsync()
        {
            var testCode = @"using SuppressMessageAttribute = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute;
public class Foo
{
    [SuppressMessage(null, null, Justification = """ + SA1404CodeAnalysisSuppressionMustHaveJustification.JustificationPlaceholder + @""")]
    public void Bar()
    {

    }
}";

            DiagnosticResult expected = Diagnostic().WithLocation(4, 34);
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestSuppressionWithNoJustificationAsync()
        {
            var testCode = @"public class Foo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null)]
    public void Bar()
    {

    }
}";

            var fixedCode = @"public class Foo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, Justification = """ + SA1404CodeAnalysisSuppressionMustHaveJustification.JustificationPlaceholder + @""")]
    public void Bar()
    {

    }
}";

            await new CSharpTest
            {
                TestCode = testCode,
                ExpectedDiagnostics =
                {
                    Diagnostic().WithLocation(3, 6),
                },
                FixedCode = fixedCode,
                RemainingDiagnostics =
                {
                    Diagnostic().WithLocation(3, 66),
                },
                NumberOfIncrementalIterations = 2,
                NumberOfFixAllIterations = 2,
            }.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestSuppressionWithUsingAliasDirectiveAndNoJustificationAsync()
        {
            var testCode = @"using SuppressMessageAttribute = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute;
public class Foo
{
    [SuppressMessage(null, null)]
    public void Bar()
    {

    }
}";

            var fixedCode = @"using SuppressMessageAttribute = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute;
public class Foo
{
    [SuppressMessage(null, null, Justification = """ + SA1404CodeAnalysisSuppressionMustHaveJustification.JustificationPlaceholder + @""")]
    public void Bar()
    {

    }
}";

            await new CSharpTest
            {
                TestCode = testCode,
                ExpectedDiagnostics =
                {
                    Diagnostic().WithLocation(4, 6),
                },
                FixedCode = fixedCode,
                RemainingDiagnostics =
                {
                    Diagnostic().WithLocation(4, 34),
                },
                NumberOfIncrementalIterations = 2,
                NumberOfFixAllIterations = 2,
            }.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestSuppressionWithUsingDifferentAliasDirectiveAndNoJustificationAsync()
        {
            var testCode = @"using MySuppressionAttribute = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute;
public class Foo
{
    [MySuppression(null, null)]
    public void Bar()
    {

    }
}";

            var fixedCode = @"using MySuppressionAttribute = System.Diagnostics.CodeAnalysis.SuppressMessageAttribute;
public class Foo
{
    [MySuppression(null, null, Justification = """ + SA1404CodeAnalysisSuppressionMustHaveJustification.JustificationPlaceholder + @""")]
    public void Bar()
    {

    }
}";

            await new CSharpTest
            {
                TestCode = testCode,
                ExpectedDiagnostics =
                {
                    Diagnostic().WithLocation(4, 6),
                },
                FixedCode = fixedCode,
                RemainingDiagnostics =
                {
                    Diagnostic().WithLocation(4, 32),
                },
                NumberOfIncrementalIterations = 2,
                NumberOfFixAllIterations = 2,
            }.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestSuppressionWithEmptyJustificationAsync()
        {
            var testCode = @"public class Foo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, Justification = """")]
    public void Bar()
    {

    }
}";

            var fixedCode = @"public class Foo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, Justification = """ + SA1404CodeAnalysisSuppressionMustHaveJustification.JustificationPlaceholder + @""")]
    public void Bar()
    {

    }
}";

            await new CSharpTest
            {
                TestCode = testCode,
                ExpectedDiagnostics =
                {
                    Diagnostic().WithLocation(3, 66),
                },
                FixedCode = fixedCode,
                RemainingDiagnostics =
                {
                    Diagnostic().WithLocation(3, 66),
                },
                NumberOfIncrementalIterations = 2,
                NumberOfFixAllIterations = 2,
            }.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestSuppressionWithEscapedIdentifierWithJustificationAsync()
        {
            var testCode = @"public class Foo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, Justific\u0061tion = """")]
    public void Bar()
    {

    }
}";

            var fixedCode = @"public class Foo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, Justific\u0061tion = """ + SA1404CodeAnalysisSuppressionMustHaveJustification.JustificationPlaceholder + @""")]
    public void Bar()
    {

    }
}";

            await new CSharpTest
            {
                TestCode = testCode,
                ExpectedDiagnostics =
                {
                    Diagnostic().WithLocation(3, 66),
                },
                FixedCode = fixedCode,
                RemainingDiagnostics =
                {
                    Diagnostic().WithLocation(3, 66),
                },
                NumberOfIncrementalIterations = 2,
                NumberOfFixAllIterations = 2,
            }.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestSuppressionWithWhitespaceJustificationAsync()
        {
            var testCode = @"public class Foo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, Justification = ""    "")]
    public void Bar()
    {

    }
}";

            var fixedCode = @"public class Foo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, Justification = """ + SA1404CodeAnalysisSuppressionMustHaveJustification.JustificationPlaceholder + @""")]
    public void Bar()
    {

    }
}";

            await new CSharpTest
            {
                TestCode = testCode,
                ExpectedDiagnostics =
                {
                    Diagnostic().WithLocation(3, 66),
                },
                FixedCode = fixedCode,
                RemainingDiagnostics =
                {
                    Diagnostic().WithLocation(3, 66),
                },
                NumberOfIncrementalIterations = 2,
                NumberOfFixAllIterations = 2,
            }.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestSuppressionWithNullJustificationAsync()
        {
            var testCode = @"public class Foo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, Justification = null)]
    public void Bar()
    {

    }
}";

            var fixedCode = @"public class Foo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, Justification = """ + SA1404CodeAnalysisSuppressionMustHaveJustification.JustificationPlaceholder + @""")]
    public void Bar()
    {

    }
}";

            await new CSharpTest
            {
                TestCode = testCode,
                ExpectedDiagnostics =
                {
                    Diagnostic().WithLocation(3, 66),
                },
                FixedCode = fixedCode,
                RemainingDiagnostics =
                {
                    Diagnostic().WithLocation(3, 66),
                },
                NumberOfIncrementalIterations = 2,
                NumberOfFixAllIterations = 2,
            }.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestSuppressionWithComplexJustificationAsync()
        {
            var testCode = @"public class Foo
{
    const string JUSTIFICATION = ""Foo"";
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, Justification = """" + JUSTIFICATION)]
    public void Bar()
    {

    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestSuppressionWithComplexWhitespaceJustificationAsync()
        {
            var testCode = @"public class Foo
{
    const string JUSTIFICATION = ""    "";
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, Justification = """" + JUSTIFICATION)]
    public void Bar()
    {

    }
}";

            var fixedCode = @"public class Foo
{
    const string JUSTIFICATION = ""    "";
    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, Justification = """ + SA1404CodeAnalysisSuppressionMustHaveJustification.JustificationPlaceholder + @""")]
    public void Bar()
    {

    }
}";

            await new CSharpTest
            {
                TestCode = testCode,
                ExpectedDiagnostics =
                {
                    Diagnostic().WithLocation(4, 66),
                },
                FixedCode = fixedCode,
                RemainingDiagnostics =
                {
                    Diagnostic().WithLocation(4, 66),
                },
                NumberOfIncrementalIterations = 2,
                NumberOfFixAllIterations = 2,
            }.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestDiagnosticDoesNotThrowNullReferenceForWrongConstantTypeAsync()
        {
            var testCode = @"public class Foo
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage(null, null, Justification = 5)]
    public void Bar()
    {

    }
}";

            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(4, 66),
                DiagnosticResult.CompilerError("CS0029").WithMessage("Cannot implicitly convert type 'int' to 'string'").WithLocation(4, 82),
            };
            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestOtherAttributeAsync()
        {
            var testCode = @"public class Foo
{
    [System.Obsolete(""Method is obsolete!"")]
    public void Bar()
    {
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("global::System.Obsolete")]
        [InlineData("global::My")]
        [WorkItem(3829, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3829")]
        public async Task TestGlobalOtherAttributeAsync(string name)
        {
            var testCode = $@"public class MyAttribute : System.Attribute
{{
}}

[{name}]
public class Foo
{{
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
