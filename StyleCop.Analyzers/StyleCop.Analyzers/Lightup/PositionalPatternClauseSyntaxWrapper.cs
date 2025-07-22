// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Lightup
{
    using Microsoft.CodeAnalysis.CSharp;

    internal partial struct PositionalPatternClauseSyntaxWrapper : ISyntaxWrapper<CSharpSyntaxNode>
    {
        public PositionalPatternClauseSyntaxWrapper AddSubpatterns(params SubpatternSyntaxWrapper[] items)
        {
            return new PositionalPatternClauseSyntaxWrapper(this.WithSubpatterns(this.Subpatterns.AddRange(items)));
        }
    }
}
