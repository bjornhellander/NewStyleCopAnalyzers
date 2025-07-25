﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.Lightup
{
    using System;
    using Microsoft.CodeAnalysis.CSharp;
    using StyleCop.Analyzers.Lightup;
    using Xunit;

    public class ConstructorDeclarationSyntaxExtensionsTests
    {
        [Fact]
        public void TestWithExpressionBody()
        {
            var syntax = SyntaxFactory.ConstructorDeclaration(SyntaxFactory.Identifier("Anything"));

            // With default value is allowed
            var syntaxWithDefaultBody = ConstructorDeclarationSyntaxExtensions.WithExpressionBody(syntax, null);
            Assert.Null(BaseMethodDeclarationSyntaxExtensions.ExpressionBody(syntaxWithDefaultBody));

            // Non-default throws an exception
            var expressionBody = SyntaxFactory.ArrowExpressionClause(SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression));
            Assert.Throws<NotSupportedException>(() => ConstructorDeclarationSyntaxExtensions.WithExpressionBody(syntax, expressionBody));
        }
    }
}
