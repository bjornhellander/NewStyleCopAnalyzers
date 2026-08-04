// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    public partial class SA1402ForEnumCSharp11UnitTests
    {
        [Fact]
        public async Task TestFileModifierAsync()
        {
            var testCode = $@"
public class TestType1 {{ }}
file enum TestType2 {{ }}
";

            await VerifyCSharpDiagnosticAsync(
                testCode,
                this.GetSettings(),
                DiagnosticResult.EmptyDiagnosticResults,
                CancellationToken.None).ConfigureAwait(true);
        }
    }
}
