// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp9.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1137ElementsShouldHaveTheSameIndentation,
        StyleCop.Analyzers.ReadabilityRules.IndentationCodeFixProvider>;

    public partial class SA1137CSharp9UnitTests
    {
        [Fact]
        public async Task TestInitAccessorAttributeListAsync()
        {
            string testCode = @"
using System;

class TestClass
{
    int Property
    {
        [My]
[| |][My]
        init { }
    }
}

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
class MyAttribute : Attribute { }
";

            string fixedCode = @"
using System;

class TestClass
{
    int Property
    {
        [My]
        [My]
        init { }
    }
}

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
class MyAttribute : Attribute { }
";

            await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
