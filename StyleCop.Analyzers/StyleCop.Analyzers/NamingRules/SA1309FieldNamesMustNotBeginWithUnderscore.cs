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
    /// A field name in C# begins with an underscore.
    /// </summary>
    /// <remarks>
    /// <para>A violation of this rule occurs when a field name begins with an underscore.</para>
    ///
    /// <para>By default, StyleCop disallows the use of underscores, <c>m_</c>, etc., to mark local class fields, in
    /// favor of the <c>this.</c> prefix. The advantage of using <c>this.</c> is that it applies equally to all element
    /// types including methods, properties, etc., and not just fields, making all calls to class members instantly
    /// recognizable, regardless of which editor is being used to view the code. Another advantage is that it creates a
    /// quick, recognizable differentiation between instance members and static members, which will not be
    /// prefixed.</para>
    ///
    /// <para>If the field or variable name is intended to match the name of an item associated with Win32 or COM, and
    /// thus needs to begin with an underscore, place the field or variable within a special <c>NativeMethods</c> class.
    /// A <c>NativeMethods</c> class is any class which contains a name ending in <c>NativeMethods</c>, and is intended
    /// as a placeholder for Win32 or COM wrappers. StyleCop will ignore this violation if the item is placed within a
    /// <c>NativeMethods</c> class.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SA1309FieldNamesMustNotBeginWithUnderscore : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1309FieldNamesMustNotBeginWithUnderscore"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1309";
        private const string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1309.md";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(NamingResources.SA1309Title), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(NamingResources.SA1309MessageFormat), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(NamingResources.SA1309Description), NamingResources.ResourceManager, typeof(NamingResources));

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.NamingRules, DiagnosticSeverity.Warning, AnalyzerConstants.EnabledByDefault, Description, HelpLink);

        private static readonly Action<SyntaxNodeAnalysisContext> FieldDeclarationAction = HandleFieldDeclaration;

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(FieldDeclarationAction, SyntaxKind.FieldDeclaration);
        }

        private static void HandleFieldDeclaration(SyntaxNodeAnalysisContext context)
        {
            FieldDeclarationSyntax syntax = (FieldDeclarationSyntax)context.Node;
            if (NamedTypeHelpers.IsContainedInNativeMethodsClass(syntax))
            {
                return;
            }

            var variables = syntax.Declaration?.Variables;
            if (variables == null)
            {
                return;
            }

            foreach (VariableDeclaratorSyntax variableDeclarator in variables.Value)
            {
                if (variableDeclarator == null)
                {
                    continue;
                }

                var identifier = variableDeclarator.Identifier;
                if (identifier.IsMissing)
                {
                    continue;
                }

                if (!identifier.ValueText.StartsWith("_", StringComparison.Ordinal))
                {
                    continue;
                }

                // Field '{name}' should not begin with an underscore
                string name = identifier.ValueText;
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, identifier.GetLocation(), name));
            }
        }
    }
}
