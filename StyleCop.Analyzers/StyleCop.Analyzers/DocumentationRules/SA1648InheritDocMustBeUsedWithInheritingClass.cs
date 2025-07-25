﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.DocumentationRules
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Xml.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Helpers;

    /// <summary>
    /// <c>&lt;inheritdoc&gt;</c> has been used on an element that doesn't inherit from a base class or implement an
    /// interface.
    /// </summary>
    /// <seealso href="https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1648.md">SA1648</seealso>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SA1648InheritDocMustBeUsedWithInheritingClass : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1648InheritDocMustBeUsedWithInheritingClass"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1648";
        private const string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1648.md";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(DocumentationResources.SA1648Title), DocumentationResources.ResourceManager, typeof(DocumentationResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(DocumentationResources.SA1648MessageFormat), DocumentationResources.ResourceManager, typeof(DocumentationResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(DocumentationResources.SA1648Description), DocumentationResources.ResourceManager, typeof(DocumentationResources));

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.DocumentationRules, DiagnosticSeverity.Warning, AnalyzerConstants.EnabledByDefault, Description, HelpLink);

        private static readonly ImmutableArray<SyntaxKind> HandledTypeLikeDeclarationKinds =
            ImmutableArray.Create(SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration, SyntaxKind.InterfaceDeclaration, SyntaxKind.EnumDeclaration, SyntaxKind.DelegateDeclaration);

        private static readonly ImmutableArray<SyntaxKind> MemberDeclarationKinds =
            ImmutableArray.Create(
                SyntaxKind.ConstructorDeclaration,
                SyntaxKind.DestructorDeclaration,
                SyntaxKind.EventDeclaration,
                SyntaxKind.MethodDeclaration,
                SyntaxKind.PropertyDeclaration,
                SyntaxKind.EventFieldDeclaration,
                SyntaxKind.FieldDeclaration,
                SyntaxKind.IndexerDeclaration,
                SyntaxKind.OperatorDeclaration,
                SyntaxKind.ConversionOperatorDeclaration);

        private static readonly Action<SyntaxNodeAnalysisContext> BaseTypeLikeDeclarationAction = HandleBaseTypeLikeDeclaration;
        private static readonly Action<SyntaxNodeAnalysisContext> MemberDeclarationAction = HandleMemberDeclaration;

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(BaseTypeLikeDeclarationAction, HandledTypeLikeDeclarationKinds);
            context.RegisterSyntaxNodeAction(MemberDeclarationAction, MemberDeclarationKinds);
        }

        private static void HandleBaseTypeLikeDeclaration(SyntaxNodeAnalysisContext context)
        {
            BaseTypeDeclarationSyntax? baseType = context.Node as BaseTypeDeclarationSyntax;

            // baseType can be null here if we are looking at a delegate declaration
            if (baseType != null && baseType.BaseList != null && baseType.BaseList.Types.Any())
            {
                return;
            }

            DocumentationCommentTriviaSyntax documentation = context.Node.GetDocumentationCommentTriviaSyntax();
            if (documentation == null)
            {
                return;
            }

            Location location;

            if (documentation.Content.GetFirstXmlElement(XmlCommentHelper.IncludeXmlTag) is XmlEmptyElementSyntax includeElement)
            {
                var declaration = context.SemanticModel.GetDeclaredSymbol(context.Node, context.CancellationToken);
                if (declaration == null)
                {
                    return;
                }

                var rawDocumentation = declaration.GetDocumentationCommentXml(expandIncludes: true, cancellationToken: context.CancellationToken);
                var completeDocumentation = XElement.Parse(rawDocumentation, LoadOptions.None);

                var inheritDocElement = completeDocumentation.Nodes().OfType<XElement>().FirstOrDefault(element => element.Name == XmlCommentHelper.InheritdocXmlTag);
                if (inheritDocElement == null)
                {
                    return;
                }

                if (HasXmlCrefAttribute(inheritDocElement))
                {
                    return;
                }

                location = includeElement.GetLocation();
            }
            else
            {
                XmlNodeSyntax inheritDocElement = documentation.Content.GetFirstXmlElement(XmlCommentHelper.InheritdocXmlTag);
                if (inheritDocElement == null)
                {
                    return;
                }

                if (HasXmlCrefAttribute(inheritDocElement))
                {
                    return;
                }

                location = inheritDocElement.GetLocation();
            }

            context.ReportDiagnostic(Diagnostic.Create(Descriptor, location));
        }

        private static void HandleMemberDeclaration(SyntaxNodeAnalysisContext context)
        {
            MemberDeclarationSyntax memberSyntax = (MemberDeclarationSyntax)context.Node;

            var modifiers = memberSyntax.GetModifiers();

            if (modifiers.Any(SyntaxKind.OverrideKeyword))
            {
                return;
            }

            DocumentationCommentTriviaSyntax documentation = memberSyntax.GetDocumentationCommentTriviaSyntax();
            if (documentation == null)
            {
                return;
            }

            Location location;

            ISymbol declaredSymbol = context.SemanticModel.GetDeclaredSymbol(memberSyntax, context.CancellationToken);

            if (memberSyntax is ConstructorDeclarationSyntax constructorDeclarationSyntax && declaredSymbol is IMethodSymbol constructorMethodSymbol)
            {
                if (constructorMethodSymbol.ContainingType != null)
                {
                    INamedTypeSymbol baseType = constructorMethodSymbol.ContainingType.BaseType;

                    if (HasMatchingSignature(baseType.Constructors, constructorMethodSymbol))
                    {
                        return;
                    }
                }
            }

            if (declaredSymbol == null && memberSyntax.IsKind(SyntaxKind.EventFieldDeclaration))
            {
                var eventFieldDeclarationSyntax = (EventFieldDeclarationSyntax)memberSyntax;
                VariableDeclaratorSyntax? firstVariable = eventFieldDeclarationSyntax.Declaration?.Variables.FirstOrDefault();
                if (firstVariable != null)
                {
                    declaredSymbol = context.SemanticModel.GetDeclaredSymbol(firstVariable, context.CancellationToken);
                }
            }

            if (documentation.Content.GetFirstXmlElement(XmlCommentHelper.IncludeXmlTag) is XmlEmptyElementSyntax includeElement)
            {
                if (declaredSymbol == null)
                {
                    return;
                }

                var rawDocumentation = declaredSymbol.GetDocumentationCommentXml(expandIncludes: true, cancellationToken: context.CancellationToken);
                var completeDocumentation = XElement.Parse(rawDocumentation, LoadOptions.None);

                var inheritDocElement = completeDocumentation.Nodes().OfType<XElement>().FirstOrDefault(element => element.Name == XmlCommentHelper.InheritdocXmlTag);
                if (inheritDocElement == null)
                {
                    return;
                }

                if (HasXmlCrefAttribute(inheritDocElement))
                {
                    return;
                }

                location = includeElement.GetLocation();
            }
            else
            {
                XmlNodeSyntax inheritDocElement = documentation.Content.GetFirstXmlElement(XmlCommentHelper.InheritdocXmlTag);
                if (inheritDocElement == null)
                {
                    return;
                }

                if (HasXmlCrefAttribute(inheritDocElement))
                {
                    return;
                }

                location = inheritDocElement.GetLocation();
            }

            // If we don't have a declared symbol we have some kind of field declaration. A field can not override or
            // implement anything so we want to report a diagnostic.
            if (declaredSymbol == null || !NamedTypeHelpers.IsImplementingAnInterfaceMember(declaredSymbol))
            {
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, location));
            }
        }

        /// <summary>
        /// Method compares a <paramref name="constructorMethodSymbol">constructor method</paramref> signature against its
        /// <paramref name="baseConstructorSymbols">base type constructors</paramref> to find if there is a method signature match.
        /// </summary>
        /// <returns><see langword="true"/> if any base type constructor's signature matches the signature of <paramref name="constructorMethodSymbol"/>, <see langword="false"/> otherwise.</returns>
        private static bool HasMatchingSignature(ImmutableArray<IMethodSymbol> baseConstructorSymbols, IMethodSymbol constructorMethodSymbol)
        {
            foreach (IMethodSymbol baseConstructorMethod in baseConstructorSymbols)
            {
                // Constructors must have the same number of parameters.
                if (constructorMethodSymbol.Parameters.Length != baseConstructorMethod.Parameters.Length)
                {
                    continue;
                }

                // Our constructor and the base constructor must have the same signature. But variable names can be different.
                bool success = true;

                for (int i = 0; i < constructorMethodSymbol.Parameters.Length; i++)
                {
                    IParameterSymbol constructorParameter = constructorMethodSymbol.Parameters[i];
                    IParameterSymbol baseParameter = baseConstructorMethod.Parameters[i];

                    if (!constructorParameter.Type.Equals(baseParameter.Type))
                    {
                        success = false;
                        break;
                    }
                }

                if (success)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasXmlCrefAttribute(XmlNodeSyntax inheritDocElement)
        {
            XmlElementSyntax? xmlElementSyntax = inheritDocElement as XmlElementSyntax;
            if (xmlElementSyntax?.StartTag?.Attributes.Any(SyntaxKind.XmlCrefAttribute) ?? false)
            {
                return true;
            }

            XmlEmptyElementSyntax? xmlEmptyElementSyntax = inheritDocElement as XmlEmptyElementSyntax;
            if (xmlEmptyElementSyntax?.Attributes.Any(SyntaxKind.XmlCrefAttribute) ?? false)
            {
                return true;
            }

            return false;
        }

        private static bool HasXmlCrefAttribute(XElement inheritDocElement)
        {
            return inheritDocElement.Attribute(XmlCommentHelper.CrefArgumentName) != null;
        }
    }
}
