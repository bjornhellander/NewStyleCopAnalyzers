﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.NamingRules
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Helpers;

    /// <summary>
    /// The name of a C# interface does not begin with the capital letter I.
    /// </summary>
    /// <remarks>
    /// <para>A violation of this rule occurs when the name of an interface does not begin with the capital letter I.
    /// Interface names should always begin with I. For example, <c>ICustomer</c>.</para>
    ///
    /// <para>If the field or variable name is intended to match the name of an item associated with Win32 or COM, and
    /// thus cannot begin with the letter I, place the field or variable within a special <c>NativeMethods</c> class. A
    /// <c>NativeMethods</c> class is any class which contains a name ending in <c>NativeMethods</c>, and is intended as
    /// a placeholder for Win32 or COM wrappers. StyleCop will ignore this violation if the item is placed within a
    /// <c>NativeMethods</c> class.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SA1302InterfaceNamesMustBeginWithI : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1302InterfaceNamesMustBeginWithI"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1302";
        private const string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1302.md";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(NamingResources.SA1302Title), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(NamingResources.SA1302MessageFormat), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(NamingResources.SA1302Description), NamingResources.ResourceManager, typeof(NamingResources));

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.NamingRules, DiagnosticSeverity.Warning, AnalyzerConstants.EnabledByDefault, Description, HelpLink);

        private static readonly Action<SyntaxNodeAnalysisContext> InterfaceDeclarationAction = HandleInterfaceDeclaration;

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(InterfaceDeclarationAction, SyntaxKind.InterfaceDeclaration);
        }

        private static void HandleInterfaceDeclaration(SyntaxNodeAnalysisContext context)
        {
            var interfaceDeclaration = (InterfaceDeclarationSyntax)context.Node;
            if (interfaceDeclaration.Identifier.IsMissing)
            {
                return;
            }

            if (NamedTypeHelpers.IsContainedInNativeMethodsClass(interfaceDeclaration))
            {
                return;
            }

            string name = interfaceDeclaration.Identifier.ValueText;
            if (name != null && !name.StartsWith("I", StringComparison.Ordinal))
            {
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, interfaceDeclaration.Identifier.GetLocation()));
            }
        }
    }
}
