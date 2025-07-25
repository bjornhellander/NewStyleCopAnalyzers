﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Lightup
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal readonly partial struct DeclarationPatternSyntaxWrapper : ISyntaxWrapper<CSharpSyntaxNode>
    {
        internal const string WrappedTypeName = "Microsoft.CodeAnalysis.CSharp.Syntax.DeclarationPatternSyntax";
        private static readonly Type WrappedType;

        private static readonly Func<CSharpSyntaxNode, TypeSyntax> TypeAccessor;
        private static readonly Func<CSharpSyntaxNode, CSharpSyntaxNode> DesignationAccessor;
        private static readonly Func<CSharpSyntaxNode, TypeSyntax, CSharpSyntaxNode> WithTypeAccessor;
        private static readonly Func<CSharpSyntaxNode, CSharpSyntaxNode, CSharpSyntaxNode> WithDesignationAccessor;

        private readonly CSharpSyntaxNode node;

        static DeclarationPatternSyntaxWrapper()
        {
            WrappedType = SyntaxWrapperHelper.GetWrappedType(typeof(DeclarationPatternSyntaxWrapper));
            TypeAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<CSharpSyntaxNode, TypeSyntax>(WrappedType, nameof(Type));
            DesignationAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<CSharpSyntaxNode, CSharpSyntaxNode>(WrappedType, nameof(Designation));
            WithTypeAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<CSharpSyntaxNode, TypeSyntax>(WrappedType, nameof(Type));
            WithDesignationAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<CSharpSyntaxNode, CSharpSyntaxNode>(WrappedType, nameof(Designation));
        }

        private DeclarationPatternSyntaxWrapper(CSharpSyntaxNode node)
        {
            this.node = node;
        }

        public CSharpSyntaxNode SyntaxNode => this.node;

        public TypeSyntax Type
        {
            get
            {
                return TypeAccessor(this.SyntaxNode);
            }
        }

        public VariableDesignationSyntaxWrapper Designation
        {
            get
            {
                return (VariableDesignationSyntaxWrapper)DesignationAccessor(this.SyntaxNode);
            }
        }

        public static explicit operator DeclarationPatternSyntaxWrapper(PatternSyntaxWrapper node)
        {
            return (DeclarationPatternSyntaxWrapper)node.SyntaxNode;
        }

        public static explicit operator DeclarationPatternSyntaxWrapper(ExpressionOrPatternSyntaxWrapper node)
        {
            return (DeclarationPatternSyntaxWrapper)node.SyntaxNode;
        }

        public static explicit operator DeclarationPatternSyntaxWrapper(SyntaxNode node)
        {
            if (node == null)
            {
                return default;
            }

            if (!IsInstance(node))
            {
                throw new InvalidCastException($"Cannot cast '{node.GetType().FullName}' to '{WrappedTypeName}'");
            }

            return new DeclarationPatternSyntaxWrapper((CSharpSyntaxNode)node);
        }

        public static implicit operator PatternSyntaxWrapper(DeclarationPatternSyntaxWrapper wrapper)
        {
            return PatternSyntaxWrapper.FromUpcast(wrapper.node);
        }

        public static implicit operator ExpressionOrPatternSyntaxWrapper(DeclarationPatternSyntaxWrapper wrapper)
        {
            return ExpressionOrPatternSyntaxWrapper.FromUpcast(wrapper.node);
        }

        public static implicit operator CSharpSyntaxNode(DeclarationPatternSyntaxWrapper wrapper)
        {
            return wrapper.node;
        }

        public static bool IsInstance(SyntaxNode node)
        {
            return node != null && LightupHelpers.CanWrapNode(node, WrappedType);
        }

        public DeclarationPatternSyntaxWrapper WithType(TypeSyntax type)
        {
            return new DeclarationPatternSyntaxWrapper(WithTypeAccessor(this.SyntaxNode, type));
        }

        public DeclarationPatternSyntaxWrapper WithDesignation(VariableDesignationSyntaxWrapper designation)
        {
            return new DeclarationPatternSyntaxWrapper(WithDesignationAccessor(this.SyntaxNode, designation));
        }
    }
}
