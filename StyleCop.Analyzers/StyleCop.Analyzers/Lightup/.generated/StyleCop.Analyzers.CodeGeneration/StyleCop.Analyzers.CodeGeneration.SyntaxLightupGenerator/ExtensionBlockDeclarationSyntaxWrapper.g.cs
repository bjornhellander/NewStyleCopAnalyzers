﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Lightup
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal readonly partial struct ExtensionBlockDeclarationSyntaxWrapper : ISyntaxWrapper<TypeDeclarationSyntax>
    {
        internal const string WrappedTypeName = "Microsoft.CodeAnalysis.CSharp.Syntax.ExtensionBlockDeclarationSyntax";
        private static readonly Type WrappedType;

        private static readonly Func<TypeDeclarationSyntax, SyntaxList<AttributeListSyntax>> AttributeListsAccessor;
        private static readonly Func<TypeDeclarationSyntax, SyntaxTokenList> ModifiersAccessor;
        private static readonly Func<TypeDeclarationSyntax, SyntaxToken> KeywordAccessor;
        private static readonly Func<TypeDeclarationSyntax, TypeParameterListSyntax> TypeParameterListAccessor;
        private static readonly Func<TypeDeclarationSyntax, ParameterListSyntax> ParameterListAccessor;
        private static readonly Func<TypeDeclarationSyntax, SyntaxList<TypeParameterConstraintClauseSyntax>> ConstraintClausesAccessor;
        private static readonly Func<TypeDeclarationSyntax, SyntaxToken> OpenBraceTokenAccessor;
        private static readonly Func<TypeDeclarationSyntax, SyntaxList<MemberDeclarationSyntax>> MembersAccessor;
        private static readonly Func<TypeDeclarationSyntax, SyntaxToken> CloseBraceTokenAccessor;
        private static readonly Func<TypeDeclarationSyntax, SyntaxToken> SemicolonTokenAccessor;
        private static readonly Func<TypeDeclarationSyntax, SyntaxList<AttributeListSyntax>, TypeDeclarationSyntax> WithAttributeListsAccessor;
        private static readonly Func<TypeDeclarationSyntax, SyntaxTokenList, TypeDeclarationSyntax> WithModifiersAccessor;
        private static readonly Func<TypeDeclarationSyntax, SyntaxToken, TypeDeclarationSyntax> WithKeywordAccessor;
        private static readonly Func<TypeDeclarationSyntax, TypeParameterListSyntax, TypeDeclarationSyntax> WithTypeParameterListAccessor;
        private static readonly Func<TypeDeclarationSyntax, ParameterListSyntax, TypeDeclarationSyntax> WithParameterListAccessor;
        private static readonly Func<TypeDeclarationSyntax, SyntaxList<TypeParameterConstraintClauseSyntax>, TypeDeclarationSyntax> WithConstraintClausesAccessor;
        private static readonly Func<TypeDeclarationSyntax, SyntaxToken, TypeDeclarationSyntax> WithOpenBraceTokenAccessor;
        private static readonly Func<TypeDeclarationSyntax, SyntaxList<MemberDeclarationSyntax>, TypeDeclarationSyntax> WithMembersAccessor;
        private static readonly Func<TypeDeclarationSyntax, SyntaxToken, TypeDeclarationSyntax> WithCloseBraceTokenAccessor;
        private static readonly Func<TypeDeclarationSyntax, SyntaxToken, TypeDeclarationSyntax> WithSemicolonTokenAccessor;

        private readonly TypeDeclarationSyntax node;

        static ExtensionBlockDeclarationSyntaxWrapper()
        {
            WrappedType = SyntaxWrapperHelper.GetWrappedType(typeof(ExtensionBlockDeclarationSyntaxWrapper));
            AttributeListsAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<TypeDeclarationSyntax, SyntaxList<AttributeListSyntax>>(WrappedType, nameof(AttributeLists));
            ModifiersAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<TypeDeclarationSyntax, SyntaxTokenList>(WrappedType, nameof(Modifiers));
            KeywordAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<TypeDeclarationSyntax, SyntaxToken>(WrappedType, nameof(Keyword));
            TypeParameterListAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<TypeDeclarationSyntax, TypeParameterListSyntax>(WrappedType, nameof(TypeParameterList));
            ParameterListAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<TypeDeclarationSyntax, ParameterListSyntax>(WrappedType, nameof(ParameterList));
            ConstraintClausesAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<TypeDeclarationSyntax, SyntaxList<TypeParameterConstraintClauseSyntax>>(WrappedType, nameof(ConstraintClauses));
            OpenBraceTokenAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<TypeDeclarationSyntax, SyntaxToken>(WrappedType, nameof(OpenBraceToken));
            MembersAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<TypeDeclarationSyntax, SyntaxList<MemberDeclarationSyntax>>(WrappedType, nameof(Members));
            CloseBraceTokenAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<TypeDeclarationSyntax, SyntaxToken>(WrappedType, nameof(CloseBraceToken));
            SemicolonTokenAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<TypeDeclarationSyntax, SyntaxToken>(WrappedType, nameof(SemicolonToken));
            WithAttributeListsAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<TypeDeclarationSyntax, SyntaxList<AttributeListSyntax>>(WrappedType, nameof(AttributeLists));
            WithModifiersAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<TypeDeclarationSyntax, SyntaxTokenList>(WrappedType, nameof(Modifiers));
            WithKeywordAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<TypeDeclarationSyntax, SyntaxToken>(WrappedType, nameof(Keyword));
            WithTypeParameterListAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<TypeDeclarationSyntax, TypeParameterListSyntax>(WrappedType, nameof(TypeParameterList));
            WithParameterListAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<TypeDeclarationSyntax, ParameterListSyntax>(WrappedType, nameof(ParameterList));
            WithConstraintClausesAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<TypeDeclarationSyntax, SyntaxList<TypeParameterConstraintClauseSyntax>>(WrappedType, nameof(ConstraintClauses));
            WithOpenBraceTokenAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<TypeDeclarationSyntax, SyntaxToken>(WrappedType, nameof(OpenBraceToken));
            WithMembersAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<TypeDeclarationSyntax, SyntaxList<MemberDeclarationSyntax>>(WrappedType, nameof(Members));
            WithCloseBraceTokenAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<TypeDeclarationSyntax, SyntaxToken>(WrappedType, nameof(CloseBraceToken));
            WithSemicolonTokenAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<TypeDeclarationSyntax, SyntaxToken>(WrappedType, nameof(SemicolonToken));
        }

        private ExtensionBlockDeclarationSyntaxWrapper(TypeDeclarationSyntax node)
        {
            this.node = node;
        }

        public TypeDeclarationSyntax SyntaxNode => this.node;

        public SyntaxList<AttributeListSyntax> AttributeLists
        {
            get
            {
                return AttributeListsAccessor(this.SyntaxNode);
            }
        }

        public SyntaxTokenList Modifiers
        {
            get
            {
                return ModifiersAccessor(this.SyntaxNode);
            }
        }

        public SyntaxToken Keyword
        {
            get
            {
                return KeywordAccessor(this.SyntaxNode);
            }
        }

        public TypeParameterListSyntax TypeParameterList
        {
            get
            {
                return TypeParameterListAccessor(this.SyntaxNode);
            }
        }

        public ParameterListSyntax ParameterList
        {
            get
            {
                return ParameterListAccessor(this.SyntaxNode);
            }
        }

        public SyntaxList<TypeParameterConstraintClauseSyntax> ConstraintClauses
        {
            get
            {
                return ConstraintClausesAccessor(this.SyntaxNode);
            }
        }

        public SyntaxToken OpenBraceToken
        {
            get
            {
                return OpenBraceTokenAccessor(this.SyntaxNode);
            }
        }

        public SyntaxList<MemberDeclarationSyntax> Members
        {
            get
            {
                return MembersAccessor(this.SyntaxNode);
            }
        }

        public SyntaxToken CloseBraceToken
        {
            get
            {
                return CloseBraceTokenAccessor(this.SyntaxNode);
            }
        }

        public SyntaxToken SemicolonToken
        {
            get
            {
                return SemicolonTokenAccessor(this.SyntaxNode);
            }
        }

        public static explicit operator ExtensionBlockDeclarationSyntaxWrapper(SyntaxNode node)
        {
            if (node == null)
            {
                return default;
            }

            if (!IsInstance(node))
            {
                throw new InvalidCastException($"Cannot cast '{node.GetType().FullName}' to '{WrappedTypeName}'");
            }

            return new ExtensionBlockDeclarationSyntaxWrapper((TypeDeclarationSyntax)node);
        }

        public static implicit operator TypeDeclarationSyntax(ExtensionBlockDeclarationSyntaxWrapper wrapper)
        {
            return wrapper.node;
        }

        public static bool IsInstance(SyntaxNode node)
        {
            return node != null && LightupHelpers.CanWrapNode(node, WrappedType);
        }

        public ExtensionBlockDeclarationSyntaxWrapper WithAttributeLists(SyntaxList<AttributeListSyntax> attributeLists)
        {
            return new ExtensionBlockDeclarationSyntaxWrapper(WithAttributeListsAccessor(this.SyntaxNode, attributeLists));
        }

        public ExtensionBlockDeclarationSyntaxWrapper WithModifiers(SyntaxTokenList modifiers)
        {
            return new ExtensionBlockDeclarationSyntaxWrapper(WithModifiersAccessor(this.SyntaxNode, modifiers));
        }

        public ExtensionBlockDeclarationSyntaxWrapper WithKeyword(SyntaxToken keyword)
        {
            return new ExtensionBlockDeclarationSyntaxWrapper(WithKeywordAccessor(this.SyntaxNode, keyword));
        }

        public ExtensionBlockDeclarationSyntaxWrapper WithTypeParameterList(TypeParameterListSyntax typeParameterList)
        {
            return new ExtensionBlockDeclarationSyntaxWrapper(WithTypeParameterListAccessor(this.SyntaxNode, typeParameterList));
        }

        public ExtensionBlockDeclarationSyntaxWrapper WithParameterList(ParameterListSyntax parameterList)
        {
            return new ExtensionBlockDeclarationSyntaxWrapper(WithParameterListAccessor(this.SyntaxNode, parameterList));
        }

        public ExtensionBlockDeclarationSyntaxWrapper WithConstraintClauses(SyntaxList<TypeParameterConstraintClauseSyntax> constraintClauses)
        {
            return new ExtensionBlockDeclarationSyntaxWrapper(WithConstraintClausesAccessor(this.SyntaxNode, constraintClauses));
        }

        public ExtensionBlockDeclarationSyntaxWrapper WithOpenBraceToken(SyntaxToken openBraceToken)
        {
            return new ExtensionBlockDeclarationSyntaxWrapper(WithOpenBraceTokenAccessor(this.SyntaxNode, openBraceToken));
        }

        public ExtensionBlockDeclarationSyntaxWrapper WithMembers(SyntaxList<MemberDeclarationSyntax> members)
        {
            return new ExtensionBlockDeclarationSyntaxWrapper(WithMembersAccessor(this.SyntaxNode, members));
        }

        public ExtensionBlockDeclarationSyntaxWrapper WithCloseBraceToken(SyntaxToken closeBraceToken)
        {
            return new ExtensionBlockDeclarationSyntaxWrapper(WithCloseBraceTokenAccessor(this.SyntaxNode, closeBraceToken));
        }

        public ExtensionBlockDeclarationSyntaxWrapper WithSemicolonToken(SyntaxToken semicolonToken)
        {
            return new ExtensionBlockDeclarationSyntaxWrapper(WithSemicolonTokenAccessor(this.SyntaxNode, semicolonToken));
        }
    }
}
