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
    /// The name of a public or internal field in C# does not begin with an upper-case letter.
    /// </summary>
    /// <remarks>
    /// <para>A violation of this rule occurs when the name of a public or internal field begins with a lower-case
    /// letter. Public or internal fields should being with an upper-case letter.</para>
    ///
    /// <para>If the field or variable name is intended to match the name of an item associated with Win32 or COM, and
    /// thus needs to start with a lower-case letter, place the field or variable within a special <c>NativeMethods</c>
    /// class. A <c>NativeMethods</c> class is any class which contains a name ending in <c>NativeMethods</c>, and is
    /// intended as a placeholder for Win32 or COM wrappers. StyleCop will ignore this violation if the item is placed
    /// within a <c>NativeMethods</c> class.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SA1307AccessibleFieldsMustBeginWithUpperCaseLetter : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1307AccessibleFieldsMustBeginWithUpperCaseLetter"/>
        /// analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1307";
        private const string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1307.md";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(NamingResources.SA1307Title), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(NamingResources.SA1307MessageFormat), NamingResources.ResourceManager, typeof(NamingResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(NamingResources.SA1307Description), NamingResources.ResourceManager, typeof(NamingResources));

#pragma warning disable SA1202 // Elements should be ordered by access
        internal static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.NamingRules, DiagnosticSeverity.Warning, AnalyzerConstants.EnabledByDefault, Description, HelpLink);
#pragma warning restore SA1202 // Elements should be ordered by access

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
            // To improve performance we are looking for the field instead of the declarator directly. That way we don't get called for local variables.
            FieldDeclarationSyntax declaration = (FieldDeclarationSyntax)context.Node;
            if (declaration.Declaration != null)
            {
                if (declaration.Modifiers.Any(SyntaxKind.ConstKeyword))
                {
                    // These are reported as SA1303.
                    return;
                }

                if (declaration.Modifiers.Any(SyntaxKind.PublicKeyword) || declaration.Modifiers.Any(SyntaxKind.InternalKeyword))
                {
                    foreach (VariableDeclaratorSyntax declarator in declaration.Declaration.Variables)
                    {
                        string name = declarator.Identifier.ValueText;

                        if (!string.IsNullOrEmpty(name)
                            && char.IsLower(name[0])
                            && !NamedTypeHelpers.IsContainedInNativeMethodsClass(declaration))
                        {
                            context.ReportDiagnostic(Diagnostic.Create(Descriptor, declarator.Identifier.GetLocation(), name));
                        }
                    }
                }
            }
        }
    }
}
