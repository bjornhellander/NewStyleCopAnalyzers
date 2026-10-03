// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1520UseBracesConsistently,
        StyleCop.Analyzers.LayoutRules.SA1503CodeFixProvider>;

    public partial class SA1520CSharp9UnitTests
    {
        [Fact]
        public async Task TestIfElseStatementWithInconsistentBracesInTopLevelProgramAsync()
        {
            var testCode = @"if (args.Length > 0)
{
    System.Console.WriteLine(1);
}
else
    [|System.Console.WriteLine(2);|]
";

            var fixedCode = @"if (args.Length > 0)
{
    System.Console.WriteLine(1);
}
else
{
    System.Console.WriteLine(2);
}
";

            await new CSharpTest()
            {
                TestState =
                {
                    OutputKind = OutputKind.ConsoleApplication,
                    Sources = { testCode },
                },
                FixedCode = fixedCode,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }
    }
}
