// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Lightup
{
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal partial struct TupleExpressionSyntaxWrapper : ISyntaxWrapper<ExpressionSyntax>
    {
        public TupleExpressionSyntaxWrapper AddArguments(params ArgumentSyntax[] items)
        {
            return new TupleExpressionSyntaxWrapper(this.WithArguments(this.Arguments.AddRange(items)));
        }
    }
}
