﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Lightup
{
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal partial struct ImplicitStackAllocArrayCreationExpressionSyntaxWrapper : ISyntaxWrapper<ExpressionSyntax>
    {
        public ImplicitStackAllocArrayCreationExpressionSyntaxWrapper AddInitializerExpressions(params ExpressionSyntax[] items)
        {
            return new ImplicitStackAllocArrayCreationExpressionSyntaxWrapper(this.WithInitializer(this.Initializer.AddExpressions(items)));
        }
    }
}
