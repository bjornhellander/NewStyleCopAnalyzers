// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp13.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1137ElementsShouldHaveTheSameIndentation,
        StyleCop.Analyzers.ReadabilityRules.IndentationCodeFixProvider>;

    public partial class SA1137CSharp14UnitTests : SA1137CSharp13UnitTests
    {
        [Fact]
        public async Task TestExtensionDeclarationAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension(string source)
    {
        public int Length1 => source.Length;
         public int Length2 => source.Length;
       public int Length3 => source.Length;
    }
}
";

            var fixedCode = @"
public static class TestClass
{
    extension(string source)
    {
        public int Length1 => source.Length;
        public int Length2 => source.Length;
        public int Length3 => source.Length;
    }
}
";

            // TODO: Syntax node actions seems to be triggered twice
            // Reported in https://github.com/dotnet/roslyn/issues/80319
            var expected = new[]
            {
                Diagnostic().WithSpan(7, 1, 7, 10),
                Diagnostic().WithSpan(7, 1, 7, 10),
                Diagnostic().WithSpan(8, 1, 8, 8),
                Diagnostic().WithSpan(8, 1, 8, 8),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestGenericExtensionDeclarationConstraintsAsync()
        {
            var testCode = @"
public static class TestClass
{
    extension<T1, T2, T3>((T1, T2, T3) source)
        where T1 : class
         where T2 : class
       where T3 : class
    {
    }
}
";

            var fixedCode = @"
public static class TestClass
{
    extension<T1, T2, T3>((T1, T2, T3) source)
        where T1 : class
        where T2 : class
        where T3 : class
    {
    }
}
";

            // TODO: Syntax node actions seems to be triggered twice
            // Reported in https://github.com/dotnet/roslyn/issues/80319
            var expected = new[]
            {
                Diagnostic().WithSpan(6, 1, 6, 10),
                Diagnostic().WithSpan(6, 1, 6, 10),
                Diagnostic().WithSpan(7, 1, 7, 8),
                Diagnostic().WithSpan(7, 1, 7, 8),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
