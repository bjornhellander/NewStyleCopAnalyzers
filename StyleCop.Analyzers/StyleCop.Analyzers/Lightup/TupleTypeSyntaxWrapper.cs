﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Lightup
{
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal partial struct TupleTypeSyntaxWrapper : ISyntaxWrapper<TypeSyntax>
    {
        public TupleTypeSyntaxWrapper AddElements(params TupleElementSyntaxWrapper[] items)
        {
            return new TupleTypeSyntaxWrapper(this.WithElements(this.Elements.AddRange(items)));
        }
    }
}
