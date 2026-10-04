// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1312VariableNamesMustBeginWithLowerCaseLetter,
        StyleCop.Analyzers.NamingRules.RenameToLowerCaseCodeFixProvider>;

    public partial class SA1312CSharp9UnitTests
    {
        [Fact]
        public async Task TestLocalVariableInTopLevelProgramAsync()
        {
            var testCode = @"int {|#0:Value|} = 0;
System.Console.WriteLine(Value);
";

            var fixedCode = @"int value = 0;
System.Console.WriteLine(value);
";

            await new CSharpTest()
            {
                TestState =
                {
                    OutputKind = OutputKind.ConsoleApplication,
                    Sources = { testCode },
                },
                ExpectedDiagnostics = { Diagnostic().WithLocation(0).WithArguments("Value") },
                FixedCode = fixedCode,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }
    }
}
