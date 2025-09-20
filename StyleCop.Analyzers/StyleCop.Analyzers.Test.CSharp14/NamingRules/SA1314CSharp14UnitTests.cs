// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp13.NamingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1314TypeParameterNamesMustBeginWithT,
        StyleCop.Analyzers.NamingRules.SA1314CodeFixProvider>;

    public partial class SA1314CSharp14UnitTests : SA1314CSharp13UnitTests
    {
        [Fact]
        public async Task TestExtensionDeclarationeAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension<{|#0:Obj|}>(Obj param)
    {
    }
}
";

            var fixedCode = @"
public static class TestClass
{
    extension<TObj>(TObj param)
    {
    }
}
";

            // TODO: Syntax node actions seems to be triggered twice
            // Reported in https://github.com/dotnet/roslyn/issues/80319
            var expected = new[]
            {
                Diagnostic().WithLocation(0),
                Diagnostic().WithLocation(0),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
