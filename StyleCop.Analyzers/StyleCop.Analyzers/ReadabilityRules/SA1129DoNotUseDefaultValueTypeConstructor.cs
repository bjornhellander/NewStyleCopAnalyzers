﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.ReadabilityRules
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Lightup;

    /// <summary>
    /// A value type was constructed using the syntax <c>new T()</c>.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SA1129DoNotUseDefaultValueTypeConstructor : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1129DoNotUseDefaultValueTypeConstructor"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1129";
        private const string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1129.md";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(ReadabilityResources.SA1129Title), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(ReadabilityResources.SA1129MessageFormat), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(ReadabilityResources.SA1129Description), ReadabilityResources.ResourceManager, typeof(ReadabilityResources));

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.ReadabilityRules, DiagnosticSeverity.Warning, AnalyzerConstants.EnabledByDefault, Description, HelpLink);

        private static readonly Action<OperationAnalysisContext> ObjectCreationOperationAction = HandleObjectCreationOperation;
        private static readonly Action<OperationAnalysisContext> TypeParameterObjectCreationOperationAction = HandleTypeParameterObjectCreationOperation;
        private static readonly Action<SyntaxNodeAnalysisContext> ObjectCreationExpressionAction = HandleObjectCreationExpression;

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            if (LightupHelpers.SupportsIOperation)
            {
                context.RegisterOperationAction(ObjectCreationOperationAction, OperationKindEx.ObjectCreation);
                context.RegisterOperationAction(TypeParameterObjectCreationOperationAction, OperationKindEx.TypeParameterObjectCreation);
            }
            else
            {
                context.RegisterSyntaxNodeAction(ObjectCreationExpressionAction, SyntaxKind.ObjectCreationExpression);
            }
        }

        private static void HandleObjectCreationOperation(OperationAnalysisContext context)
        {
            var objectCreation = IObjectCreationOperationWrapper.FromOperation(context.Operation);

            var typeToCreate = objectCreation.Constructor.ContainingType;
            if ((typeToCreate == null) || typeToCreate.IsReferenceType || IsReferenceTypeParameter(typeToCreate))
            {
                return;
            }

            if (!objectCreation.Arguments.IsEmpty)
            {
                // Not a use of the default constructor
                return;
            }

            if (!objectCreation.Constructor.IsImplicitlyDeclared)
            {
                // The value type includes an explicit parameterless constructor
                return;
            }

            if (objectCreation.Initializer.WrappedOperation != null)
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Descriptor, objectCreation.WrappedOperation.Syntax.GetLocation()));
        }

        private static void HandleTypeParameterObjectCreationOperation(OperationAnalysisContext context)
        {
            var objectCreation = ITypeParameterObjectCreationOperationWrapper.FromOperation(context.Operation);

            var typeToCreate = objectCreation.Type;
            if ((typeToCreate == null) || typeToCreate.IsReferenceType || IsReferenceTypeParameter(typeToCreate))
            {
                return;
            }

            if (objectCreation.Initializer.WrappedOperation != null)
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Descriptor, objectCreation.WrappedOperation.Syntax.GetLocation()));
        }

        private static void HandleObjectCreationExpression(SyntaxNodeAnalysisContext context)
        {
            ObjectCreationExpressionSyntax newExpression = (ObjectCreationExpressionSyntax)context.Node;

            var typeToCreate = context.SemanticModel.GetTypeInfo(newExpression, context.CancellationToken);
            if ((typeToCreate.Type == null) || typeToCreate.Type.IsReferenceType || IsReferenceTypeParameter(typeToCreate.Type))
            {
                return;
            }

            if ((newExpression.ArgumentList == null) || (newExpression.ArgumentList.Arguments.Count > 0))
            {
                return;
            }

            if (newExpression.Initializer != null)
            {
                return;
            }

            context.ReportDiagnostic(Diagnostic.Create(Descriptor, newExpression.GetLocation()));
        }

        private static bool IsReferenceTypeParameter(ITypeSymbol type)
        {
            return (type.Kind == SymbolKind.TypeParameter) && !((ITypeParameterSymbol)type).HasValueTypeConstraint;
        }
    }
}
