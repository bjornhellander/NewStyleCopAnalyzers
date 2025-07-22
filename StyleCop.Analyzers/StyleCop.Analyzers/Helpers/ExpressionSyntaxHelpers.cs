// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Helpers
{
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    internal static class ExpressionSyntaxHelpers
    {
        public static ExpressionSyntax WalkDownParentheses(this ExpressionSyntax expression)
        {
            var result = expression;
            while (result is ParenthesizedExpressionSyntax parenthesizedExpression)
            {
                result = parenthesizedExpression.Expression;
            }

            return result;
        }

        public static ExpressionSyntax WalkUpParentheses(this ExpressionSyntax expression)
        {
            var result = expression;
            while (result.Parent is ParenthesizedExpressionSyntax parenthesizedExpression)
            {
                result = parenthesizedExpression;
            }

            return result;
        }
    }
}
