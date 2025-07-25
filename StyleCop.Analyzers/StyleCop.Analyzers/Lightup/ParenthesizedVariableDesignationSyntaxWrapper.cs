﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Lightup
{
    using Microsoft.CodeAnalysis.CSharp;

    internal partial struct ParenthesizedVariableDesignationSyntaxWrapper : ISyntaxWrapper<CSharpSyntaxNode>
    {
        public ParenthesizedVariableDesignationSyntaxWrapper AddVariables(params VariableDesignationSyntaxWrapper[] items)
        {
            return new ParenthesizedVariableDesignationSyntaxWrapper(this.WithVariables(this.Variables.AddRange(items)));
        }
    }
}
