﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.CSharp7.Lightup
{
    using System;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using StyleCop.Analyzers.Helpers;
    using StyleCop.Analyzers.Lightup;
    using Xunit;

    public class CommonForEachStatementSyntaxWrapperTests
    {
        [Fact]
        public void TestNull()
        {
            var syntaxNode = default(SyntaxNode);
            var wrapper = (CommonForEachStatementSyntaxWrapper)syntaxNode;
            Assert.Null(wrapper.SyntaxNode);
            Assert.Throws<NullReferenceException>(() => wrapper.AwaitKeyword);
            Assert.Throws<NullReferenceException>(() => wrapper.ForEachKeyword);
            Assert.Throws<NullReferenceException>(() => wrapper.OpenParenToken);
            Assert.Throws<NullReferenceException>(() => wrapper.InKeyword);
            Assert.Throws<NullReferenceException>(() => wrapper.Expression);
            Assert.Throws<NullReferenceException>(() => wrapper.CloseParenToken);
            Assert.Throws<NullReferenceException>(() => wrapper.Statement);
            Assert.Throws<NullReferenceException>(() => wrapper.WithAwaitKeyword(SyntaxFactory.Token(SyntaxKind.AwaitKeyword)));
            Assert.Throws<NullReferenceException>(() => wrapper.WithForEachKeyword(SyntaxFactory.Token(SyntaxKind.ForEachKeyword)));
            Assert.Throws<NullReferenceException>(() => wrapper.WithOpenParenToken(SyntaxFactory.Token(SyntaxKind.OpenParenToken)));
            Assert.Throws<NullReferenceException>(() => wrapper.WithInKeyword(SyntaxFactory.Token(SyntaxKind.InKeyword)));
            Assert.Throws<NullReferenceException>(() => wrapper.WithExpression(SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)));
            Assert.Throws<NullReferenceException>(() => wrapper.WithCloseParenToken(SyntaxFactory.Token(SyntaxKind.CloseParenToken)));
            Assert.Throws<NullReferenceException>(() => wrapper.WithStatement(SyntaxFactory.EmptyStatement()));
        }

        [Fact]
        public void TestWrapperIdentity()
        {
            var syntaxNode = this.CreateForEachStatement();
            Assert.True(syntaxNode.IsKind(SyntaxKind.ForEachStatement));

            var wrapper = (CommonForEachStatementSyntaxWrapper)syntaxNode;
            Assert.Same(syntaxNode, wrapper.SyntaxNode);
        }

        [Fact]
        public void TestAwaitKeyword()
        {
            var syntaxNode = this.CreateForEachStatement();
            var wrapper = (CommonForEachStatementSyntaxWrapper)syntaxNode;
            Assert.Equal(default, wrapper.AwaitKeyword);

            Assert.Throws<NotSupportedException>(() => wrapper.WithAwaitKeyword(SyntaxFactory.Token(SyntaxKind.AwaitKeyword)));
        }

        [Fact]
        public void TestForEachKeyword()
        {
            var syntaxNode = this.CreateForEachStatement();
            var wrapper = (CommonForEachStatementSyntaxWrapper)syntaxNode;
            Assert.Equal(syntaxNode.ForEachKeyword, wrapper.ForEachKeyword);

            Assert.Throws<NotSupportedException>(() => wrapper.WithForEachKeyword(SpacingExtensions.WithoutTrivia(SyntaxFactory.Token(SyntaxKind.ForEachKeyword))));
        }

        [Fact]
        public void TestOpenParenToken()
        {
            var syntaxNode = this.CreateForEachStatement();
            var wrapper = (CommonForEachStatementSyntaxWrapper)syntaxNode;
            Assert.Equal(syntaxNode.OpenParenToken, wrapper.OpenParenToken);

            Assert.Throws<NotSupportedException>(() => wrapper.WithOpenParenToken(SpacingExtensions.WithoutTrivia(SyntaxFactory.Token(SyntaxKind.OpenParenToken))));
        }

        [Fact]
        public void TestInKeyword()
        {
            var syntaxNode = this.CreateForEachStatement();
            var wrapper = (CommonForEachStatementSyntaxWrapper)syntaxNode;
            Assert.Equal(syntaxNode.InKeyword, wrapper.InKeyword);

            Assert.Throws<NotSupportedException>(() => wrapper.WithInKeyword(SpacingExtensions.WithoutTrivia(SyntaxFactory.Token(SyntaxKind.InKeyword))));
        }

        [Fact]
        public void TestExpression()
        {
            var syntaxNode = this.CreateForEachStatement();
            var wrapper = (CommonForEachStatementSyntaxWrapper)syntaxNode;
            Assert.Same(syntaxNode.Expression, wrapper.Expression);

            Assert.Throws<NotSupportedException>(() => wrapper.WithExpression(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(0))));
        }

        [Fact]
        public void TestCloseParenToken()
        {
            var syntaxNode = this.CreateForEachStatement();
            var wrapper = (CommonForEachStatementSyntaxWrapper)syntaxNode;
            Assert.Equal(syntaxNode.CloseParenToken, wrapper.CloseParenToken);

            Assert.Throws<NotSupportedException>(() => wrapper.WithCloseParenToken(SpacingExtensions.WithoutTrivia(SyntaxFactory.Token(SyntaxKind.CloseParenToken))));
        }

        [Fact]
        public void TestStatement()
        {
            var syntaxNode = this.CreateForEachStatement();
            var wrapper = (CommonForEachStatementSyntaxWrapper)syntaxNode;
            Assert.Same(syntaxNode.Statement, wrapper.Statement);

            Assert.Throws<NotSupportedException>(() => wrapper.WithStatement(SyntaxFactory.Block()));
        }

        [Fact]
        public void TestIsInstance()
        {
            Assert.False(CommonForEachStatementSyntaxWrapper.IsInstance(null));
            Assert.False(CommonForEachStatementSyntaxWrapper.IsInstance(SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)));

            var syntaxNode = this.CreateForEachStatement();
            Assert.True(CommonForEachStatementSyntaxWrapper.IsInstance(syntaxNode));
        }

        [Fact]
        public void TestConversionsNull()
        {
            var syntaxNode = default(SyntaxNode);
            var wrapper = (CommonForEachStatementSyntaxWrapper)syntaxNode;

            StatementSyntax syntax = wrapper;
            Assert.Null(syntax);
        }

        [Fact]
        public void TestConversions()
        {
            var syntaxNode = this.CreateForEachStatement();
            var wrapper = (CommonForEachStatementSyntaxWrapper)syntaxNode;

            StatementSyntax syntax = wrapper;
            Assert.Same(syntaxNode, syntax);
        }

        [Fact]
        public void TestInvalidConversion()
        {
            var syntaxNode = SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression);
            Assert.Throws<InvalidCastException>(() => (CommonForEachStatementSyntaxWrapper)syntaxNode);
        }

        private ForEachStatementSyntax CreateForEachStatement()
        {
            return SyntaxFactory.ForEachStatement(
                SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword)),
                "x",
                SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression),
                SyntaxFactory.EmptyStatement());
        }
    }
}
