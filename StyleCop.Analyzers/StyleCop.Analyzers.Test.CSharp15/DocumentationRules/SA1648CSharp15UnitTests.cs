// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    public partial class SA1648CSharp15UnitTests
    {
        [Fact]
        public async Task TestUnionWithInheritdocAndBaseListAsync()
        {
            var testCode = @"
/// <inheritdoc/>
public union TestUnion(string, int) : System.IComparable
{
    public int CompareTo(object obj) => 0;
}
";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
