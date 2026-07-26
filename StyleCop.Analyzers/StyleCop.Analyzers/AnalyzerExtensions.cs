// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Immutable;
    using System.Runtime.CompilerServices;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Diagnostics;
    using StyleCop.Analyzers.Settings.ObjectModel;

    /// <summary>
    /// Provides extension methods to deal for analyzers.
    /// </summary>
    internal static class AnalyzerExtensions
    {
        /// <summary>
        /// Register an action to be executed at completion of parsing of a code document. A syntax tree action reports
        /// diagnostics about the <see cref="SyntaxTree"/> of a document.
        /// </summary>
        /// <param name="context">The analysis context.</param>
        /// <param name="action">Action to be executed at completion of parsing of a document.</param>
        public static void RegisterSyntaxTreeAction(this CompilationStartAnalysisContext context, Action<SyntaxTreeAnalysisContext, StyleCopSettings> action)
        {
            var settingsFile = context.GetStyleCopSettingsFile(context.CancellationToken);

            context.RegisterSyntaxTreeAction(
                context =>
                {
                    StyleCopSettings settings = context.GetStyleCopSettings(GetOrCreateSettingsStorage(context.Tree), settingsFile);
                    action(context, settings);
                });

            StrongBox<StyleCopSettings> GetOrCreateSettingsStorage(SyntaxTree tree)
                => SettingsHelper.GetOrCreateSettingsStorage(context, tree);
        }

        /// <summary>
        /// Register an action to be executed at completion of semantic analysis of a <see cref="SyntaxNode"/> with an
        /// appropriate kind. A syntax node action can report diagnostics about a <see cref="SyntaxNode"/>, and can also
        /// collect state information to be used by other syntax node actions or code block end actions.
        /// </summary>
        /// <param name="context">The analysis context.</param>
        /// <param name="action">Action to be executed at completion of semantic analysis of a
        /// <see cref="SyntaxNode"/>.</param>
        /// <param name="syntaxKind">The kind of syntax that should be analyzed.</param>
        /// <typeparam name="TLanguageKindEnum">Enum type giving the syntax node kinds of the source language for which
        /// the action applies.</typeparam>
        public static void RegisterSyntaxNodeAction<TLanguageKindEnum>(this CompilationStartAnalysisContext context, Action<SyntaxNodeAnalysisContext, StyleCopSettings> action, TLanguageKindEnum syntaxKind)
            where TLanguageKindEnum : struct
        {
            context.RegisterSyntaxNodeAction(action, LanguageKindArrays<TLanguageKindEnum>.GetOrCreateArray(syntaxKind));
        }

        /// <summary>
        /// Register an action to be executed at completion of semantic analysis of a <see cref="SyntaxNode"/> with an
        /// appropriate kind. A syntax node action can report diagnostics about a <see cref="SyntaxNode"/>, and can also
        /// collect state information to be used by other syntax node actions or code block end actions.
        /// </summary>
        /// <param name="context">The analysis context.</param>
        /// <param name="action">Action to be executed at completion of semantic analysis of a
        /// <see cref="SyntaxNode"/>.</param>
        /// <param name="syntaxKinds">The kinds of syntax that should be analyzed.</param>
        /// <typeparam name="TLanguageKindEnum">Enum type giving the syntax node kinds of the source language for which
        /// the action applies.</typeparam>
        public static void RegisterSyntaxNodeAction<TLanguageKindEnum>(this CompilationStartAnalysisContext context, Action<SyntaxNodeAnalysisContext, StyleCopSettings> action, ImmutableArray<TLanguageKindEnum> syntaxKinds)
            where TLanguageKindEnum : struct
        {
            var settingsFile = context.GetStyleCopSettingsFile(context.CancellationToken);

            context.RegisterSyntaxNodeAction(
                context =>
                {
                    StyleCopSettings settings = context.GetStyleCopSettings(GetOrCreateSettingsStorage(context.Node.SyntaxTree), settingsFile);
                    action(context, settings);
                },
                syntaxKinds);

            StrongBox<StyleCopSettings> GetOrCreateSettingsStorage(SyntaxTree tree)
                => SettingsHelper.GetOrCreateSettingsStorage(context, tree);
        }

        /// <summary>
        /// Register an action to be executed at completion of semantic analysis of a <see cref="SyntaxNode"/> with
        /// the given <paramref name="syntaxKind"/>, guarding against the analyzer driver invoking the action more
        /// than once for the same node. This is a workaround for a Roslyn quirk currently observed for very new
        /// experimental <see cref="Microsoft.CodeAnalysis.CSharp.SyntaxKind"/> values (e.g.
        /// <see cref="StyleCop.Analyzers.Lightup.SyntaxKindEx.UnionDeclaration"/>), where the analyzer driver
        /// dispatches the identical node twice within a single analysis pass.
        /// </summary>
        /// <param name="context">The analysis context.</param>
        /// <param name="action">Action to be executed at completion of semantic analysis of a
        /// <see cref="SyntaxNode"/>.</param>
        /// <param name="syntaxKind">The (experimental) kind of syntax that should be analyzed.</param>
        /// <typeparam name="TLanguageKindEnum">Enum type giving the syntax node kinds of the source language for which
        /// the action applies.</typeparam>
        // TODO: Remove these methods when it's no longer needed (c# 15 officially supported)
        public static void RegisterSyntaxNodeActionWithDuplicateNodeGuard<TLanguageKindEnum>(this CompilationStartAnalysisContext context, Action<SyntaxNodeAnalysisContext> action, TLanguageKindEnum syntaxKind)
            where TLanguageKindEnum : struct
        {
            var processedNodes = new ConcurrentDictionary<SyntaxNode, byte>();
            context.RegisterSyntaxNodeAction(
                nodeContext =>
                {
                    if (processedNodes.TryAdd(nodeContext.Node, 0))
                    {
                        action(nodeContext);
                    }
                },
                syntaxKind);
        }

        /// <summary>
        /// Register an action to be executed at completion of semantic analysis of a <see cref="SyntaxNode"/> with
        /// the given <paramref name="syntaxKind"/>, guarding against the analyzer driver invoking the action more
        /// than once for the same node. See
        /// <see cref="RegisterSyntaxNodeActionWithDuplicateNodeGuard{TLanguageKindEnum}(CompilationStartAnalysisContext, Action{SyntaxNodeAnalysisContext}, TLanguageKindEnum)"/>
        /// for details on why this guard is needed.
        /// </summary>
        /// <param name="context">The analysis context.</param>
        /// <param name="action">Action to be executed at completion of semantic analysis of a
        /// <see cref="SyntaxNode"/>.</param>
        /// <param name="syntaxKind">The (experimental) kind of syntax that should be analyzed.</param>
        /// <typeparam name="TLanguageKindEnum">Enum type giving the syntax node kinds of the source language for which
        /// the action applies.</typeparam>
        public static void RegisterSyntaxNodeActionWithDuplicateNodeGuard<TLanguageKindEnum>(this CompilationStartAnalysisContext context, Action<SyntaxNodeAnalysisContext, StyleCopSettings> action, TLanguageKindEnum syntaxKind)
            where TLanguageKindEnum : struct
        {
            var processedNodes = new ConcurrentDictionary<SyntaxNode, byte>();
            context.RegisterSyntaxNodeAction(
                (nodeContext, settings) =>
                {
                    if (processedNodes.TryAdd(nodeContext.Node, 0))
                    {
                        action(nodeContext, settings);
                    }
                },
                syntaxKind);
        }

        private static class LanguageKindArrays<TLanguageKindEnum>
            where TLanguageKindEnum : struct
        {
            private static readonly ConcurrentDictionary<TLanguageKindEnum, ImmutableArray<TLanguageKindEnum>> Arrays =
                new ConcurrentDictionary<TLanguageKindEnum, ImmutableArray<TLanguageKindEnum>>();

            private static readonly Func<TLanguageKindEnum, ImmutableArray<TLanguageKindEnum>> CreateValueFactory = CreateValue;

            public static ImmutableArray<TLanguageKindEnum> GetOrCreateArray(TLanguageKindEnum syntaxKind)
            {
                return Arrays.GetOrAdd(syntaxKind, CreateValueFactory);
            }

            private static ImmutableArray<TLanguageKindEnum> CreateValue(TLanguageKindEnum syntaxKind)
            {
                return ImmutableArray.Create(syntaxKind);
            }
        }
    }
}
