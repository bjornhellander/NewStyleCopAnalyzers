﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.MaintainabilityRules
{
    using System;
    using System.Collections.Immutable;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Text;
    using StyleCop.Analyzers.Helpers;

    /// <summary>
    /// Store files as UTF-8 with byte order mark.
    /// </summary>
    /// <remarks>
    /// <para>Storing files in this encoding ensures that the files are always treated the same way by the compiler,
    /// even when compiled on systems with varying default system encodings. In addition,
    /// this encoding is the most widely supported encoding for features like visual diffs on GitHub and other tooling.
    /// This encoding is also the default encoding used when creating new C# source files within Visual Studio.
    /// </para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    internal class SA1412StoreFilesAsUtf8 : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the
        /// <see cref="SA1412StoreFilesAsUtf8"/> analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1412";
        private const string HelpLink = "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1412.md";
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(MaintainabilityResources.SA1412Title), MaintainabilityResources.ResourceManager, typeof(MaintainabilityResources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(MaintainabilityResources.SA1412MessageFormat), MaintainabilityResources.ResourceManager, typeof(MaintainabilityResources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(MaintainabilityResources.SA1412Description), MaintainabilityResources.ResourceManager, typeof(MaintainabilityResources));

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, AnalyzerCategory.MaintainabilityRules, DiagnosticSeverity.Warning, AnalyzerConstants.DisabledByDefault, Description, HelpLink);

        private static readonly Action<SyntaxTreeAnalysisContext> SyntaxTreeAction = HandleSyntaxTree;

        private static readonly byte[] Utf8Preamble = Encoding.UTF8.GetPreamble();

        /// <summary>
        /// Gets the key for the detected encoding name in the <see cref="Diagnostic.Properties"/> collection.
        /// </summary>
        /// <value>
        /// The key for the detected encoding name in the <see cref="Diagnostic.Properties"/> collection.
        /// </value>
        public static string EncodingProperty { get; } = "Encoding";

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxTreeAction(SyntaxTreeAction);
        }

        private static void HandleSyntaxTree(SyntaxTreeAnalysisContext context)
        {
            if (context.Tree.IsWhitespaceOnly(context.CancellationToken))
            {
                // Handling of empty documents is now the responsibility of the analyzers
                return;
            }

            byte[] preamble = context.Tree.Encoding.GetPreamble();

            if (!IsUtf8Preamble(preamble))
            {
                ImmutableDictionary<string, string> properties = ImmutableDictionary<string, string>.Empty.SetItem(EncodingProperty, context.Tree.Encoding?.WebName ?? "<null>");
                context.ReportDiagnostic(Diagnostic.Create(Descriptor, Location.Create(context.Tree, TextSpan.FromBounds(0, 0)), properties));
            }
        }

        private static bool IsUtf8Preamble(byte[] preamble)
        {
            if (preamble == null || preamble.Length != Utf8Preamble.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < Utf8Preamble.Length; i++)
                {
                    if (Utf8Preamble[i] != preamble[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
