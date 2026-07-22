// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1137ElementsShouldHaveTheSameIndentation,
        StyleCop.Analyzers.ReadabilityRules.IndentationCodeFixProvider>;

    public partial class SA1137CSharp15UnitTests : SA1137CSharp14UnitTests
    {
        [Fact]
        public async Task TestSingleLineCollectionExpressionWithElementAsync()
        {
            string testCode = @"
using System.Collections.Generic;

class TestClass
{
    private List<string> testField = [with(capacity: 10), ""a"", ""b""];
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestMultiLineCollectionExpressionWithElementEstablishesIndentationAsync()
        {
            string testCode = @"
using System.Collections.Generic;

class TestClass
{
    private List<string> testField =
    [
        with(capacity: 10),
[|         |]""a"",
[|       |]""b""
[|     |]];
}
";

            string fixedCode = @"
using System.Collections.Generic;

class TestClass
{
    private List<string> testField =
    [
        with(capacity: 10),
        ""a"",
        ""b""
    ];
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionMembersWithInconsistentIndentationAsync()
        {
            string testCode = @"
public union TestUnion(string, int)
{
    public string DisplayName => """";
[|      |]public int Count => 0;
}
";

            string fixedCode = @"
public union TestUnion(string, int)
{
    public string DisplayName => """";
    public int Count => 0;
}
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
