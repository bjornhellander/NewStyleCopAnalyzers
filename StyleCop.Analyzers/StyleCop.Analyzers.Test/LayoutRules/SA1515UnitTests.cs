﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.LayoutRules
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1515SingleLineCommentMustBePrecededByBlankLine,
        StyleCop.Analyzers.LayoutRules.SA1515CodeFixProvider>;

    public class SA1515UnitTests
    {
        /// <summary>
        /// Verifies that all known types valid single line comment lines will not produce a diagnostic.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestValidSingleLineCommentsAsync()
        {
            var testCode = @"// A single line comment at the start of the file is valid
namespace Foo
{
    public class Bar
    {
        // A single line comment at the start of the scope is valid
        private int field1;

        // This is valid as well ofcourse
        private int field2;
        private int field3; // This should not trigger ofcourse

#if (SPECIALTEST)
        private int field4;
#else
        // this is also allowed
        private double field4;
#endif

        // This is valid ofcourse
        private int field5;

        // Multiple single line comments
        // directly after each other are valid as well
        public int Baz()
        {
            var x = field1;
            ////return 0;
            return x;
        }

        public void Qux()
        {
            switch (this.Baz())
            {
                case 1:
                    // Single line comment after case statement is valid
                    break;
                default:
                    // Single line comment after default statement is valid
                    return;
            }
        }
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that invalid single line comment lines will produce a diagnostic.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestInvalidSingleLineCommentsAsync()
        {
            var testCode = @"namespace Foo
{
    public class Bar
    {
        private int field1;
        // This is invalid
        private int field2;

#if (SPECIALTEST)
        private int field3;
#endif
        // This is invalid #2
        private int field4;
    }
}
";

            var fixedTestCode = @"namespace Foo
{
    public class Bar
    {
        private int field1;

        // This is invalid
        private int field2;

#if (SPECIALTEST)
        private int field3;
#endif

        // This is invalid #2
        private int field4;
    }
}
";

            DiagnosticResult[] expectedDiagnostic =
            {
                Diagnostic().WithLocation(6, 9),
                Diagnostic().WithLocation(12, 9),
            };

            await VerifyCSharpFixAsync(testCode, expectedDiagnostic, fixedTestCode, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that an invalid single line comment line within a disabled conditional directive will not produce a diagnostic.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestSingleLineCommentWithinConditionalDirectiveAsync()
        {
            var testCode = @"// A single line comment at the start of the file is valid
namespace Foo
{
    public class Bar
    {
#if (SPECIALTEST)
        private int field1;
        // This is invalid 
        private int field2;
#endif

        public int Baz()
        {
            return 0;
        }
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// This is a regression test for DotNetAnalyzers/StyleCopAnalyzers#1083 "SA1515 fires after some directives
        /// when it shouldn't".
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestPreprocessorInteractionAsync()
        {
            string testCode = @"#region Test

// Inside region
using System;

#endregion

// After region
#pragma warning restore 1234

// After pragma
using System.Reflection;

#if DEBUG

// After if
using System.Resources;

#endif

// After endif
using System.Runtime.InteropServices;
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies the analyzer will properly handle documentation followed by a comment.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestDocumentationFollowedByCommentAsync()
        {
            var testCode = @"
/// <summary>some documentation</summary>
// some comment
public class TestClass
{
    /// <summary>more documentation.</summary>
    // another comment
    public void TestMethod() { }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that a comment between two statements with an end of line comment is handled properly.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        [WorkItem(2176, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/2176")]
        public async Task TestCommentBetweenStatementsWithEndOfLineCommentAsync()
        {
            var testCode = @"
public class TestConstants
{
  public const string Constant1 = ""1""; // my end of line comment
  // my comment
  public const string Constant2 = ""2""; // my second end of line comment
}
";

            var fixedTestCode = @"
public class TestConstants
{
  public const string Constant1 = ""1""; // my end of line comment

  // my comment
  public const string Constant2 = ""2""; // my second end of line comment
}
";

            DiagnosticResult[] expectedDiagnostic =
            {
                Diagnostic().WithLocation(5, 3),
            };

            await VerifyCSharpFixAsync(testCode, expectedDiagnostic, fixedTestCode, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that the analyzer will properly handle documentation followed by a comment,
        /// even if there is another non-adjacent comment earlier.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        [WorkItem(3481, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3481")]
        public async Task TestDocumentationFollowedByCommentWhenThereIsAlsoAnEarlierCommentAsync()
        {
            var testCode = @"
public class Class1 // Comment 1
{
    public Class1()
    {
    }

    /// <summary>
    /// Gets value.
    /// </summary>
    // Comment 2
    public double Value { get; }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies that the analyzer does not fire in file headers (i.e. one line comments at the start of the file).
        /// </summary>
        /// <param name="startWithPragma"><see langword="true"/> if the source code should start with a pragma; otherwise, <see langword="false"/>.</param>
        /// <param name="numberOfHeaderLines">The number of lines in the header.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Theory]
        [CombinatorialData]
        [WorkItem(3630, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3630")]
        public async Task TestFileHeaderAsync(
            bool startWithPragma,
            [CombinatorialValues(1, 2, 3)] int numberOfHeaderLines)
        {
            var testCode = @"
class TestClass
{
}";

            for (var i = 0; i < numberOfHeaderLines; i++)
            {
                testCode = "// A comment line." + Environment.NewLine + testCode;
            }

            if (startWithPragma)
            {
                testCode = "#pragma warning disable CS1591" + Environment.NewLine + testCode;
            }

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
