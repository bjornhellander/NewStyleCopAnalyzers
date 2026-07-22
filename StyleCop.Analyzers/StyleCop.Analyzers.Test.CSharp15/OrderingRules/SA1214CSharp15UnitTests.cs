// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.OrderingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1214ReadonlyElementsMustAppearBeforeNonReadonlyElements,
        StyleCop.Analyzers.OrderingRules.ElementOrderCodeFixProvider>;

    public partial class SA1214CSharp15UnitTests : SA1214CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionReadonlyFieldAfterNonReadonlyFieldAsync()
        {
            // Instance fields are not permitted in a union declaration, so both fields here are static.
            var testCode = @"
public union TestUnion(string, int)
{
    private static int nonReadonlyField;
    private static readonly int [|readonlyField|];
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionReadonlyFieldBeforeNonReadonlyFieldAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static readonly int readonlyField;
    private static int nonReadonlyField;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
