// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.MaintainabilityRules
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CodeFixes;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Testing;
    using StyleCop.Analyzers.MaintainabilityRules;
    using Xunit;

    public abstract class SA1402ForBlockDeclarationUnitTestsBase : FileMayOnlyContainTestBase
    {
        public override bool SupportsCodeFix => true;

        protected override DiagnosticAnalyzer Analyzer => new SA1402FileMayOnlyContainASingleType();

        protected override CodeFixProvider CodeFix => new SA1402CodeFixProvider();

        protected SA1402SettingsConfiguration SettingsConfiguration { get; set; } = SA1402SettingsConfiguration.ConfigureAsTopLevelType;

        protected virtual string SettingKeyword => this.Keyword;

        protected abstract bool IsConfiguredAsTopLevelTypeByDefault { get; }

        protected virtual string MemberModifier => "public ";

        protected virtual bool SupportsStaticMemberUsageInBodies => true;

        [Fact]
        public async Task TestTwoGenericElementsAsync()
        {
            var testCode = @"%1 Foo<T1>
{
}
%1 Bar<T2, T3>
{
}";

            var fixedCode = new[]
            {
                ("/0/Test0.cs", @"%1 Foo<T1>
{
}
"),
                ("Bar{T2,T3}.cs", @"%1 Bar<T2, T3>
{
}"),
            };

            testCode = testCode.Replace("%1", this.Keyword);
            fixedCode = fixedCode.Select(c => (c.Item1, c.Item2.Replace("%1", this.Keyword))).ToArray();

            DiagnosticResult expected = this.Diagnostic().WithLocation(4, this.Keyword.Length + 2);
            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestTwoElementsWithRuleDisabledAsync()
        {
            this.SettingsConfiguration = SA1402SettingsConfiguration.ConfigureAsNonTopLevelType;

            var testCode = @"%1 Foo
{
}
%1 Bar
{
}";

            testCode = testCode.Replace("%1", this.Keyword);

            await this.VerifyCSharpDiagnosticAsync(testCode, this.GetSettings(), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestTwoElementsWithDefaultRuleConfigurationAsync()
        {
            this.SettingsConfiguration = SA1402SettingsConfiguration.KeepDefaultConfiguration;

            var testCode = @"%1 Foo
{
}
%1 Bar
{
}";

            var fixedCode = new[]
            {
                ("/0/Test0.cs", @"%1 Foo
{
}
"),
                ("Bar.cs", @"%1 Bar
{
}"),
            };

            testCode = testCode.Replace("%1", this.Keyword);
            fixedCode = fixedCode.Select(c => (c.Item1, c.Item2.Replace("%1", this.Keyword))).ToArray();

            if (this.IsConfiguredAsTopLevelTypeByDefault)
            {
                DiagnosticResult expected = this.Diagnostic().WithLocation(4, this.Keyword.Length + 2);
                await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
            }
            else
            {
                await this.VerifyCSharpDiagnosticAsync(testCode, this.GetSettings(), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
            }
        }

        [Fact]
        public async Task TestPartialTypesAsync()
        {
            var testCode = $@"public partial {this.Keyword} Foo
{{
}}
public partial {this.Keyword} Foo
{{

}}";

            await this.VerifyCSharpDiagnosticAsync(testCode, this.GetSettings(), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestDifferentPartialTypesAsync()
        {
            var testCode = $@"public partial {this.Keyword} Foo
{{
}}
public partial {this.Keyword} Bar
{{

}}";

            var fixedCode = new[]
            {
                ("/0/Test0.cs", $@"public partial {this.Keyword} Foo
{{
}}
"),
                ("Bar.cs", $@"public partial {this.Keyword} Bar
{{

}}"),
            };

            DiagnosticResult expected = this.Diagnostic().WithLocation(4, 17 + this.Keyword.Length);
            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestPreferFilenameTypeAsync()
        {
            var testCode = $@"public {this.Keyword} Foo
{{
}}
public {this.Keyword} Test0
{{
}}";

            var fixedCode = new[]
            {
                ("/0/Test0.cs", $@"public {this.Keyword} Test0
{{
}}"),
                ("Foo.cs", $@"public {this.Keyword} Foo
{{
}}
"),
            };

            DiagnosticResult expected = this.Diagnostic().WithLocation(1, 9 + this.Keyword.Length);
            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestNestedTypesAsync()
        {
            var testCode = $@"public class Foo
{{
    public {this.Keyword} Bar
    {{
    
    }}
}}";

            await this.VerifyCSharpDiagnosticAsync(testCode, this.GetSettings(), DiagnosticResult.EmptyDiagnosticResults, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        [WorkItem(3109, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3109")]
        public async Task TestCodeFixRemovesUnnecessaryUsingsInBothTypesAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
    using System;
    using System.Collections.Generic;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    using System.Collections.Generic;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{
    using System;

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepsNeededUsingsInBothTypesAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
    using System.Collections.Generic;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} List<string> Items2 {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    using System.Collections.Generic;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{
    using System.Collections.Generic;

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} List<string> Items2 {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixRemovesUnneededUsingFromFirstTypeAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
    using System;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{
    using System;

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixRemovesUnneededUsingFromSecondTypeAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
    using System;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    using System;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeWithNoUsingsAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int MyProperty {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} string MyProperty {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int MyProperty {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} string MyProperty {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepsAliasUsingInBothTypesAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
    using Col = System.Collections.Generic;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} Col::List<int> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} Col::List<string> Items2 {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    using Col = System.Collections.Generic;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} Col::List<int> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{
    using Col = System.Collections.Generic;

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} Col::List<string> Items2 {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepsAliasUsingFromFirstTypeAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
    using Col = System.Collections.Generic;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} Col::List<int> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} string Name {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    using Col = System.Collections.Generic;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} Col::List<int> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} string Name {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepsAliasUsingFromSecondTypeAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
    using Col = System.Collections.Generic;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int Id {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} Col::List<int> Items2 {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int Id {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{
    using Col = System.Collections.Generic;

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} Col::List<int> Items2 {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixRemovesAliasUsingFromBothTypesAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
    using Col = System.Collections.Generic;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} string Name {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} string Name {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepsStaticUsingInBothTypesAsync()
        {
            if (!this.SupportsStaticMemberUsageInBodies)
            {
                return;
            }

            var testCode = $@"
namespace TestNamespace
{{
    using static System.Math;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} double R1 => PI;
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} double R2 => 2 * PI;
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    using static System.Math;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} double R1 => PI;
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{
    using static System.Math;

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} double R2 => 2 * PI;
    }}
}}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepstaticUsingFromFirstTypeAsync()
        {
            if (!this.SupportsStaticMemberUsageInBodies)
            {
                return;
            }

            var testCode = $@"
namespace TestNamespace
{{
    using static System.Math;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} double R1 => PI;
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} int Id {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    using static System.Math;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} double R1 => PI;
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} int Id {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepsStaticUsingFromSecondTypeAsync()
        {
            if (!this.SupportsStaticMemberUsageInBodies)
            {
                return;
            }

            var testCode = $@"
namespace TestNamespace
{{
    using static System.Math;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} double R2 => Sqrt(4);
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{
    using static System.Math;

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} double R2 => Sqrt(4);
    }}
}}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixRemovesStaticUsingsFromBothTypesAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
    using static System.Math;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} string Name {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} string Name {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveNeededUsingsAndPreprocessorDirectivesFromBothTypesAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
#if true
    using System;
#endif
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} DateTime MyDate {{ get; set; }}
    }}
    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime MyDate2 {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
#if true
    using System;
#endif
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} DateTime MyDate {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{
#if true
    using System;

#endif
    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} DateTime MyDate2 {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveUnneededUsingsAndPreprocessorDirectivesFromBothTypesAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
#if true
    using System;
#endif
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} string Test {{ get; set; }}
    }}
    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} string Test2 {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
#if true
    using System;
#endif
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} string Test {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{
#if true
    using System;

#endif
    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} string Test2 {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveUnnecessaryUsingInFirstTypeDueToCommentAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
    using System; // Interesting comment
    using System.Collections.Generic;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    using System; // Interesting comment
    using System.Collections.Generic;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{
    using System; // Interesting comment

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveUnnecessaryUsingInSecondTypeDueToCommentAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
    using System;
    using System.Collections.Generic; // Interesting comment

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    using System.Collections.Generic; // Interesting comment

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{
    using System;
    using System.Collections.Generic; // Interesting comment

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveCommentFromNeededUsingInFirstTypeAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
    using System;
    using System.Collections.Generic;  // Interesting comment

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    using System.Collections.Generic;  // Interesting comment

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{
    using System;
    using System.Collections.Generic;  // Interesting comment

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveCommentFromNeededUsingInSecondTypeAsync()
        {
            var testCode = $@"
namespace TestNamespace
{{
    using System; // Interesting comment
    using System.Collections.Generic;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    using System; // Interesting comment
    using System.Collections.Generic;

    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{
    using System; // Interesting comment

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixRemovesUnnecessaryUsingsInBothTypes_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"
using System;
using System.Collections.Generic;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"using System.Collections.Generic;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
using System;

namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepsNeededUsingsInBothTypes_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"
using System.Collections.Generic;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} List<string> Items2 {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
using System.Collections.Generic;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
using System.Collections.Generic;

namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} List<string> Items2 {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixRemovesUnneededUsingFromFirstType_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"
using System;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
using System;

namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixRemovesUnneededUsingFromSecondType_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"
using System;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
using System;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepsAliasUsingInBothTypes_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"
using Col = System.Collections.Generic;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} Col::List<int> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} Col::List<string> Items2 {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
using Col = System.Collections.Generic;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} Col::List<int> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
using Col = System.Collections.Generic;

namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} Col::List<string> Items2 {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepsAliasUsingFromFirstType_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"
using Col = System.Collections.Generic;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} Col::List<int> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} string Name {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
using Col = System.Collections.Generic;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} Col::List<int> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} string Name {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepsAliasUsingFromSecondType_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"
using Col = System.Collections.Generic;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int Id {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} Col::List<int> Items2 {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int Id {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
using Col = System.Collections.Generic;

namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} Col::List<int> Items2 {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixRemovesAliasUsingFromBothTypes_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"
using Col = System.Collections.Generic;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} string Name {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} string Name {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepsStaticUsingInBothTypes_UsingsOutsideNamespaceAsync()
        {
            if (!this.SupportsStaticMemberUsageInBodies)
            {
                return;
            }

            var testCode = $@"
using static System.Math;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} double R1 => PI;
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} double R2 => 2 * PI;
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
using static System.Math;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} double R1 => PI;
    }}
}}
"),
        ("TestType2.cs", $@"
using static System.Math;

namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} double R2 => 2 * PI;
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepstaticUsingFromFirstType_UsingsOutsideNamespaceAsync()
        {
            if (!this.SupportsStaticMemberUsageInBodies)
            {
                return;
            }

            var testCode = $@"
using static System.Math;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} double R1 => PI;
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} int Id {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
using static System.Math;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} double R1 => PI;
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} int Id {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepsStaticUsingFromSecondType_UsingsOutsideNamespaceAsync()
        {
            if (!this.SupportsStaticMemberUsageInBodies)
            {
                return;
            }

            var testCode = $@"
using static System.Math;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} double R2 => Sqrt(4);
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
using static System.Math;

namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} double R2 => Sqrt(4);
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixRemovesStaticUsingsFromBothTypes_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"
using static System.Math;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} string Name {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} int X {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} string Name {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveNeededUsingsAndPreprocessorDirectivesFromBothTypes_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"
#if true
using System;
#endif

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} DateTime MyDate {{ get; set; }}
    }}
    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime MyDate2 {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
#if true
using System;
#endif

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} DateTime MyDate {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
#if true
using System;
#endif

namespace TestNamespace
{{
    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} DateTime MyDate2 {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveUnneededUsingsAndPreprocessorDirectivesFromBothTypes_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"
#if true
using System;
#endif

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} string Test {{ get; set; }}
    }}
    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} string Test2 {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
#if true
using System;
#endif

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} string Test {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
#if true
using System;
#endif

namespace TestNamespace
{{
    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} string Test2 {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveUnnecessaryUsingInFirstTypeDueToComment_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"
using System; // Interesting comment
using System.Collections.Generic;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
using System; // Interesting comment
using System.Collections.Generic;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
using System; // Interesting comment

namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveUnnecessaryUsingInSecondTypeDueToComment_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"
using System;
using System.Collections.Generic; // Interesting comment

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"using System.Collections.Generic; // Interesting comment

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
using System;
using System.Collections.Generic; // Interesting comment

namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveCommentFromNeededUsingInFirstType_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"
using System;
using System.Collections.Generic;  // Interesting comment

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"using System.Collections.Generic;  // Interesting comment

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
using System;
using System.Collections.Generic;  // Interesting comment

namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveCommentFromNeededUsingInSecondType_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"
using System; // Interesting comment
using System.Collections.Generic;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"
using System; // Interesting comment
using System.Collections.Generic;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
        {this.MemberModifier} List<string> Items {{ get; set; }}
    }}
}}
"),
        ("TestType2.cs", $@"
using System; // Interesting comment

namespace TestNamespace
{{

    public {this.Keyword} TestType2
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveFileHeaderInSourceFile_UsingsOutsideNamespaceAsync()
        {
            var testCode = $@"// <copyright file=""TestFile.cs"" company=""PlaceholderCompany"">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
    }}

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
";

            // TODO: Would be nice to remove the unnecessary using directive in the original file
            // TODO: Would be nice to remove the blank line before the type in the new file
            var fixedCode = new[]
            {
        ("/0/Test0.cs", $@"// <copyright file=""TestFile.cs"" company=""PlaceholderCompany"">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace TestNamespace
{{
    public {this.Keyword} TestType
    {{
    }}
}}
"),
        ("TestType2.cs", $@"// <copyright file=""TestFile.cs"" company=""PlaceholderCompany"">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;

namespace TestNamespace
{{

    public {this.Keyword} {{|#0:TestType2|}}
    {{
        {this.MemberModifier} DateTime Date {{ get; set; }}
    }}
}}
"),
            };

            var expected = new[] { this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"), };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None)
                .ConfigureAwait(false);
        }

        protected override string GetSettings()
        {
            return this.SettingsConfiguration.GetSettings(this.SettingKeyword);
        }
    }
}
