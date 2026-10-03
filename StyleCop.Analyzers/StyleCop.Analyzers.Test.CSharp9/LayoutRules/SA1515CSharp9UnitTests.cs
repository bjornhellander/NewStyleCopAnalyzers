// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1515SingleLineCommentMustBePrecededByBlankLine,
        StyleCop.Analyzers.LayoutRules.SA1515CodeFixProvider>;

    public partial class SA1515CSharp9UnitTests
    {
        [Fact]
        public async Task TestCommentAfterStatementInTopLevelProgramAsync()
        {
            var testCode = @"System.Console.WriteLine(1);
[|//|] Comment
System.Console.WriteLine(2);
";

            var fixedCode = @"System.Console.WriteLine(1);

// Comment
System.Console.WriteLine(2);
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
