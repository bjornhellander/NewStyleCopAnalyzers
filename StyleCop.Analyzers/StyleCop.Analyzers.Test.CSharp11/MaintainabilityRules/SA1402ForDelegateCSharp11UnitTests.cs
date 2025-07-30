// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp10.MaintainabilityRules;
    using Xunit;

    public partial class SA1402ForDelegateCSharp11UnitTests : SA1402ForDelegateCSharp10UnitTests
    {
        [Fact]
        [WorkItem(3803, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3803")]
        public async Task TestFileModifierAsync()
        {
            var testCode = $@"
public class TestType1 {{ }}
file delegate void TestType2();
";

            await VerifyCSharpDiagnosticAsync(
                testCode,
                this.GetSettings(),
                DiagnosticResult.EmptyDiagnosticResults,
                CancellationToken.None).ConfigureAwait(false);
        }
    }
}
