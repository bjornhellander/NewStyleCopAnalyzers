﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.MaintainabilityRules
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Helpers;
    using StyleCop.Analyzers.Lightup;

    /// <summary>
    /// A C# statement contains parenthesis which are unnecessary and should be removed.
    /// </summary>
    /// <remarks>
    /// <para>It is possible in C# to insert parenthesis around virtually any type of expression, statement, or clause,
    /// and in many situations use of parenthesis can greatly improve the readability of the code. However, excessive
    /// use of parenthesis can have the opposite effect, making it more difficult to read and maintain the code.</para>
    ///
    /// <para>A violation of this rule occurs when parenthesis are used in situations where they provide no practical
    /// value. Typically, this happens anytime the parenthesis surround an expression which does not strictly require
    /// the use of parenthesis, and the parenthesis expression is located at the root of a statement. For example, the
    /// following lines of code all contain unnecessary parenthesis which will result in violations of this rule:</para>
    ///
    /// <code language="csharp">
    /// int x = (5 + b);
    /// string y = (this.Method()).ToString();
    /// return (x.Value);
    /// </code>
    ///
    /// <para>In each of these statements, the extra parenthesis can be removed without sacrificing the readability of
    /// the code:</para>
    ///
    /// <code language="csharp">
    /// int x = 5 + b;
    /// string y = this.Method().ToString();
    /// return x.Value;
    /// </code>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SA1119StatementMustNotUseUnnecessaryParenthesis : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1119StatementMustNotUseUnnecessaryParenthesis"/>
        /// analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1119";

        /// <summary>
        /// The ID for the helper diagnostic used to mark the parentheses tokens surrounding the expression with
        /// <see cref="WellKnownDiagnosticTags.Unnecessary"/>.
        /// </summary>
        public const string ParenthesesDiagnosticId = DiagnosticId + "_p";
        private const string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1119.md";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(MaintainabilityResources.SA1119Title), MaintainabilityResources.ResourceManager, typeof(MaintainabilityResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(MaintainabilityResources.SA1119MessageFormat), MaintainabilityResources.ResourceManager, typeof(MaintainabilityResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(MaintainabilityResources.SA1119Description), MaintainabilityResources.ResourceManager, typeof(MaintainabilityResources));

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.MaintainabilityRules, DiagnosticSeverity.Warning, AnalyzerConstants.EnabledByDefault, Description, HelpLink);

        private static readonly DiagnosticDescriptor ParenthesisDescriptor =
#pragma warning disable RS2000 // Add analyzer diagnostic IDs to analyzer release.
            new DiagnosticDescriptor(ParenthesesDiagnosticId, Title, MessageFormat, AnalyzerCategory.MaintainabilityRules, DiagnosticSeverity.Hidden, AnalyzerConstants.EnabledByDefault, Description, HelpLink, customTags: new[] { WellKnownDiagnosticTags.Unnecessary, WellKnownDiagnosticTags.NotConfigurable });
