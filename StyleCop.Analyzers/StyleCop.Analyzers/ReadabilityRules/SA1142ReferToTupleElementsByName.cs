// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.ReadabilityRules
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Lightup;
    using Microsoft.CodeAnalysis.Operations.Lightup;
    using StyleCop.Analyzers.Helpers;
    using StyleCop.Analyzers.Lightup;

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SA1142ReferToTupleElementsByName : DiagnosticAnalyzerBase
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1142ReferToTupleElementsByName"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1142";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(ReadabilityResources.SA1142Title), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(ReadabilityResources.SA1142MessageFormat), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(ReadabilityResources.SA1142Description), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));

        private static readonly Action<OperationAnalysisContext> FieldReferenceOperationAction = HandleFieldReferenceOperation;
        private static readonly Action<SyntaxNodeAnalysisContext> SimpleMemberAccessExpressionAction = HandleSimpleMemberAccessExpression;

        private static readonly DiagnosticDescriptor Descriptor = CreateDiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.ReadabilityRules, Description);

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        protected override void HandleCompilationStart(CompilationStartAnalysisContext context)
        {
            if (LightupHelpers.SupportsIOperation)
            {
                context.RegisterOperationAction(FieldReferenceOperationAction, OperationKindEx.FieldReference);
            }
            else
            {
                context.RegisterSyntaxNodeAction(SimpleMemberAccessExpressionAction, SyntaxKind.SimpleMemberAccessExpression);
            }
        }

        private static void HandleFieldReferenceOperation(OperationAnalysisContext context)
        {
            // TODO: Check this earlier?
            if (!context.SupportsTuples())
            {
                return;
            }

            var fieldReference = IFieldReferenceOperationWrapper.Wrap(context.Operation);

            if (CheckFieldName(fieldReference.Field))
            {
                var syntax = ((IOperation)fieldReference.Unwrap()).Syntax; // !!!
                var location = syntax is MemberAccessExpressionSyntax memberAccessExpression
                    ? memberAccessExpression.Name.GetLocation()
                    : syntax.GetLocation();
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, location));
            }
        }

        private static void HandleSimpleMemberAccessExpression(SyntaxNodeAnalysisContext context)
        {
            if (!context.SupportsTuples())
            {
                return;
            }

            var memberAccessExpression = (MemberAccessExpressionSyntax)context.Node;

            if (!(context.SemanticModel.GetSymbolInfo(memberAccessExpression).Symbol is IFieldSymbol fieldSymbol))
            {
                return;
            }

            if (CheckFieldName(fieldSymbol))
            {
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, memberAccessExpression.Name.GetLocation()));
            }
        }

        private static bool CheckFieldName(IFieldSymbol fieldSymbol)
        {
            if (!fieldSymbol.ContainingType.IsTupleType()) // !!!
            {
                return false;
            }

            // check if this already is a proper tuple field name
            if (!Equals(fieldSymbol.CorrespondingTupleField(), fieldSymbol)) // !!!
            {
                return false;
            }

            // check if there is a tuple field name declared.
            return fieldSymbol.ContainingType.GetMembers().OfType<IFieldSymbol>().Count(fs => Equals(fs.CorrespondingTupleField(), fieldSymbol)) > 1;
        }
    }
}
