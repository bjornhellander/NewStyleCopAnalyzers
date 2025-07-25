﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.CSharp10.ReadabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.Test.CSharp9.ReadabilityRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.ReadabilityRules.SA1121UseBuiltInTypeAlias,
        StyleCop.Analyzers.ReadabilityRules.SA1121CodeFixProvider>;

    public partial class SA1121CSharp10UnitTests : SA1121CSharp9UnitTests
    {
        [Fact]
        public async Task TestUsingNameChangeInFileScopedNamespaceAsync()
        {
            string oldSource = @"namespace Foo;

using MyInt = System.UInt32;
class Bar
{
{|#0:MyInt|} value = 3;
}
";
            string newSource = @"namespace Foo;

using MyInt = System.UInt32;
class Bar
{
uint value = 3;
}
";

            await new CSharpTest
            {
                TestCode = oldSource,
                ExpectedDiagnostics = { Diagnostic().WithLocation(0) },
                FixedCode = newSource,
            }.RunAsync(CancellationToken.None).ConfigureAwait(false);
        }
    }
}
