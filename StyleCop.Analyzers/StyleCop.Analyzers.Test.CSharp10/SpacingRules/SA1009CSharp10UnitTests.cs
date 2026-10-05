// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    using static StyleCop.Analyzers.SpacingRules.SA1009ClosingParenthesisMustBeSpacedCorrectly;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1009ClosingParenthesisMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1009CSharp10UnitTests
    {
        // TODO: A theory in a base test class would have made this unnecessary
        [Fact]
        public async Task TestRecordStructPrimaryConstructorAsync()
        {
            var testCode = @"
public interface IQuery
{
}

public record struct MyQuery1(int X {|#0:)|}: IQuery;

public record struct MyQuery2(int X {|#1:)|} : IQuery;

public record struct MyQuery3(int X{|#2:)|}: IQuery;
";

            var fixedCode = @"
public interface IQuery
{
}

public record struct MyQuery1(int X) : IQuery;

public record struct MyQuery2(int X) : IQuery;

public record struct MyQuery3(int X) : IQuery;
";

            var expected = new[]
            {
                Diagnostic(DescriptorNotPreceded).WithLocation(0),
                Diagnostic(DescriptorFollowed).WithLocation(0),
                Diagnostic(DescriptorNotPreceded).WithLocation(1),
                Diagnostic(DescriptorFollowed).WithLocation(2),
            };
            await VerifyCSharpFixAsync(testCode, expected, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestLineSpanDirectiveAsync()
        {
            // #line span directives are primarily used in generated code, so their parentheses are not checked
            var testCode = @"public class TestClass
{
    public void TestMethod()
    {
#line (1, 1) - (5, 60) 10 ""file.cs""
        int x = 0;
#line ( 2, 3 )-(4,5 ) ""file.cs""
        int y = 0;
#line (3, 4)  -  ( 5, 6) 10 ""file.cs""
        int z = 0;
#line default
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
