// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.DocumentationRules
{
    using Microsoft.CodeAnalysis.CSharp;

    public partial class SA1600CSharp8UnitTests
    {
        // Using 'Default' here makes sure that later test projects also run these tests with their own language version, without having to override this property
        protected override LanguageVersion LanguageVersion => LanguageVersion.Default;
    }
}
