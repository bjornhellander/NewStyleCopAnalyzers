// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1122UseStringEmptyForEmptyStrings,
        StyleCop.Analyzers.ReadabilityRules.SA1122CodeFixProvider>;

    public partial class SA1122CSharp8UnitTests
    {
        /// <summary>
        /// Verifies the analyzer will properly handle an empty string in a switch expression.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestEmptyStringInSwitchExpressionAsync()
        {
            string testCode = @"
public class TestClass
{
    public void TestMethod(string condition)
    {
        _ = [|""""|] switch
        {
        """" when condition == [|""""|] =>
            0,
        ("""" + ""a"") when condition == [|""""|] =>
            0,
        };
    }
}
";
            string fixedCode = @"
public class TestClass
{
    public void TestMethod(string condition)
    {
        _ = string.Empty switch
        {
        """" when condition == string.Empty =>
            0,
        ("""" + ""a"") when condition == string.Empty =>
            0,
        };
    }
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestEmptyStringInTuplePatternAsync()
        {
            string testCode = @"
public class TestClass
{
    public bool TestMethod((string, string) condition)
    {
        return condition is ("""", null);
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestEmptyStringInRecursivePatternAsync()
        {
            string testCode = @"
using System.Collections.Generic;
public class TestClass
{
    public bool TestMethod(KeyValuePair<string, string> condition)
    {
        return condition is { Key: """" };
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        /// <summary>
        /// Verifies that an empty interpolated verbatim string is not reported. C# 8 allows these strings to be
        /// written as <c>@$"..."</c> as well as <c>$@"..."</c>.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        // TODO: Should this trigger?
        [Fact]
        public async Task TestEmptyInterpolatedVerbatimStringAsync()
        {
            string testCode = @"
public class TestClass
{
    public string TestMethod()
    {
        return @$"""";
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
