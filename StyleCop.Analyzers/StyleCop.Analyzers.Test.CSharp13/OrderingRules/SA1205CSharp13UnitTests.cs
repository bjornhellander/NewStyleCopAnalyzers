// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.OrderingRules.SA1205PartialElementsMustDeclareAccess>;

    public partial class SA1205CSharp13UnitTests
    {
        [Fact]
        public async Task TestPartialPropertyWithoutAccessModifierAsync()
        {
            var testCode = @"
public partial class ClassName
{
    partial int Test { get; set; }
}

public partial class ClassName
{
    partial int Test
    {
        get => 0;
        set { }
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
