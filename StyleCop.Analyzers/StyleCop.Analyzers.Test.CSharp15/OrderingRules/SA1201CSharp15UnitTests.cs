// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp14.OrderingRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1201ElementsMustAppearInTheCorrectOrder,
        StyleCop.Analyzers.OrderingRules.ElementOrderCodeFixProvider>;

    public partial class SA1201CSharp15UnitTests : SA1201CSharp14UnitTests
    {
        [Fact]
        public async Task TestTopLevelUnionAfterClassAsync()
        {
            string testCode = @"
public class MyClass;
public union {|#0:MyUnion|}(string, int);
";

            var expected = Diagnostic().WithLocation(0).WithArguments("union", "class");

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestTopLevelUnionAfterStructAsync()
        {
            string testCode = @"
public struct MyStruct;
public union {|#0:MyUnion|}(string, int);
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestTopLevelUnionBeforeClassAsync()
        {
            string testCode = @"
public union MyUnion(string, int);
public class MyClass;
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestTopLevelUnionBeforeStructAsync()
        {
            string testCode = @"
public union MyUnion(string, int);
public struct MyStruct;
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionAfterClassInUnionContainerAsync()
        {
            string testCode = @"
public union TestUnion(string, int)
{
    public class MyClass;
    public union {|#0:MyUnion|}(string, int);
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("union", "class");

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionAfterStructInUnionContainerAsync()
        {
            string testCode = @"
public union TestUnion(string, int)
{
    public struct MyStruct;
    public union {|#0:MyUnion|}(string, int);
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionBeforeClassInUnionContainerAsync()
        {
            string testCode = @"
public union TestUnion(string, int)
{
    public union MyUnion(string, int);
    public class MyClass;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionBeforeStructInUnionContainerAsync()
        {
            string testCode = @"
public union TestUnion(string, int)
{
    public union MyUnion(string, int);
    public struct MyStruct;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionFieldAfterPropertyInUnionContainerAsync()
        {
            string testCode = @"
public union TestUnion(string, int)
{
    public string DisplayName => """";

    private static int {|#0:counter|};
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("field", "property");

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionPropertyAfterFieldInUnionContainerAsync()
        {
            string testCode = @"
public union TestUnion(string, int)
{
    private static int counter;

    public string DisplayName => """";
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionAfterClassInClassContainerAsync()
        {
            string testCode = @"
public class Container
{
    public class MyClass;
    public union {|#0:MyUnion|}(string, int);
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("union", "class");

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionBeforeClassInClassContainerAsync()
        {
            string testCode = @"
public class Container
{
    public union MyUnion(string, int);
    public class MyClass;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionAfterStructInStructContainerAsync()
        {
            string testCode = @"
public struct Container
{
    public struct MyStruct;
    public union MyUnion(string, int);
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionBeforeStructInStructContainerAsync()
        {
            string testCode = @"
public struct Container
{
    public union MyUnion(string, int);
    public struct MyStruct;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestMultipleUnionsInSameFileAsync()
        {
            string testCode = @"
public union FirstUnion(string, int)
{
    public string DisplayName => """";

    private static int {|#0:firstCounter|};
}

public union SecondUnion(bool, double)
{
    public string DisplayName => """";

    private static int {|#1:secondCounter|};
}
";

            var expected = new[]
            {
                Diagnostic().WithLocation(0).WithArguments("field", "property"),
                Diagnostic().WithLocation(1).WithArguments("field", "property"),
            };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
