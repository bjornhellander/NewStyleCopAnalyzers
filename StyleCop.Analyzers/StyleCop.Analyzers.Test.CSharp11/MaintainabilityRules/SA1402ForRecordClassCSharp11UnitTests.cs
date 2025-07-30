// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.Test.CSharp10.MaintainabilityRules;
    using Xunit;

    public partial class SA1402ForRecordClassCSharp11UnitTests : SA1402ForRecordClassCSharp10UnitTests
    {
        [Fact]
        [WorkItem(3803, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3803")]
        public async Task TestFileModifierAsync()
        {
            var testCode = $@"
public class TestType1 {{ }}
file record class TestType2 {{ }}
";

            await this.VerifyCSharpDiagnosticAsync(
                testCode,
                this.GetSettings(),
                DiagnosticResult.EmptyDiagnosticResults,
                CancellationToken.None).ConfigureAwait(false);
        }
    }
}
