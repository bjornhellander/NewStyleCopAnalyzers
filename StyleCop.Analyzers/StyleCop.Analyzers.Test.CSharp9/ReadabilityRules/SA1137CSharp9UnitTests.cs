// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1137ElementsShouldHaveTheSameIndentation,
        StyleCop.Analyzers.ReadabilityRules.IndentationCodeFixProvider>;

    public partial class SA1137CSharp9UnitTests
    {
        [Fact]
        public async Task TestInitAccessorAttributeListAsync()
        {
            string testCode = @"
using System;

class TestClass
{
    int Property
    {
        [My]
[| |][My]
        init { }
    }
}

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
class MyAttribute : Attribute { }
";

            string fixedCode = @"
using System;

class TestClass
{
    int Property
    {
        [My]
        [My]
        init { }
    }
}

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
class MyAttribute : Attribute { }
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestGlobalStatementIndentationInTopLevelProgramAsync()
        {
            var testCode = @"System.Console.WriteLine(1);
{|#0:  |}System.Console.WriteLine(2);
System.Console.WriteLine(3);
";

            var fixedCode = @"System.Console.WriteLine(1);
System.Console.WriteLine(2);
System.Console.WriteLine(3);
";

            var test = new CSharpTest()
            {
                TestState =
                {
                    OutputKind = OutputKind.ConsoleApplication,
                    Sources = { testCode },
                },
                FixedCode = fixedCode,
            };
            test.TestState.ExpectedDiagnostics.AddRange(this.GetExpectedResultTestGlobalStatementIndentationInTopLevelProgram());
            await test.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestMemberIndentationInPositionalRecordAsync()
        {
            var testCode = @"public record TestRecord(int X)
{
    public int A { get; }

{|#0:  |}public int B { get; }
}
";

            var fixedCode = @"public record TestRecord(int X)
{
    public int A { get; }

    public int B { get; }
}
";

            var expected = this.GetExpectedResultTestMemberIndentationInPositionalRecord();
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestGlobalStatementIndentationInTopLevelProgram()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/58561
                Diagnostic().WithLocation(0),
                Diagnostic().WithLocation(0),
            };
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestMemberIndentationInPositionalRecord()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/53136
                Diagnostic().WithLocation(0),
                Diagnostic().WithLocation(0),
            };
        }
    }
}
