// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1212PropertyAccessorsMustFollowOrder,
        StyleCop.Analyzers.OrderingRules.SA1212SA1213CodeFixProvider>;

    public partial class SA1212CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionPropertyWithWrongOrderAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static int backingValue;

    public static int MyValue
    {
        [|set { backingValue = value; }|]
        get { return backingValue; }
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    private static int backingValue;

    public static int MyValue
    {
        get { return backingValue; }
        set { backingValue = value; }
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
