// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1642ConstructorSummaryDocumentationMustBeginWithStandardText,
        StyleCop.Analyzers.DocumentationRules.SA1642SA1643CodeFixProvider>;

    public partial class SA1642CSharp15UnitTests
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
            var testCode = @"
public union TestUnion(string, int)
{
    /// [|<summary>
    /// Initializes static members of the <see cref=""TestUnion""/> class.
    /// </summary>|]
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

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
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
            var testCode = @"
public union TestUnion(string, int)
{
    /// [|<summary>
    /// Initializes a new instance of the <see cref=""TestUnion""/> class.
    /// </summary>|]
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

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestGenericUnionStaticConstructorWithClassWordingAsync()
        {
            var testCode = @"
public union TestUnion<T>(T, int)
{
    /// [|<summary>
    /// Initializes static members of the <see cref=""TestUnion{T}""/> class.
    /// </summary>|]
    static TestUnion()
    {
    }
}
";

            var fixedCode = @"
public union TestUnion<T>(T, int)
{
    /// <summary>
    /// Initializes static members of the <see cref=""TestUnion{T}""/> struct.
    /// Initializes static members of the <see cref=""TestUnion{T}""/> class.
    /// </summary>
    static TestUnion()
    {
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
