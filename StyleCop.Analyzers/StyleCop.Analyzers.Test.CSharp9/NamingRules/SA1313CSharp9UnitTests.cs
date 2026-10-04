// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1313ParameterNamesMustBeginWithLowerCaseLetter,
        StyleCop.Analyzers.NamingRules.RenameToLowerCaseCodeFixProvider>;

    public partial class SA1313CSharp9UnitTests
    {
        [Fact]
        public async Task TestPositionalRecordAsync()
        {
            var testCode = @"
public record R(int A)
{
    public R(int [|A|], int [|B|])
        : this(A)
    {
    }
}
";

            var fixedCode = @"
public record R(int A)
{
    public R(int a, int b)
        : this(a)
    {
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestLambdaDiscardParametersAsync()
        {
            var testCode = @"
public class TypeName
{
    public void MethodName()
    {
        System.Func<int, int, int> function1 = (_, _) => 0;
        System.Func<int, int, int> function2 = (int _, int _) => 0;
        System.Func<int, int, int, int> function3 = (_, _, _) => 0;
        System.Func<int, int, int> function4 = delegate(int _, int _) { return 0; };
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