#pragma warning restore RS2000 // Add analyzer diagnostic IDs to analyzer release.

        private static readonly Action<CompilationStartAnalysisContext> CompilationStartAction = HandleCompilationStart;
        private static readonly Action<SyntaxNodeAnalysisContext> ParenthesizedExpressionAction = HandleParenthesizedExpression;

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(
                Descriptor,
                ParenthesisDescriptor);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterCompilationStartAction(CompilationStartAction);
        }

        private static void HandleCompilationStart(CompilationStartAnalysisContext context)
        {
            // Only register the syntax node action if the diagnostic is enabled. This is important because
            // otherwise the diagnostic for fading out the parenthesis is still active, even if the main diagnostic
            // is disabled
            if (!context.IsAnalyzerSuppressed(Descriptor))
            {
                context.RegisterSyntaxNodeAction(ParenthesizedExpressionAction, SyntaxKind.ParenthesizedExpression);
            }
        }

        private static void HandleParenthesizedExpression(SyntaxNodeAnalysisContext context)
        {
            var node = (ParenthesizedExpressionSyntax)context.Node;

            if (node.Expression != null)
            {
                if (!(node.Expression is BinaryExpressionSyntax)
                    && !(node.Expression is AssignmentExpressionSyntax)
                    && !(node.Expression is PrefixUnaryExpressionSyntax)
                    && !(node.Expression is PostfixUnaryExpressionSyntax)
                    && !node.Expression.IsKind(SyntaxKind.CastExpression)
                    && !node.Expression.IsKind(SyntaxKind.ConditionalExpression)
                    && !node.Expression.IsKind(SyntaxKind.IsExpression)
                    && !node.Expression.IsKind(SyntaxKindEx.IsPatternExpression)
                    && !node.Expression.IsKind(SyntaxKind.SimpleLambdaExpression)
                    && !node.Expression.IsKind(SyntaxKind.ParenthesizedLambdaExpression)
                    && !node.Expression.IsKind(SyntaxKind.ArrayCreationExpression)
                    && !node.Expression.IsKind(SyntaxKind.CoalesceExpression)
                    && !node.Expression.IsKind(SyntaxKind.QueryExpression)
                    && !node.Expression.IsKind(SyntaxKind.AwaitExpression)
                    && !node.Expression.IsKind(SyntaxKindEx.RangeExpression)
                    && !node.IsKind(SyntaxKind.ConstructorDeclaration))
                {
                    if (node.Expression.IsKind(SyntaxKind.ConditionalAccessExpression)
                        && (node.Parent is ElementAccessExpressionSyntax
                        || node.Parent is MemberAccessExpressionSyntax
                        || node.Parent is ConditionalAccessExpressionSyntax))
                    {
                        return;
                    }

                    if (IsSwitchOrWithExpressionWithRequiredParentheses(node))
                    {
                        return;
                    }

                    if ((node.Expression.IsKind(SyntaxKind.StackAllocArrayCreationExpression)
                        || node.Expression.IsKind(SyntaxKindEx.ImplicitStackAllocArrayCreationExpression))
                        && node.Parent.IsKind(SyntaxKind.EqualsValueClause))
                    {
                        return;
                    }

                    ReportDiagnostic(context, node);
                }
                else
                {
                    if (node.Parent is InterpolationSyntax
                        && IsConditionalAccessInInterpolation(node.Expression))
                    {
                        // Parenthesis can't be removed here
                        return;
                    }

                    if (node.Parent is AssignmentExpressionSyntax assignmentExpression
                        && node.Expression.IsKind(SyntaxKind.ConditionalExpression)
                        && assignmentExpression.Left == node)
                    {
                        // NOTE: This is only valid syntax if the conditional expression is a ref expression
                        // Parenthesis can't be removed here
                        return;
                    }

                    if (!(node.Parent is ExpressionSyntax)
                        || node.Parent is CheckedExpressionSyntax
                        || node.Parent is MemberAccessExpressionSyntax)
                    {
                        if (node.Parent is MemberAccessExpressionSyntax memberAccess)
                        {
                            if (memberAccess.Expression != node)
                            {
                                ReportDiagnostic(context, node);
                            }
                        }
                        else
                        {
                            ReportDiagnostic(context, node);
                        }
                    }
                    else
                    {
                        if (node.Parent is EqualsValueClauseSyntax equalsValue
                            && equalsValue.Value == node)
                        {
                            ReportDiagnostic(context, node);
                        }
                        else
                        {
                            if (node.Parent is AssignmentExpressionSyntax)
                            {
                                ReportDiagnostic(context, node);
                            }
                        }
                    }
                }
            }
        }

        private static bool IsConditionalAccessInInterpolation(ExpressionSyntax node)
        {
            Queue<ExpressionSyntax> expressionToCheck = new Queue<ExpressionSyntax>();
            expressionToCheck.Enqueue(node);

            ExpressionSyntax currentNode;
            while (expressionToCheck.Count > 0)
            {
                currentNode = expressionToCheck.Dequeue();

                if (currentNode.IsKind(SyntaxKind.ConditionalExpression))
                {
                    return true;
                }
                else if (currentNode is AssignmentExpressionSyntax)
                {
                    // We have to use parenthesis if the conditional access is in an interpolation inside an assignment.
                    var assignment = currentNode as AssignmentExpressionSyntax;
                    expressionToCheck.Enqueue(assignment.Left);
                    expressionToCheck.Enqueue(assignment.Right);
                }
            }

            return false;
        }

        private static bool IsSwitchOrWithExpressionWithRequiredParentheses(ParenthesizedExpressionSyntax node)
        {
            if (!node.Expression.IsKind(SyntaxKindEx.SwitchExpression)
                && !node.Expression.IsKind(SyntaxKindEx.WithExpression))
            {
                return false;
            }

            var outerExpression = node.WalkUpParentheses();
            return outerExpression.Parent switch
            {
                AwaitExpressionSyntax awaitExpression => awaitExpression.Expression == outerExpression,
                CastExpressionSyntax castExpression => castExpression.Expression == outerExpression,
                MemberAccessExpressionSyntax memberAccessExpression => memberAccessExpression.Expression == outerExpression,
                ConditionalAccessExpressionSyntax conditionalAccessExpression => conditionalAccessExpression.Expression == outerExpression,
                ElementAccessExpressionSyntax elementAccessExpression => elementAccessExpression.Expression == outerExpression,
                InvocationExpressionSyntax invocationExpression => invocationExpression.Expression == outerExpression,
                _ => false,
            };
        }

        private static void ReportDiagnostic(SyntaxNodeAnalysisContext context, ParenthesizedExpressionSyntax node)
        {
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, node.GetLocation()));
            context.ReportDiagnostic(Diagnostic.Create(ParenthesisDescriptor, node.OpenParenToken.GetLocation()));
            context.ReportDiagnostic(Diagnostic.Create(ParenthesisDescriptor, node.CloseParenToken.GetLocation()));
        }
    }
}
