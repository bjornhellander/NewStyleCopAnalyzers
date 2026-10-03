// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp11.ReadabilityRules
{
    public partial class SA1129CSharp11UnitTests
    {
        /// <summary>
        /// Gets the expected fixed code for <see cref="CSharp9.ReadabilityRules.SA1129CSharp9UnitTests.VerifyNativeSizedIntegerAsync"/>.
        /// From C# 11, <c>nint</c> and <c>nuint</c> are aliases of <see cref="System.IntPtr"/> and
        /// <see cref="System.UIntPtr"/>, so they are handled the same way.
        /// </summary>
        /// <returns>The expected fixed code.</returns>
        protected override string GetExpectedFixedCodeVerifyNativeSizedInteger()
        {
            return @"class TestClass
{
    public void TestMethod()
    {
        nint a = nint.Zero;
        nuint b = nuint.Zero;
        nint c = nint.Zero;
    }
}
";
        }
    }
}
