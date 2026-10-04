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
        StyleCop.Analyzers.OrderingRules.SA1206DeclarationKeywordsMustFollowOrder,
        StyleCop.Analyzers.OrderingRules.SA1206CodeFixProvider>;

    public partial class SA1206CSharp9UnitTests
    {
        [Theory]
        [MemberData(nameof(CommonMemberData.ReferenceTypeKeywordsWhichSupportPrimaryConstructors), MemberType = typeof(CommonMemberData))]
        public async Task TestModifierOrderInTypeWithPrimaryConstructorAsync(string typeKeyword)
        {
            var testCode = $@"sealed {{|#0:public|}} {typeKeyword} TestType(int X);";

            var fixedCode = $@"public sealed {typeKeyword} TestType(int X);";

            var expected = this.GetExpectedResultTestModifierOrderInTypeWithPrimaryConstructor();
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestModifierOrderInAnonymousFunctionsAsync()
        {
            var testCode = @"using System;
using System.Threading.Tasks;

public class TestClass
{
    public void TestMethod()
    {
        Func<int, Task> a = async {|#0:static|} x => await Task.Delay(x);
        Func<int, Task> b = async {|#1:static|} (x) => await Task.Delay(x);
        Func<int, Task> c = async {|#2:static|} delegate (int x) { await Task.Delay(x); };
    }
}";

            var fixedCode = @"using System;
using System.Threading.Tasks;

public class TestClass
{
    public void TestMethod()
    {
        Func<int, Task> a = static async x => await Task.Delay(x);
        Func<int, Task> b = static async (x) => await Task.Delay(x);
        Func<int, Task> c = static async delegate (int x) { await Task.Delay(x); };
    }
}";

            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(0).WithArguments("static", "async"),
                Diagnostic().WithLocation(1).WithArguments("static", "async"),
                Diagnostic().WithLocation(2).WithArguments("static", "async"),
            };

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        protected virtual DiagnosticResult[] GetExpectedResultTestModifierOrderInTypeWithPrimaryConstructor()
        {
            return new[]
            {
                // Diagnostic issued twice because of https://github.com/dotnet/roslyn/issues/53136
                Diagnostic().WithLocation(0).WithArguments("public", "sealed"),
                Diagnostic().WithLocation(0).WithArguments("public", "sealed"),
            };
        }
    }
}
