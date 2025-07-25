﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;

    public class SA1402ForEnumUnitTests : SA1402ForNonBlockDeclarationUnitTestsBase
    {
        public override string Keyword => "enum";

        [Fact]
        public async Task TestOneElementAsync()
        {
            var testCode = @"enum Foo
{
    A, B, C
}
";

            await VerifyCSharpDiagnosticAsync(testCode, this.GetSettings(), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestTwoElementsAsync()
        {
            var testCode = @"enum Foo
{
    A, B, C
}
enum Bar
{
    D, E
}
";

            var fixedCode = new[]
            {
                ("/0/Test0.cs", @"enum Foo
{
    A, B, C
}
"),
                ("Bar.cs", @"enum Bar
{
    D, E
}
"),
            };

            DiagnosticResult expected = Diagnostic().WithLocation(5, 6);
            await VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestTwoElementsWithRuleDisabledAsync()
        {
            this.SettingsConfiguration = SA1402SettingsConfiguration.ConfigureAsNonTopLevelType;

            var testCode = @"enum Foo
{
    A, B, C
}
enum Bar
{
    D, E
}
";

            await VerifyCSharpDiagnosticAsync(testCode, this.GetSettings(), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestTwoElementsWithDefaultRuleConfigurationAsync()
        {
            this.SettingsConfiguration = SA1402SettingsConfiguration.KeepDefaultConfiguration;

            var testCode = @"enum Foo
{
    A, B, C
}
enum Bar
{
    D, E
}
";

            await VerifyCSharpDiagnosticAsync(testCode, this.GetSettings(), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestThreeElementsAsync()
        {
            var testCode = @"enum Foo
{
    A, B, C
}
enum Bar
{
    D, E
}
enum FooBar
{
    F, G, H
}
";

            var fixedCode = new[]
            {
                ("/0/Test0.cs", @"enum Foo
{
    A, B, C
}
"),
                ("Bar.cs", @"enum Bar
{
    D, E
}
"),
                ("FooBar.cs", @"enum FooBar
{
    F, G, H
}
"),
            };

            DiagnosticResult[] expected =
            {
                Diagnostic().WithLocation(5, 6),
                Diagnostic().WithLocation(9, 6),
            };

            await VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestPreferFilenameTypeAsync()
        {
            var testCode = @"enum Foo
{
    A, B, C
}
enum Test0
{
    D, E
}
";

            var fixedCode = new[]
            {
                ("/0/Test0.cs", @"enum Test0
{
    D, E
}
"),
                ("Foo.cs", @"enum Foo
{
    A, B, C
}
"),
            };

            DiagnosticResult expected = Diagnostic().WithLocation(1, 6);
            await VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
