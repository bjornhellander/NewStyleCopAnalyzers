// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1400AccessModifierMustBeDeclared,
        StyleCop.Analyzers.MaintainabilityRules.SA1400CodeFixProvider>;

    public partial class SA1400CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionWithoutAccessModifierAsync()
        {
            var testCode = @"
union {|#0:TestUnion|}(string, int)
{
}
";

            var fixedCode = @"
internal union TestUnion(string, int)
{
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("TestUnion");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestNestedUnionWithoutAccessModifierAsync()
        {
            var testCode = @"
public class OuterClass
{
    union {|#0:TestUnion|}(string, int)
    {
    }
}
";

            var fixedCode = @"
public class OuterClass
{
    private union TestUnion(string, int)
    {
    }
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("TestUnion");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionWithAttributesWithoutAccessModifierAsync()
        {
            var testCode = @"
using System;

[Obsolete]
union {|#0:TestUnion|}(string, int)
{
}
";

            var fixedCode = @"
using System;

[Obsolete]
internal union TestUnion(string, int)
{
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("TestUnion");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnionWithDirectivesWithoutAccessModifierAsync()
        {
            var testCode = @"
#if true
union {|#0:TestUnion|}(string, int)
{
}
#endif
";

            var fixedCode = @"
#if true
internal union TestUnion(string, int)
{
}
#endif
";

            var expected = Diagnostic().WithLocation(0).WithArguments("TestUnion");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestClosedClassWithoutAccessModifierAsync()
        {
            var testCode = @"
closed class {|#0:TestClosed|}
{
}
";

            var fixedCode = @"
internal closed class TestClosed
{
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("TestClosed");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestSafeExplicitLayoutFieldWithoutAccessModifierAsync()
        {
            var testCode = @"
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
internal struct TestStruct
{
    [FieldOffset(0)]
    safe int {|#0:Value|};
}
";

            var fixedCode = @"
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
internal struct TestStruct
{
    [FieldOffset(0)]
    private safe int Value;
}
";

            var expected = Diagnostic().WithLocation(0).WithArguments("Value");

            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
