// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.SpecialRules
{
    using System;
    using System.Collections.Immutable;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Testing;
    using Microsoft.CodeAnalysis.Text;
    using StyleCop.Analyzers.Settings;
    using StyleCop.Analyzers.SpecialRules;
    using Xunit;
    using static StyleCop.Analyzers.Test.CSharp6.Verifiers.StyleCopDiagnosticVerifier<StyleCop.Analyzers.SpecialRules.SA0002InvalidSettingsFile>;

    /// <summary>
    /// Unit tests for <see cref="SA0002InvalidSettingsFile"/>.
    /// </summary>
    public class SA0002UnitTests
    {
        private const string TestCode = @"
namespace NamespaceName { }
";

        [Fact]
        public async Task TestMissingSettingsAsync()
        {
            await VerifyCSharpDiagnosticAsync(TestCode, DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestNoSourceFilesAsync()
        {
            string emptySettings = @"{ ""settings"": { } }";
            await new CSharpTest
            {
                TestState =
                {
                    Sources = { string.Empty },
                    AdditionalFiles = { ("stylecop.json", emptySettings) },
                    AdditionalProjects =
                    {
                        ["EmptyProjectWithSettings"] =
                        {
                            // The main test state doesn't allow empty Sources, so we use a second project to validate
                            // the completely empty case.
                            AdditionalFiles = { ("stylecop.json", emptySettings) },
                        },
                    },
                },
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestValidSettingsAsync()
        {
            await new CSharpTest
            {
                TestCode = TestCode,
                Settings = SettingsFileCodeFixProvider.DefaultSettingsFileContent,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInvalidSettingsAsync()
        {
            // The settings file is missing a comma after the $schema property
            var settings = @"
{
  ""$schema"": ""https://raw.githubusercontent.com/bjornhellander/NewStyleCopAnalyzers/master/StyleCop.Analyzers/StyleCop.Analyzers/Settings/stylecop.schema.json""
  ""settings"": {
    ""documentationRules"": {
      ""companyName"": ""ACME, Inc"",
      ""copyrightText"": ""Copyright 2015 {companyName}. All rights reserved.""
    }
  }
}
";

            // This diagnostic is reported without a location
            DiagnosticResult expected = Diagnostic();

            await new CSharpTest
            {
                TestCode = TestCode,
                ExpectedDiagnostics = { expected },
                Settings = settings,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInvalidSettingStringValueAsync()
        {
            var settings = @"
{
  ""settings"": {
    ""documentationRules"": {
      ""companyName"": 3
    }
  }
}
";

            // This diagnostic is reported without a location
            DiagnosticResult expected = Diagnostic();

            await new CSharpTest
            {
                TestCode = TestCode,
                ExpectedDiagnostics = { expected },
                Settings = settings,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInvalidSettingStringArrayElementValueAsync()
        {
            var settings = @"
{
  ""settings"": {
    ""namingRules"": {
      ""allowedHungarianPrefixes"": [ 3 ]
    }
  }
}
";

            // This diagnostic is reported without a location
            DiagnosticResult expected = Diagnostic();

            await new CSharpTest
            {
                TestCode = TestCode,
                ExpectedDiagnostics = { expected },
                Settings = settings,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInvalidSettingBooleanValueAsync()
        {
            var settings = @"
{
  ""settings"": {
    ""documentationRules"": {
      ""xmlHeader"": 3
    }
  }
}
";

            // This diagnostic is reported without a location
            DiagnosticResult expected = Diagnostic();

            await new CSharpTest
            {
                TestCode = TestCode,
                ExpectedDiagnostics = { expected },
                Settings = settings,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInvalidSettingIntegerValueAsync()
        {
            var settings = @"
{
  ""settings"": {
    ""indentation"": {
      ""tabSize"": ""3""
    }
  }
}
";

            // This diagnostic is reported without a location
            DiagnosticResult expected = Diagnostic();

            await new CSharpTest
            {
                TestCode = TestCode,
                ExpectedDiagnostics = { expected },
                Settings = settings,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInvalidSettingEnumValueNotStringAsync()
        {
            var settings = @"
{
  ""settings"": {
    ""documentationRules"": {
      ""fileNamingConvention"": 3
    }
  }
}
";

            // This diagnostic is reported without a location
            DiagnosticResult expected = Diagnostic();

            await new CSharpTest
            {
                TestCode = TestCode,
                ExpectedDiagnostics = { expected },
                Settings = settings,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInvalidSettingArrayElementEnumValueNotStringAsync()
        {
            var settings = @"
{
  ""settings"": {
    ""maintainabilityRules"": {
      ""topLevelTypes"": [ 3 ]
    }
  }
}
";

            // This diagnostic is reported without a location
            DiagnosticResult expected = Diagnostic();

            await new CSharpTest
            {
                TestCode = TestCode,
                ExpectedDiagnostics = { expected },
                Settings = settings,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInvalidSettingArrayElementEnumValueNotRecognizedAsync()
        {
            var settings = @"
{
  ""settings"": {
    ""maintainabilityRules"": {
      ""topLevelTypes"": [ ""Some incorrect value"" ]
    }
  }
}
";

            // This diagnostic is reported without a location
            DiagnosticResult expected = Diagnostic();

            await new CSharpTest
            {
                TestCode = TestCode,
                ExpectedDiagnostics = { expected },
                Settings = settings,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInvalidSettingArrayAsync()
        {
            var settings = @"
{
  ""settings"": {
    ""namingRules"": {
      ""allowedHungarianPrefixes"": ""ah""
    }
  }
}
";

            // This diagnostic is reported without a location
            DiagnosticResult expected = Diagnostic();

            await new CSharpTest
            {
                TestCode = TestCode,
                ExpectedDiagnostics = { expected },
                Settings = settings,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInvalidSettingObjectAsync()
        {
            var settings = @"
{
  ""settings"": {
    ""namingRules"": true
  }
}
";

            // This diagnostic is reported without a location
            DiagnosticResult expected = Diagnostic();

            await new CSharpTest
            {
                TestCode = TestCode,
                ExpectedDiagnostics = { expected },
                Settings = settings,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestInvalidSettingSyntaxAsync()
        {
            // Missing the ':' between "companyName" and "name"
            var settings = @"
{
  ""settings"": {
    ""documentationRules"": {
      ""companyName"" ""name""
    }
  }
}
";

            // This diagnostic is reported without a location
            DiagnosticResult expected = Diagnostic();

            await new CSharpTest
            {
                TestCode = TestCode,
                ExpectedDiagnostics = { expected },
                Settings = settings,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestEmptySettingsAsync()
        {
            // The test infrastructure will not add a settings file to the compilation if GetSettings returns null or an empty string.
            // This is why we set settings to a simple whitespace character.
            var settings = " ";

            // This diagnostic is reported without a location
            DiagnosticResult expected = Diagnostic();

            await new CSharpTest
            {
                TestCode = TestCode,
                ExpectedDiagnostics = { expected },
                Settings = settings,
            }.RunAsync(CancellationToken.None).ConfigureAwait(true);
        }

        [Fact]
        public async Task TestUnexpectedExceptionNotCaughtAsync()
        {
            var cancellationToken = TestContext.Current.CancellationToken;

            var syntaxTree = CSharpSyntaxTree.ParseText(string.Empty, cancellationToken: cancellationToken);
            var compilation = CSharpCompilation.Create("Test", new[] { syntaxTree });

            var additionalFiles = ImmutableArray.Create<AdditionalText>(new InvalidAdditionalText());
            Assert.Null(additionalFiles[0].Path);
            Assert.Null(additionalFiles[0].GetText(cancellationToken));

            var analyzer = new SA0002InvalidSettingsFile();
            var compilationWithAnalyzers = compilation.WithAnalyzers(ImmutableArray.Create<DiagnosticAnalyzer>(analyzer), new AnalyzerOptions(additionalFiles), cancellationToken);
            var diagnostics = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync(cancellationToken).ConfigureAwait(true);

            var diagnostic = Assert.Single(diagnostics);
            Assert.Equal("AD0001", diagnostic.Id);
            Assert.Contains(nameof(ArgumentNullException), diagnostic.GetMessage());
        }

        private class InvalidAdditionalText : AdditionalText
        {
            public override string? Path => null;

            public override SourceText? GetText(CancellationToken cancellationToken) => null;
        }
    }
}
