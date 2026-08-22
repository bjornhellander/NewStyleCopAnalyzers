// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1311StaticReadonlyFieldsMustBeginWithUpperCaseLetter,
        StyleCop.Analyzers.NamingRules.RenameToUpperCaseCodeFixProvider>;

    public partial class SA1311CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionStaticReadonlyFieldStartingWithLowerCaseLetterAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    private static readonly int [|myField|] = 1;
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    private static readonly int MyField = 1;
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
