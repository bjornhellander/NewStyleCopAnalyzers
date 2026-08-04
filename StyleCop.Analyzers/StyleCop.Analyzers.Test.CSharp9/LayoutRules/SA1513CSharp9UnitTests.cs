// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1513ClosingBraceMustBeFollowedByBlankLine,
        StyleCop.Analyzers.LayoutRules.SA1513CodeFixProvider>;

    public partial class SA1513CSharp9UnitTests
    {
        [Fact]
        public async Task TestThrowSwitchExpressionValueAsync()
        {
            var testCode = @"using System;

public class Foo
{
    public void Baz(string arg)
    {
        throw arg switch
        {
            """" => new ArgumentException(),
            _ => new Exception()
        };
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, testCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInitAccessorAsync()
        {
            var testCode = @"using System;

public class Foo
{
    public int X
    {
        get
        {
            return 0;
        }
        init
        {
        }
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
