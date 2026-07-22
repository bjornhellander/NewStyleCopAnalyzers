// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1128ConstructorInitializerMustBeOnOwnLine,
        StyleCop.Analyzers.ReadabilityRules.SA1128CodeFixProvider>;

    public class SA1128UnitTests
    {
        public static TheoryData<string> NullTests { get; } = new TheoryData<string>()
        {
            $"class Foo\r\n{{\r\n}}",
            $"class Foo\r\n{{\r\n    public Foo() {{}}\r\n}}",
            $"class Foo\r\n{{\r\n    public Foo(int bar) {{}}\r\n}}",
        };

        [Theory]
        [MemberData(nameof(NullTests))]
        public async Task TestNullScenariosAsync(string declaration)
        {
            await VerifyCSharpDiagnosticAsync(declaration, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestViolationWithBaseInitializerOnSameLineAsync()
        {
            var testCode = @"
public class TypeName
{
    public TypeName() : base()
    {
    }
}";
            var fixedCode = @"
public class TypeName
{
    public TypeName()
        : base()
    {
    }
}";
            var expected = Diagnostic().WithLocation(4, 23);
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestViolationWithThisInitializerOnSameLineAsync()
        {
            var testCode = @"
public class TypeName
{
    public TypeName() : this(0)
    {
    }

    public TypeName(int value)
    {
    }
}";
            var fixedCode = @"
public class TypeName
{
    public TypeName()
        : this(0)
    {
    }

    public TypeName(int value)
    {
    }
}";
            var expected = Diagnostic().WithLocation(4, 23);
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestViolationWithColonOnDifferentLineAsync()
        {
            var testCode = @"
public class TypeName
{
    public TypeName() 
        : 
        this(0)
    {
    }

    public TypeName(int value)
    {
    }
}";
            var fixedCode = @"
public class TypeName
{
    public TypeName()
        : this(0)
    {
    }

    public TypeName(int value)
    {
    }
}";
            var expected = Diagnostic().WithLocation(5, 9);
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestViolationWithColonOnSameLineAsync()
        {
            var testCode = @"
public class TypeName
{
    public TypeName() : 
        this(0)
    {
    }

    public TypeName(int value)
    {
    }
}";
            var fixedCode = @"
public class TypeName
{
    public TypeName()
        : this(0)
    {
    }

    public TypeName(int value)
    {
    }
}";
            var expected = Diagnostic().WithLocation(4, 23);
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestViolationWithCommentsAsync()
        {
            var testCode = @"
public class TypeName
{
    public TypeName() /* c1 */ : /* c2 */ this(0) /* c3 */
    {
    }

    public TypeName(int value)
    {
    }
}";
            var fixedCode = @"
public class TypeName
{
    public TypeName() /* c1 */
        : /* c2 */ this(0) /* c3 */
    {
    }

    public TypeName(int value)
    {
    }
}";
            var expected = Diagnostic().WithLocation(4, 32);
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
