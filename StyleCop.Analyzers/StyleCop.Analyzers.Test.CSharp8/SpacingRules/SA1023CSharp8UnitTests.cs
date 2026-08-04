// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.SpacingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.SpacingRules.SA1023DereferenceAndAccessOfSymbolsMustBeSpacedCorrectly,
        StyleCop.Analyzers.SpacingRules.TokenSpacingCodeFixProvider>;

    public partial class SA1023CSharp8UnitTests
    {
        [Fact]
        public async Task TestGenericTypePointerAsync()
        {
            const string testCode = @"using System;

public struct Foo<T>
{
    internal unsafe Foo<T> [|*|] Next1;
    internal unsafe Foo<T>[|*|]Next2;
    internal unsafe Foo<T> [|[|*|]|]Next3;
}";
            const string fixedCode = @"using System;

public struct Foo<T>
{
    internal unsafe Foo<T>* Next1;
    internal unsafe Foo<T>* Next2;
    internal unsafe Foo<T>* Next3;
}";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
