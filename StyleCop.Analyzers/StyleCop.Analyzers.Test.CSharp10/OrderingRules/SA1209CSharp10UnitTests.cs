// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.OrderingRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Testing;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopCodeFixVerifier<
        StyleCop.Analyzers.OrderingRules.SA1209UsingAliasDirectivesMustBePlacedAfterOtherUsingDirectives,
        StyleCop.Analyzers.OrderingRules.UsingCodeFixProvider>;

    public partial class SA1209CSharp10UnitTests
    {
        [Fact]
        public async Task TestWhenUsingAliasDirectivesAreNotPlacedCorrectlyInFileScopedNamespaceAsync()
        {
            var testCodeNamespace = @"namespace Test;

using System.Net;
using System.Threading;
[|using L = System.Linq;|]
using System.IO;
using P = System.Threading.Tasks;

class A
{
}
";
            var fixedTestCodeNamespace = @"namespace Test;

using System.IO;
using System.Net;
using System.Threading;
using L = System.Linq;
using P = System.Threading.Tasks;

class A
{
}
";

            await VerifyCSharpFixAsync(testCodeNamespace, DiagnosticResult.EmptyDiagnosticResults, fixedTestCodeNamespace, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
