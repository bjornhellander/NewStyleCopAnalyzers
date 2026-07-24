// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.Lightup
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using StyleCop.Analyzers.Lightup;

    public class AutoWrapSeparatedSyntaxListTests : SeparatedSyntaxListWrapperTestBase
    {
        internal override SeparatedSyntaxListWrapper<SyntaxNode> CreateList()
        {
            return new SeparatedSyntaxListWrapper<SyntaxNode>.AutoWrapSeparatedSyntaxList<SyntaxNode>(default);
        }

        internal override bool TryCreateNonEmptyList([NotNullWhen(true)] out SeparatedSyntaxListWrapper<SyntaxNode>? list)
        {
            list = new SeparatedSyntaxListWrapper<SyntaxNode>.AutoWrapSeparatedSyntaxList<SyntaxNode>(
                SyntaxFactory.SingletonSeparatedList<SyntaxNode>(SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)));
            return true;
        }
    }
}
