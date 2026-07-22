// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.DocumentationRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1642ConstructorSummaryDocumentationMustBeginWithStandardText,
        StyleCop.Analyzers.DocumentationRules.SA1642SA1643CodeFixProvider>;

    public partial class SA1642CSharp15UnitTests : SA1642CSharp14UnitTests
    {
        [Fact]
        public async Task TestUnionStaticConstructorWithStructWordingAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// Initializes static members of the <see cref=""TestUnion""/> struct.
    /// </summary>
    static TestUnion()
    {
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionStaticConstructorWithClassWordingAsync()
        {
            // TODO: Report bug - The compiler calls the registered constructor action three times
            var testCode = @"
public union TestUnion(string, int)
{
    /// {|#0:<summary>
    /// Initializes static members of the <see cref=""TestUnion""/> class.
    /// </summary>|}
    static TestUnion()
    {
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// Initializes static members of the <see cref=""TestUnion""/> struct.
    /// Initializes static members of the <see cref=""TestUnion""/> class.
    /// </summary>
    static TestUnion()
    {
    }
}
";

            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionInstanceConstructorWithStructWordingAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// Initializes a new instance of the <see cref=""TestUnion""/> struct.
    /// </summary>
    public TestUnion()
        : this(string.Empty)
    {
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionInstanceConstructorWithClassWordingAsync()
        {
            // TODO: Report bug - The compiler calls the registered constructor action three times
            var testCode = @"
public union TestUnion(string, int)
{
    /// {|#0:<summary>
    /// Initializes a new instance of the <see cref=""TestUnion""/> class.
    /// </summary>|}
    public TestUnion()
        : this(string.Empty)
    {
    }
}
";

            var fixedCode = @"
public union TestUnion(string, int)
{
    /// <summary>
    /// Initializes a new instance of the <see cref=""TestUnion""/> struct.
    /// Initializes a new instance of the <see cref=""TestUnion""/> class.
    /// </summary>
    public TestUnion()
        : this(string.Empty)
    {
    }
}
";

            var expected = new[] { Diagnostic().WithLocation(0), Diagnostic().WithLocation(0), Diagnostic().WithLocation(0) };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
