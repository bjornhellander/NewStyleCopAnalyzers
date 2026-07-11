// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Lightup
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    internal static class LightupHelpers
    {
        public static bool SupportsCSharp7 { get; }
            = Enum.GetNames(typeof(LanguageVersion)).Contains(nameof(LanguageVersionEx.CSharp7));

        public static bool SupportsCSharp71 { get; }
            = Enum.GetNames(typeof(LanguageVersion)).Contains(nameof(LanguageVersionEx.CSharp7_1));

        public static bool SupportsCSharp72 { get; }
            = Enum.GetNames(typeof(LanguageVersion)).Contains(nameof(LanguageVersionEx.CSharp7_2));

        public static bool SupportsCSharp73 { get; }
            = Enum.GetNames(typeof(LanguageVersion)).Contains(nameof(LanguageVersionEx.CSharp7_3));

        public static bool SupportsCSharp8 { get; }
            = Enum.GetNames(typeof(LanguageVersion)).Contains(nameof(LanguageVersionEx.CSharp8));

        public static bool SupportsCSharp9 { get; }
            = Enum.GetNames(typeof(LanguageVersion)).Contains(nameof(LanguageVersionEx.CSharp9));

        public static bool SupportsCSharp10 { get; }
            = Enum.GetNames(typeof(LanguageVersion)).Contains(nameof(LanguageVersionEx.CSharp10));

        public static bool SupportsCSharp11 { get; }
            = Enum.GetNames(typeof(LanguageVersion)).Contains(nameof(LanguageVersionEx.CSharp11));

        public static bool SupportsCSharp12 { get; }
            = Enum.GetNames(typeof(LanguageVersion)).Contains(nameof(LanguageVersionEx.CSharp12));

        public static bool SupportsCSharp13 { get; }
            = Enum.GetNames(typeof(LanguageVersion)).Contains(nameof(LanguageVersionEx.CSharp13));

        public static bool SupportsCSharp14 { get; }
            = Enum.GetNames(typeof(LanguageVersion)).Contains(nameof(LanguageVersionEx.CSharp14));

        // TODO: Update when the CSharp15 member exists
        public static bool SupportsCSharp15 { get; }
            = Enum.GetNames(typeof(SyntaxKind)).Contains(nameof(SyntaxKindEx.UnionKeyword));

        public static bool SupportsIOperation => SupportsCSharp73;
    }
}
