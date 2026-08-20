// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<
        StyleCop.Analyzers.DocumentationRules.SA1606ElementDocumentationMustHaveSummaryText>;

    public partial class SA1606CSharp13UnitTests
    {
        [Fact]
        public async Task TestPartialPropertyWithoutDocumentationAsync()
        {
            var testCode = @"
public partial class ClassName
{
    /// <summary>
    ///
    /// </summary>
    public partial int Test { get; set; }
}

public partial class ClassName
{
    /// <summary>
    ///
    /// </summary>
    public partial int Test
    {
        get => 0;
        set { }
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestPartialIndexerWithoutDocumentationAsync()
        {
            var testCode = @"
public partial class ClassName
{
    /// <summary>
    ///
    /// </summary>
    public partial int this[int index] { get; set; }
}

public partial class ClassName
{
    /// <summary>
    ///
    /// </summary>
    public partial int this[int index]
    {
        get => 0;
        set { }
    }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
