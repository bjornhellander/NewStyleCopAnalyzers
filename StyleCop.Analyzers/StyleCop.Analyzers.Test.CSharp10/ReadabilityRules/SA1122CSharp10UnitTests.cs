// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1122UseStringEmptyForEmptyStrings,
        StyleCop.Analyzers.ReadabilityRules.SA1122CodeFixProvider>;

    public partial class SA1122CSharp10UnitTests
    {
        /// <summary>
        /// Verifies that an empty interpolated string is not reported where it has to be constant, which is allowed
        /// from C# 10.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestConstantEmptyInterpolatedStringAsync()
        {
            var testCode = @"using System;

public class TestClass
{
    private const string A = $"""";

    [Obsolete($"""")]
    public void TestMethod(string value)
    {
        const string b = $"""";
        string c = [|$""""|];

        switch (value)
        {
        case $"""":
            break;
        }
    }
}";

            var fixedCode = @"using System;

public class TestClass
{
    private const string A = $"""";

    [Obsolete($"""")]
    public void TestMethod(string value)
    {
        const string b = $"""";
        string c = string.Empty;

        switch (value)
        {
        case $"""":
            break;
        }
    }
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
