// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.MaintainabilityRules
{
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class SA1402ForClassUnitTests : SA1402ForBlockDeclarationUnitTestsBase
    {
        public override string Keyword => "class";

        protected override bool IsConfiguredAsTopLevelTypeByDefault => true;

        [Fact]
        [WorkItem(3109, "https://github.com/DotNetAnalyzers/StyleCopAnalyzers/issues/3109")]
        public async Task TestCodeFixRemovesUnnecessaryUsingsAsync()
        {
            var testCode = @"
namespace TestNamespace
{
    using System;
    using System.Collections.Generic;

    public class TestClass
    {
        public List<string> Items { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public DateTime Date { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
namespace TestNamespace
{
    using System;
    using System.Collections.Generic;

    public class TestClass
    {
        public List<string> Items { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
namespace TestNamespace
{
    using System;

    public class TestClass2
    {
        public DateTime Date { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepsNeededUsingsAsync()
        {
            var testCode = @"
namespace TestNamespace
{
    using System.Collections.Generic;

    public class TestClass
    {
        public List<string> Items { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public List<string> Items2 { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
namespace TestNamespace
{
    using System.Collections.Generic;

    public class TestClass
    {
        public List<string> Items { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
namespace TestNamespace
{
    using System.Collections.Generic;

    public class TestClass2
    {
        public List<string> Items2 { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixRemovesUnnecessaryUsingsFromSecondFileOnlyAsync()
        {
            var testCode = @"
namespace TestNamespace
{
    using System.Collections.Generic;

    public class TestClass
    {
        public string Items { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public string Items2 { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
namespace TestNamespace
{
    using System.Collections.Generic;

    public class TestClass
    {
        public string Items { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
namespace TestNamespace
{

    public class TestClass2
    {
        public string Items2 { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeWithNoUsingsAsync()
        {
            var testCode = @"
namespace TestNamespace
{
    public class TestClass
    {
        public int MyProperty { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public string MyProperty { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
namespace TestNamespace
{
    public class TestClass
    {
        public int MyProperty { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
namespace TestNamespace
{

    public class TestClass2
    {
        public string MyProperty { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveUsingsAndPreprocessorDirectivesFromSecondFileAsync()
        {
            var testCode = @"
namespace TestNamespace
{
#if true
    using System;
#endif

    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public DateTime MyDate2 { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
namespace TestNamespace
{
#if true
    using System;
#endif

    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
namespace TestNamespace
{
#if true
    using System;

#endif

    public class TestClass2
    {
        public DateTime MyDate2 { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveUsingsAndPreprocessorDirectivesWithElifFromSecondFileAsync()
        {
            var testCode = @"
namespace TestNamespace
{
#if false
    using System;
#elif true
    using System;
#endif

    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public DateTime MyDate2 { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
namespace TestNamespace
{
#if false
    using System;
#elif true
    using System;
#endif

    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
namespace TestNamespace
{
#if false
    using System;
#elif true
    using System;

#endif

    public class TestClass2
    {
        public DateTime MyDate2 { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixRemovesUnnecessaryUsingsWithPreprocessorDirectivesFromSecondFileAsync()
        {
            var testCode = @"
namespace TestNamespace
{
#if true
    using System;
#endif

    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public string Test { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
namespace TestNamespace
{
#if true
    using System;
#endif

    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
namespace TestNamespace
{

#if true

#endif

    public class TestClass2
    {
        public string Test { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixRemovesUnnecessaryUsingsWithPreprocessorDirectivesWithElifFromSecondFileAsync()
        {
            var testCode = @"
namespace TestNamespace
{
#if false
    using System;

#elif true
    using System;
#endif    

    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public string Test { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
namespace TestNamespace
{
#if false
    using System;

#elif true
    using System;
#endif    

    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
namespace TestNamespace
{

#if false
#elif true

#endif    

    public class TestClass2
    {
        public string Test { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
                this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixWithAliasUsingAsync()
        {
            var testCode = @"
namespace TestNamespace
{
    using System;
    using Col = System.Collections.Generic;

    public class TestClass
    {
        public Col::List<string> Items { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public DateTime Date { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
namespace TestNamespace
{
    using System;
    using Col = System.Collections.Generic;

    public class TestClass
    {
        public Col::List<string> Items { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
namespace TestNamespace
{
    using System;

    public class TestClass2
    {
        public DateTime Date { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixWithStaticUsingAsync()
        {
            var testCode = @"
namespace TestNamespace
{
    using static System.Math;

    public class TestClass
    {
        public double GetPi() => PI;
    }

    public class {|#0:TestClass2|}
    {
        public string Name { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
namespace TestNamespace
{
    using static System.Math;

    public class TestClass
    {
        public double GetPi() => PI;
    }
}
"),
        ("TestClass2.cs", @"
namespace TestNamespace
{

    public class TestClass2
    {
        public string Name { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixSplitsAliasUsingCorrectlyAsync()
        {
            var testCode = @"
namespace TestNamespace
{
    using System;
    using Col = System.Collections.Generic;

    public class TestClass
    {
        public DateTime Date { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public Col::List<int> Items { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
namespace TestNamespace
{
    using System;
    using Col = System.Collections.Generic;

    public class TestClass
    {
        public DateTime Date { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
namespace TestNamespace
{
    using Col = System.Collections.Generic;

    public class TestClass2
    {
        public Col::List<int> Items { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixSplitsStaticUsingCorrectlyAsync()
        {
            var testCode = @"
namespace TestNamespace
{
    using System;
    using static System.Math;

    public class TestClass
    {
        public DateTime Date { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public double Radius => PI * 2;
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
namespace TestNamespace
{
    using System;
    using static System.Math;

    public class TestClass
    {
        public DateTime Date { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
namespace TestNamespace
{
    using static System.Math;

    public class TestClass2
    {
        public double Radius => PI * 2;
    }
}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        // Same tests but with usings outside of namespace
        [Fact]
        public async Task TestCodeFixRemovesUnnecessaryUsingsOutsideOfNamespaceAsync()
        {
            var testCode = @"
using System;
using System.Collections.Generic;

namespace TestNamespace
{
    public class TestClass
    {
        public List<string> Items { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public DateTime Date { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
using System;
using System.Collections.Generic;

namespace TestNamespace
{
    public class TestClass
    {
        public List<string> Items { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
using System;

namespace TestNamespace
{

    public class TestClass2
    {
        public DateTime Date { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixKeepsNeededUsingsOutsideOfNamespaceAsync()
        {
            var testCode = @"
using System.Collections.Generic;

namespace TestNamespace
{
    public class TestClass
    {
        public List<string> Items { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public List<string> Items2 { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
using System.Collections.Generic;

namespace TestNamespace
{
    public class TestClass
    {
        public List<string> Items { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
using System.Collections.Generic;

namespace TestNamespace
{

    public class TestClass2
    {
        public List<string> Items2 { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveUsingsOutsideOfNamespaceWithPreprocessorDirectivesFromSecondFileAsync()
        {
            var testCode = @"
#if true
using System;
#endif

namespace TestNamespace
{
    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public DateTime MyDate2 { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
#if true
using System;
#endif

namespace TestNamespace
{
    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
#if true
using System;
#endif

namespace TestNamespace
{

    public class TestClass2
    {
        public DateTime MyDate2 { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixDoesNotRemoveUsingsOutsideOfNamespaceWithPreprocessorDirectivesWithElifFromSecondFileAsync()
        {
            var testCode = @"
#if false
using System;
#elif true
using System;
#endif

namespace TestNamespace
{
    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public DateTime MyDate2 { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
#if false
using System;
#elif true
using System;
#endif

namespace TestNamespace
{
    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
#if false
using System;
#elif true
using System;
#endif

namespace TestNamespace
{

    public class TestClass2
    {
        public DateTime MyDate2 { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixRemovesUnnecessaryOutsideOfNamespaceUsingsWithPreprocessorDirectivesFromSecondFileAsync()
        {
            var testCode = @"
#if true
using System;
#endif

namespace TestNamespace
{
    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public string Test { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
#if true
using System;
#endif

namespace TestNamespace
{
    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
#if true
#endif

namespace TestNamespace
{

    public class TestClass2
    {
        public string Test { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixRemovesUnnecessaryUsingsOutsideOfNamespaceWithPreprocessorDirectivesWithElifFromSecondFileAsync()
        {
            var testCode = @"
#if false
using System;

#elif true
using System;
#endif    

namespace TestNamespace
{
    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public string Test { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
#if false
using System;

#elif true
using System;
#endif    

namespace TestNamespace
{
    public class TestClass
    {
        public DateTime MyDate { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
#if false
#elif true
#endif    

namespace TestNamespace
{

    public class TestClass2
    {
        public string Test { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixWithAliasUsingOutsideOfNamespaceAsync()
        {
            var testCode = @"
using System;
using Col = System.Collections.Generic;

namespace TestNamespace
{
    public class TestClass
    {
        public Col::List<string> Items { get; set; }
    }

    public class {|#0:TestClass2|}
    {
        public DateTime Date { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
using System;
using Col = System.Collections.Generic;

namespace TestNamespace
{
    public class TestClass
    {
        public Col::List<string> Items { get; set; }
    }
}
"),
        ("TestClass2.cs", @"
using System;

namespace TestNamespace
{

    public class TestClass2
    {
        public DateTime Date { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }

        [Fact]
        public async Task TestCodeFixWithStaticUsingOutsideOfNamespaceAsync()
        {
            var testCode = @"
using static System.Math;

namespace TestNamespace
{
    public class TestClass
    {
        public double GetPi() => PI;
    }

    public class {|#0:TestClass2|}
    {
        public string Name { get; set; }
    }
}
";

            var fixedCode = new[]
            {
        ("/0/Test0.cs", @"
using static System.Math;

namespace TestNamespace
{
    public class TestClass
    {
        public double GetPi() => PI;
    }
}
"),
        ("TestClass2.cs", @"
namespace TestNamespace
{

    public class TestClass2
    {
        public string Name { get; set; }
    }
}
"),
            };

            var expected = new[]
            {
        this.Diagnostic().WithLocation(0).WithArguments("not", "preceded"),
            };

            await this.VerifyCSharpFixAsync(testCode, this.GetSettings(), expected, fixedCode, CancellationToken.None).ConfigureAwait(false);
        }
    }
}
