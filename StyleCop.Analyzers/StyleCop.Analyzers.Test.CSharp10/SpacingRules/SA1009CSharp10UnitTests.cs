// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
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
    }
}
