// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.LayoutRules.SA1504AllAccessorsMustBeSingleLineOrMultiLine,
        StyleCop.Analyzers.LayoutRules.SA1504CodeFixProvider>;

    public partial class SA1504CSharp9UnitTests
    {
        [Theory]
        [InlineData("int Prop")]
        [InlineData("int this[int index]")]
        public async Task TestGetInOneLineInitInMultipleLinesAsync(string propertyDeclaration)
        {
            var testCode = $@"
public class Foo
{{
    public {propertyDeclaration}
    {{
        [|get|] {{ return 1; }}
        init
        {{
        }}
    }}
}}";

            var fixedTestCodeSingle = $@"
public class Foo
{{
    public {propertyDeclaration}
    {{
        get {{ return 1; }}
        init {{ }}
    }}
}}";

            var fixedTestCodeMultiple = $@"
public class Foo
{{
    public {propertyDeclaration}
    {{
        get
        {{
            return 1;
        }}

        init
        {{
        }}
    }}
}}";

            await new CSharpTest
            {
                TestCode = testCode,
                FixedCode = fixedTestCodeSingle,
                CodeActionIndex = 0,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);

            await new CSharpTest
            {
                TestCode = testCode,
                FixedCode = fixedTestCodeMultiple,
                CodeActionIndex = 1,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }
    }
}
