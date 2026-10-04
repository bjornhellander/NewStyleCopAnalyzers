// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp6.Helpers;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1202ElementsMustBeOrderedByAccess,
        StyleCop.Analyzers.OrderingRules.ElementOrderCodeFixProvider>;

    public partial class SA1202CSharp9UnitTests
    {
        /// <summary>
        /// Verifies that partial methods with access modifiers are ordered by their declared accessibility, in each part.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestPartialMethodsWithAccessModifiersAsync()
        {
            var testCode = @"public partial class TestClass
{
    private partial bool Bar();

    public partial int {|#0:Foo|}();
}

public partial class TestClass
{
    private partial bool Bar() => true;

    public partial int {|#1:Foo|}() => 0;
}
";

            var fixedCode = @"public partial class TestClass
{
    public partial int Foo();

    private partial bool Bar();
}

public partial class TestClass
{
    public partial int Foo() => 0;

    private partial bool Bar() => true;
}
";

            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(0).WithArguments("public", "private"),
                Diagnostic().WithLocation(1).WithArguments("public", "private"),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Theory]
        [MemberData(nameof(CommonMemberData.TypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestMemberOrderInTypeWithPrimaryConstructorAsync(string typeKeyword)
        {
            var testCode = $@"public {typeKeyword} TestType(int X)
{{
    private int Bar() => 0;

    public int {{|#0:Foo|}}() => 0;
}}";

            var fixedCode = $@"public {typeKeyword} TestType(int X)
{{
    public int Foo() => 0;

    private int Bar() => 0;
}}";

            var expected = this.GetExpectedResultTestMemberOrderInTypeWithPrimaryConstructor();
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestMemberOrderInTypeWithPrimaryConstructor()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/53136
                Diagnostic().WithLocation(0).WithArguments("public", "private"),
                Diagnostic().WithLocation(0).WithArguments("public", "private"),
            };
        }
    }
}
