// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp14.LayoutRules
{
    using System;
    using Microsoft.CodeAnalysis.Testing;

    public partial class SA1504CSharp14UnitTests
    {
        protected override DiagnosticResult[] GetExpectedResultAccessorWithoutBody()
        {
            return Array.Empty<DiagnosticResult>();
        }
    }
}
