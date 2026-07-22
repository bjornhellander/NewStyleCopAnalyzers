// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1626SingleLineCommentsMustNotUseDocumentationStyleSlashes,
        StyleCop.Analyzers.DocumentationRules.SA1626CodeFixProvider>;

    public class SA1626UnitTests
    {
        [Fact]
        public async Task TestClassWithXmlCommentAsync()
        {
            var testCode = @"/// <summary>
/// XML Documentation
/// </summary>
public class TypeName
{
    public void Bar()
    {
    }
}
";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestMethodWithCommentAsync()
        {
            var testCode = @"public class TypeName
{
    public void Bar()
    {
        // This is a comment
    }
}
";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestMethodWithOneLineThreeSlashCommentAsync()
        {
            var testCode = @"public class TypeName
{
    public void Bar()
    {
        /// This is a comment
    }
}
";
            var fixedCode = @"public class TypeName
{
    public void Bar()
    {
        // This is a comment
    }
}
";

            DiagnosticResult expected = Diagnostic().WithLocation(5, 9);
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestMethodWithMultiLineThreeSlashCommentAsync()
        {
            var testCode = @"public class TypeName
{
    public void Bar()
    {
        /// This is
        /// a comment
    }
}
";
            var fixedCode = @"public class TypeName
{
    public void Bar()
    {
        // This is
        // a comment
    }
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(5, 9),
                Diagnostic().WithLocation(6, 9),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestMethodWithCodeCommentsAsync()
        {
            var testCode = @"public class TypeName
{
    public void Bar()
    {
        //// System.Console.WriteLine(""Bar"")
    }
}
";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestMethodWithSingeLineDocumentationAsync()
        {
            var testCode = @"public class TypeName
{
    /// <summary>Summary text</summary>
    public void Bar()
    {
    }
}
";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
