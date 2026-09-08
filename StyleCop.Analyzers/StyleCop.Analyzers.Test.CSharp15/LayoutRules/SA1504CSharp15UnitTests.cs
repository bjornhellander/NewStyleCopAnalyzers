// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1504AllAccessorsMustBeSingleLineOrMultiLine,
        StyleCop.Analyzers.LayoutRules.SA1504CodeFixProvider>;

    public partial class SA1504CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionPropertyWithMixedAccessorStyleAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static int backingField;

    public static int TestProperty
    {
        [|get|] { return backingField; }

        set
        {
            backingField = value;
        }
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    private static int backingField;

    public static int TestProperty
    {
        get { return backingField; }
        set { backingField = value; }
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
