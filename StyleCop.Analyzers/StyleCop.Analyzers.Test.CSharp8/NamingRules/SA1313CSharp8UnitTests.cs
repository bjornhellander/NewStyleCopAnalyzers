// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp8.NamingRules
{
    using Microsoft.CodeAnalysis.Testing;

    public partial class SA1313CSharp8UnitTests
    {
        protected override DiagnosticResult[] GetInvalidMethodOverrideShouldNotProduceDiagnosticAsyncDiagnostics()
        {
            return new DiagnosticResult[]
            {
                DiagnosticResult.CompilerError("CS0534").WithLocation(9, 18).WithMessage("'TestClass' does not implement inherited abstract member 'BaseClass.TestMethod(int, int)'"),
                DiagnosticResult.CompilerError("CS0246").WithLocation(11, 49).WithMessage("The type or namespace name 'X' could not be found (are you missing a using directive or an assembly reference?)"),
                DiagnosticResult.CompilerError("CS1001").WithLocation(11, 51).WithMessage("Identifier expected"),
                DiagnosticResult.CompilerError("CS1003").WithLocation(11, 51).WithMessage("Syntax error, ',' expected"),
            };
        }
    }
}
