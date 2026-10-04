// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using StyleCop.Analyzers.Test.CSharp6.Helpers;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1402FileMayOnlyContainASingleType,
        StyleCop.Analyzers.MaintainabilityRules.SA1402CodeFixProvider>;

    /// <summary>
    /// Tests for SA1402 in files with top-level statements, where the implicit <c>Program</c> class is counted as a
    /// class declared at the position of the top-level statements. No code fix is offered in such files, to keep the code fix provider simple.
    /// </summary>
    public partial class SA1402CSharp9UnitTests
    {
        [Theory]
        [InlineData("Program.cs")]
        [InlineData("Xyz.cs")]
        public async Task TestTopLevelStatementsOnlyAsync(string fileName)
        {
            var testCode = @"System.Console.WriteLine();
";

            await VerifyTopLevelProgramAsync(fileName, testCode, settings: null).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.ReferenceTypeDeclarationKeywords), MemberType = typeof(CommonMemberData))]
        public async Task TestTopLevelStatementsAndTypeAsync(string typeKeyword)
        {
            var testCode = $@"System.Console.WriteLine();

{typeKeyword} [|TestType|]
{{
}}
";

            await VerifyTopLevelProgramAsync("Program.cs", testCode, settings: null).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestTopLevelStatementsAndTypeMatchingFileNameAsync()
        {
            var testCode = @"using System;

[||]Console.WriteLine();

class TestType
{
}
";

            await VerifyTopLevelProgramAsync("TestType.cs", testCode, settings: null).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestTopLevelStatementsAndTypeMatchingFileNameAndOtherTypeAsync()
        {
            var testCode = @"[||]System.Console.WriteLine();

class TestType
{
}

class [|OtherType|]
{
}
";

            await VerifyTopLevelProgramAsync("TestType.cs", testCode, settings: null).ConfigureAwait(true);
        }

        [Theory]
        [InlineData("Program.cs")]
        [InlineData("Xyz.cs")]
        public async Task TestTopLevelStatementsAndTypeInNamespaceAsync(string fileName)
        {
            var testCode = @"System.Console.WriteLine();

namespace TestNamespace
{
    class [|TestType|]
    {
    }
}
";

            await VerifyTopLevelProgramAsync(fileName, testCode, settings: null).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that a declared <c>partial class Program</c> is treated as part of the implicit <c>Program</c>
        /// class, so it is not reported when the implicit <c>Program</c> class is the type kept by the rule.
        /// </summary>
        /// <remarks>
        /// <para>The rule is purely syntactic. With compilers before Roslyn 4.0 (e.g. the Roslyn 3.8 used by the CSharp9
        /// tests), the implicit class is named <c>&lt;Program&gt;$</c>, so <c>partial class Program</c> is actually a
        /// separate class there and strictly should be reported as well. This is accepted, since it only leads to a
        /// missing diagnostic in an unusual file.</para>
        /// </remarks>
        /// <param name="fileName">The name of the file containing the test code.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [InlineData("Program.cs")]
        [InlineData("Xyz.cs")]
        public async Task TestTopLevelStatementsAndPartialProgramAsync(string fileName)
        {
            var testCode = @"System.Console.WriteLine();

partial class Program
{
}

class [|TestType|]
{
}
";

            await VerifyTopLevelProgramAsync(fileName, testCode, settings: null).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that when the implicit <c>Program</c> class is not the type kept by the rule, every part of it is
        /// reported, both the top-level statements and a declared <c>partial class Program</c>. This matches how all
        /// parts of other partial types are reported.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestTopLevelStatementsPartialProgramAndTypeMatchingFileNameAsync()
        {
            var testCode = @"[||]System.Console.WriteLine();

partial class [|Program|]
{
}

class TestType
{
}
";

            await VerifyTopLevelProgramAsync("TestType.cs", testCode, settings: null).ConfigureAwait(true);
        }

        [Theory]
        [InlineData("Program.cs")]
        [InlineData("Xyz.cs")]
        public async Task TestTopLevelStatementsAndInterfaceWhenClassIsConfiguredAsync(string fileName)
        {
            var testCode = @"System.Console.WriteLine();

interface [|ITestType|]
{
}
";

            var settings = @"
{
  ""settings"": {
    ""maintainabilityRules"": {
      ""topLevelTypes"": [""class"", ""interface""]
    }
  }
}";

            await VerifyTopLevelProgramAsync(fileName, testCode, settings).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestTopLevelStatementsAndTypesWhenClassIsNotConfiguredAsync()
        {
            var testCode = @"System.Console.WriteLine();

class TestType
{
}

interface ITestType
{
}
";

            var settings = @"
{
  ""settings"": {
    ""maintainabilityRules"": {
      ""topLevelTypes"": [""interface""]
    }
  }
}";

            await VerifyTopLevelProgramAsync("Program.cs", testCode, settings).ConfigureAwait(true);
        }

        private static Task VerifyTopLevelProgramAsync(string fileName, string source, string? settings)
        {
            // The fixed source is the same as the test source, which verifies that no code fix is offered.
            return new CSharpTest()
            {
                TestState =
                {
                    OutputKind = OutputKind.ConsoleApplication,
                },
                TestSources = { (fileName, source) },
                FixedSources = { (fileName, source) },
                Settings = settings,
            }.RunAsync(CancellationToken.None);
        }
    }
}
