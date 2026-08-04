// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    public partial class SA1648CSharp8UnitTests
    {
        [Theory]
        [InlineData("void TestMethod() {}")]
        [InlineData("int TestProperty { get; set; }")]
        [InlineData("event System.Action TestEvent;")]
        public async Task TestIncorrectMemberInheritDocFromStaticMemberInInterfaceAsync(string member)
        {
            var testCode = $@"
public interface TestInterface
{{
    /// <summary>
    /// A summary text.
    /// </summary>
    static {member}
}}

public class TestClass : TestInterface
{{
    /// [|<inheritdoc />|]
    public static {member}
}}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
