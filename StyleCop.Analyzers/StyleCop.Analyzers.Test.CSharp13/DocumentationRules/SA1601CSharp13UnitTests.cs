// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp13.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<StyleCop.Analyzers.DocumentationRules.SA1601PartialElementsMustBeDocumented>;

    public partial class SA1601CSharp13UnitTests
    {
        [Fact]
        public async Task TestPartialPropertyWithDocumentationAsync()
        {
            var testCode = @"
/// <summary>
/// Some Documentation
/// </summary>
public partial class TypeName
{
    /// <summary>
    /// Some Documentation
    /// </summary>
    public partial int MemberName { get; set; }
}

/// <summary>
/// Some Documentation
/// </summary>
public partial class TypeName
{
    /// <summary>
    /// Some Documentation
    /// </summary>
    public partial int MemberName
    {
        get => 0;
        set { }
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestPartialPropertyWithoutDocumentationAsync()
        {
            var testCode = @"
/// <summary>
/// Some Documentation
/// </summary>
public partial class TypeName
{
    public partial int [|MemberName|] { get; set; }
}

/// <summary>
/// Some Documentation
/// </summary>
public partial class TypeName
{
    /// <summary>
    /// Some Documentation
    /// </summary>
    public partial int MemberName
    {
        get => 0;
        set { }
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestPartialIndexerWithDocumentationAsync()
        {
            var testCode = @"
/// <summary>
/// Some Documentation
/// </summary>
public partial class TypeName
{
    /// <summary>
    /// Some Documentation
    /// </summary>
    public partial int this[int index] { get; set; }
}

/// <summary>
/// Some Documentation
/// </summary>
public partial class TypeName
{
    /// <summary>
    /// Some Documentation
    /// </summary>
    public partial int this[int index]
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
/// <summary>
/// Some Documentation
/// </summary>
public partial class TypeName
{
    public partial int [|this|][int index] { get; set; }
}

/// <summary>
/// Some Documentation
/// </summary>
public partial class TypeName
{
    /// <summary>
    /// Some Documentation
    /// </summary>
    public partial int this[int index]
    {
        get => 0;
        set { }
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestNonPartialPropertyWithoutDocumentationAsync()
        {
            var testCode = @"
/// <summary>
/// Some Documentation
/// </summary>
public partial class TypeName
{
    public int MemberName { get; set; }
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestNonPartialIndexerWithoutDocumentationAsync()
        {
            var testCode = @"
/// <summary>
/// Some Documentation
/// </summary>
public partial class TypeName
{
    public int this[int index] => 42;
}";

            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
