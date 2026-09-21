// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1304NonPrivateReadonlyFieldsMustBeginWithUpperCaseLetter,
        StyleCop.Analyzers.NamingRules.RenameToUpperCaseCodeFixProvider>;

    public partial class SA1304CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionNonPrivateReadonlyFieldStartingWithLowerCaseLetterAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    internal static readonly int [|myField|] = 1;
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    internal static readonly int MyField = 1;
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
