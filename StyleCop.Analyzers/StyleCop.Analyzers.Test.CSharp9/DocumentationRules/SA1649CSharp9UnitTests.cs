// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using StyleCop.Analyzers.Test.CSharp6.Helpers;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1649FileNameMustMatchTypeName,
        StyleCop.Analyzers.DocumentationRules.SA1649CodeFixProvider>;

    public partial class SA1649CSharp9UnitTests
    {
        /// <summary>
        /// Verifies that a file with top-level statements is ignored, even when it also declares types.
        /// </summary>
        /// <param name="typeKeyword">The type keyword to use during the test.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [MemberData(nameof(CommonMemberData.TypeDeclarationKeywords), MemberType = typeof(CommonMemberData))]
        public async Task VerifyFileWithTopLevelStatementsIsIgnoredAsync(string typeKeyword)
        {
            var testCode = $@"System.Console.WriteLine();

{GetTypeDeclaration(typeKeyword, "TestType")}
";

            await new CSharpTest()
            {
                TestState =
                {
                    OutputKind = OutputKind.ConsoleApplication,
                },
                TestSources = { ("Program.cs", testCode) },
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }
    }
}
