// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Lightup;
    using StyleCop.Analyzers.OrderingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1200UsingDirectivesMustBePlacedCorrectly,
        StyleCop.Analyzers.OrderingRules.UsingCodeFixProvider>;

    /// <summary>
    /// Unit tests for the <see cref="SA1200UsingDirectivesMustBePlacedCorrectly"/>.
    /// </summary>
    public class SA1200UnitTests
    {
        public static TheoryData<string> TypeDefinitions
        {
            get
            {
                var data = new TheoryData<string>()
                {
                    @"public class TestClass {}",
                    @"public struct TestStruct {}",
                    @"public interface TestInterface {}",
                    @"public enum TestEnum { TestValue }",
                    @"public delegate void TestDelegate();",
                };

                if (LightupHelpers.SupportsCSharp9)
                {
                    data.Add(@"public record TestRecord {}");
                }

                if (LightupHelpers.SupportsCSharp10)
                {
                    data.Add(@"public record struct TestRecordStruct {}");
                    data.Add(@"public record class TestRecordClass {}");
                }

                if (LightupHelpers.SupportsCSharp15)
                {
                    data.Add(@"public union TestUnion(string, int) {}");
                }

                return data;
            }
        }

        /// <summary>
        /// Verifies that valid using directives in a namespace does not produce any diagnostics.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestValidUsingDirectivesInNamespaceAsync()
        {
            var testCode = @"namespace TestNamespace
{
    using System;
    using System.Threading;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that having using directives in the compilation unit will not produce any diagnostics when there are type definition present.
        /// </summary>
        /// <param name="typeDefinition">The type definition to test.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [MemberData(nameof(TypeDefinitions))]
        public async Task TestValidUsingDirectivesInCompilationUnitWithTypeDefinitionAsync(string typeDefinition)
        {
            var testCode = $@"using System;

{typeDefinition}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that having using directives in the compilation unit will not produce any diagnostics when there are attributes present.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestValidUsingDirectivesInCompilationUnitWithAttributesAsync()
        {
            var testCode = @"using System.Reflection;

[assembly: AssemblyVersion(""1.0.0.0"")]

namespace TestNamespace
{
    using System;
    using System.Threading;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that having using directives in the compilation unit will produce the expected diagnostics.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestInvalidUsingDirectivesInCompilationUnitAsync()
        {
            var testCode = @"using System;
using System.Threading;

namespace TestNamespace
{
}
";

            var fixedTestCode = @"namespace TestNamespace
{
    using System;
    using System.Threading;
}
";

            DiagnosticResult[] expectedResults =
            {
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(1, 1),
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(2, 1),
            };

            await VerifyCSharpFixAsync(testCode, expectedResults, fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInvalidUsingDirectivesInCompilationUnitWithPragmaAsync()
        {
            var testCode = @"#pragma warning disable 1573 // Comment
using System;
using System.Threading;

namespace TestNamespace
{
}
";

            var fixedTestCode = @"namespace TestNamespace
{
#pragma warning disable 1573 // Comment
    using System;
    using System.Threading;
}
";

            DiagnosticResult[] expectedResults =
            {
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(2, 1),
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(3, 1),
            };

            await VerifyCSharpFixAsync(testCode, expectedResults, fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInvalidUsingDirectivesInCompilationUnitWithRegionBeforeAsync()
        {
            var testCode = @"#region Comment
#endregion Comment
using System;
using System.Threading;

namespace TestNamespace
{
}
";

            var fixedTestCode = @"namespace TestNamespace
{
    #region Comment
    #endregion Comment
    using System;
    using System.Threading;
}
";

            DiagnosticResult[] expectedResults =
            {
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(3, 1),
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(4, 1),
            };

            await VerifyCSharpFixAsync(testCode, expectedResults, fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        [WorkItem(2363, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2363")]
        public async Task TestInvalidUsingDirectivesWithFileHeaderTriviaAsync()
        {
            var testCode = @"// Some comment
using System;
using System.Threading;

namespace TestNamespace
{
}
";

            var fixedTestCode = @"// Some comment
namespace TestNamespace
{
    using System;
    using System.Threading;
}
";

            DiagnosticResult[] expectedResults =
            {
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(2, 1),
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(3, 1),
            };

            await VerifyCSharpFixAsync(testCode, expectedResults, fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        [WorkItem(2363, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2363")]
        public async Task TestInvalidUsingDirectivesWithSeparatedFileHeaderTriviaAsync()
        {
            var testCode = @"// Some comment

using System;
using System.Threading;

namespace TestNamespace
{
}
";

            var fixedTestCode = @"// Some comment

namespace TestNamespace
{
    using System;
    using System.Threading;
}
";

            DiagnosticResult[] expectedResults =
            {
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(3, 1),
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(4, 1),
            };

            await VerifyCSharpFixAsync(testCode, expectedResults, fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        [WorkItem(2363, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2363")]
        public async Task TestInvalidUsingDirectivesWithSeparatedFileHeaderAndTriviaAsync()
        {
            var testCode = @"// File Header

// Leading Comment

using System;
using System.Threading;

namespace TestNamespace
{
}
";

            var fixedTestCode = @"// File Header

namespace TestNamespace
{
    // Leading Comment

    using System;
    using System.Threading;
}
";

            DiagnosticResult[] expectedResults =
            {
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(5, 1),
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(6, 1),
            };

            await VerifyCSharpFixAsync(testCode, expectedResults, fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        [WorkItem(2363, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2363")]
        public async Task TestInvalidUsingDirectivesWithTriviaAsync()
        {
            var testCode = @"
// Some comment
using System;
using System.Threading;

namespace TestNamespace
{
}
";

            var fixedTestCode = @"
namespace TestNamespace
{
    // Some comment
    using System;
    using System.Threading;
}
";

            DiagnosticResult[] expectedResults =
            {
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(3, 1),
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(4, 1),
            };

            await VerifyCSharpFixAsync(testCode, expectedResults, fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        [WorkItem(2363, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2363")]
        public async Task TestInvalidUsingDirectivesWithHeaderAndTriviaAsync()
        {
            var testCode = @"// Copyright notice here

// Some comment
using System;
using System.Threading;

namespace TestNamespace
{
}
";

            var fixedTestCode = @"// Copyright notice here

namespace TestNamespace
{
    // Some comment
    using System;
    using System.Threading;
}
";

            DiagnosticResult[] expectedResults =
            {
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(4, 1),
                Diagnostic(SA1200UsingDirectivesMustBePlacedCorrectly.DescriptorInside).WithLocation(5, 1),
            };

            await VerifyCSharpFixAsync(testCode, expectedResults, fixedTestCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
