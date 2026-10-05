// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1312VariableNamesMustBeginWithLowerCaseLetter,
        StyleCop.Analyzers.NamingRules.RenameToLowerCaseCodeFixProvider>;

    public partial class SA1312CSharp10UnitTests
    {
        [Fact]
        public async Task TestMixedDeconstructionAsync()
        {
            var testCode = @"public class TypeName
{
    public void MethodName()
    {
        int x = 0;
        (x, var {|#0:Y|}) = (1, 2);
        (int {|#1:Z|}, x) = (3, 4);
    }
}";

            var fixedCode = @"public class TypeName
{
    public void MethodName()
    {
        int x = 0;
        (x, var y) = (1, 2);
        (int z, x) = (3, 4);
    }
}";

            var expected = new[]
            {
                Diagnostic().WithArguments("Y").WithLocation(0),
                Diagnostic().WithArguments("Z").WithLocation(1),
            };
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
