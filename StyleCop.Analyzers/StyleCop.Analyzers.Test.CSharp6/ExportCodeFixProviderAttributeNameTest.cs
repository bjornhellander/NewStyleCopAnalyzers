// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeFixes;
    using StyleCop.Analyzers.SpacingRules;
    using Xunit;

    public class ExportCodeFixProviderAttributeNameTest
    {
        public static TheoryData<Type> CodeFixProviderTypeData
        {
            get
            {
                var codeFixProviders = typeof(TokenSpacingCodeFixProvider)
                    .Assembly
                    .GetTypes()
                    .Where(t => typeof(CodeFixProvider).IsAssignableFrom(t));

                return new(codeFixProviders);
            }
        }

        [Theory]
        [MemberData(nameof(CodeFixProviderTypeData))]
        public void TestExportCodeFixProviderAttribute(Type codeFixProvider)
        {
            var exportCodeFixProviderAttribute = codeFixProvider.GetCustomAttributes<ExportCodeFixProviderAttribute>(false).FirstOrDefault();
            var noCodeFixAttribute = codeFixProvider.GetCustomAttributes<NoCodeFixAttribute>(false).FirstOrDefault();

            if (noCodeFixAttribute != null)
            {
                Assert.Null(exportCodeFixProviderAttribute);

                return;
            }

            Assert.NotNull(exportCodeFixProviderAttribute);
            Assert.Equal(codeFixProvider.Name, exportCodeFixProviderAttribute.Name);
            Assert.Single(exportCodeFixProviderAttribute.Languages);
            Assert.Equal(LanguageNames.CSharp, exportCodeFixProviderAttribute.Languages[0]);
        }
    }
}
