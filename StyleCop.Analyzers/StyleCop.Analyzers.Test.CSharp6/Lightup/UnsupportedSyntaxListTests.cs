// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.Lightup
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.CodeAnalysis;
    using StyleCop.Analyzers.Lightup;

    public class UnsupportedSyntaxListTests : SeparatedSyntaxListWrapperTestBase
    {
        internal override SeparatedSyntaxListWrapper<SyntaxNode> CreateList()
        {
            return SeparatedSyntaxListWrapper<SyntaxNode>.UnsupportedEmpty;
        }

        internal override bool TryCreateNonEmptyList([NotNullWhen(true)] out SeparatedSyntaxListWrapper<SyntaxNode>? list)
        {
            list = null;
            return false;
        }
    }
}
