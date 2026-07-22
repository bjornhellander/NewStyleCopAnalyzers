// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp15.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1402FileMayOnlyContainASingleType>;

    public class SA1402CSharp15UnitTests
    {
        [Fact]
        public async Task TestFileWithClassAndUnionAsync()
        {
            var testCode = @"
public class TestClass
{
}

public union [|TestUnion|](string, int)
{
}
";

            var settings = @"
{
  ""settings"": {
    ""maintainabilityRules"": {
      ""topLevelTypes"": [""class"", ""struct""]
    }
  }
}";

            await VerifyCSharpDiagnosticAsync(null, testCode, settings, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestFileWithOnlyUnionAsync()
        {
            var testCode = @"
public union TestUnion(string, int)
{
}
";

            var settings = @"
{
  ""settings"": {
    ""maintainabilityRules"": {
      ""topLevelTypes"": [""class"", ""struct""]
    }
  }
}";

            await VerifyCSharpDiagnosticAsync(null, testCode, settings, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
