// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.NamingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.NamingRules.SA1316TupleElementNamesShouldUseCorrectCasing,
        StyleCop.Analyzers.NamingRules.SA1316CodeFixProvider>;

    public partial class SA1316CSharp8UnitTests
    {
        // TODO: Use from base class instead
        private const string PascalCaseTestSettings = @"
{
  ""settings"": {
    ""namingRules"": {
      ""tupleElementNameCasing"": ""PascalCase""
    }
  }
}
";

        /// <summary>
        /// Verifies that the names of an await foreach deconstruction, which C# 8 introduced, are exempt from the
        /// configured casing just like those of an ordinary foreach deconstruction.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task TestAwaitForEachDeconstructionAsync()
        {
            var testCode = @"
using System.Collections.Generic;
using System.Threading.Tasks;

public class TypeName
{
    public async Task MethodNameAsync(IAsyncEnumerable<(string Name, string Value)> list)
    {
        await foreach ((string name, string value) in list)
        {
        }
    }
}
";

            await VerifyCSharpDiagnosticAsync(testCode, PascalCaseTestSettings, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
