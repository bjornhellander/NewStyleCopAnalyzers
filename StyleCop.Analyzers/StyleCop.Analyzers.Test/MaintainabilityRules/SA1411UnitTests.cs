﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.MaintainabilityRules.SA1411AttributeConstructorMustNotUseUnnecessaryParenthesis,
        StyleCop.Analyzers.MaintainabilityRules.SA1410SA1411CodeFixProvider>;

    public class SA1411UnitTests
    {
        [Fact]
        public async Task TestMissingParenthesisAsync()
        {
            var testCode = @"public class Foo
{
    [System.Obsolete]
    public void Bar()
    {
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestNonEmptyParameterListAsync()
        {
            var testCode = @"public class Foo
{
    [System.Obsolete(""bar"")]
    public void Bar()
    {
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestNonEmptyParameterListNamedArgumentAsync()
        {
            var testCode = @"
using System.Runtime.CompilerServices;
public class Foo
{
    [MethodImpl(MethodCodeType = MethodCodeType.IL)]
    public void Bar()
    {
    }
}";
            await VerifyCSharpDiagnosticAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestEmptyParameterListAsync()
        {
            var testCode = @"public class Foo
{
    [System.Obsolete()]
    public void Bar()
    {
    }
}";
            DiagnosticResult expected = Diagnostic().WithLocation(3, 21);

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestEmptyParameterListMultipleAttributesAsync()
        {
            var testCode = @"public class Foo
{
    [System.Obsolete(), System.Runtime.CompilerServices.MethodImpl()]
    public void Bar()
    {
    }
}";
            DiagnosticResult[] expected =
                {
                    Diagnostic().WithLocation(3, 21),
                    Diagnostic().WithLocation(3, 67),
                };

            await VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixAsync()
        {
            var oldSource = @"public class Foo
{
    [System.Obsolete()]
    public void Bar()
    {
    }
}";

            var newSource = @"public class Foo
{
    [System.Obsolete]
    public void Bar()
    {
    }
}";

            var expected = Diagnostic().WithLocation(3, 21);
            await VerifyCSharpFixAsync(oldSource, expected, newSource, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveExteriorTriviaAsync()
        {
            var oldSource = @"public class Foo
{
    [System.Obsolete/*Foo*/(/*Bar*/)/*Foo*/]
    public void Bar()
    {
    }
}";

            var newSource = @"public class Foo
{
    [System.Obsolete/*Foo*//*Bar*//*Foo*/]
    public void Bar()
    {
    }
}";

            var expected = Diagnostic().WithLocation(3, 28);
            await VerifyCSharpFixAsync(oldSource, expected, newSource, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixMultipleAttributesAsync()
        {
            var oldSource = @"public class Foo
{
    [System.Obsolete(), System.Runtime.CompilerServices.MethodImpl()]
    public void Bar()
    {
    }
}";

            var newSource = @"public class Foo
{
    [System.Obsolete, System.Runtime.CompilerServices.MethodImpl]
    public void Bar()
    {
    }
}";

            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(3, 21),
                Diagnostic().WithLocation(3, 67),
            };

            await VerifyCSharpFixAsync(oldSource, expected, newSource, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
