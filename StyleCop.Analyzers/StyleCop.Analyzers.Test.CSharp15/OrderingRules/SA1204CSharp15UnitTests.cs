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
        StyleCop.Analyzers.OrderingRules.SA1204StaticElementsMustAppearBeforeInstanceElements,
        StyleCop.Analyzers.OrderingRules.ElementOrderCodeFixProvider>;

    public partial class SA1204CSharp15UnitTests : SA1204CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionStaticMemberAfterInstanceMemberAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private string InstanceProperty => string.Empty;

    private static string [|StaticProperty|] => string.Empty;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionStaticMemberBeforeInstanceMemberAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static string StaticProperty => string.Empty;

    private string InstanceProperty => string.Empty;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
