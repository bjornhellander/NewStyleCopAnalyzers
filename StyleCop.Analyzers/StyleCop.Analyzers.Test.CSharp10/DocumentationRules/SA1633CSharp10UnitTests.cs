// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp10.DocumentationRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using StyleCop.Analyzers.DocumentationRules;
    using Xunit;

    public partial class SA1633CSharp10UnitTests
    {
        [Fact]
        public async Task TestHeaderNotFoundAfterMappedLineDirectiveAsync()
        {
            var testCode = @"#line (1,1)-(1,1) ""Test0.cs""
#line default
// <copyright file=""Test0.cs"" company=""FooCorp"">
//   Copyright (c) FooCorp. All rights reserved.
// </copyright>

public class TestClass
{
}
#line default";

            var expected = Diagnostic(FileHeaderAnalyzers.SA1633DescriptorMissing).WithLocation(1, 1);
            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestMissingHeaderAfterMappedLineDirectiveAsync()
        {
            var testCode = @"#line (1,1)-(1,1) ""Test0.cs""
#line default
public class TestClass
{
}
#line default";

            var expected = Diagnostic(FileHeaderAnalyzers.SA1633DescriptorMissing).WithLocation(1, 1);
            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None).ConfigureAwait(true);
        }
    }
}
