// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.Lightup
{
    using System;
    using Microsoft.CodeAnalysis;
    using StyleCop.Analyzers.Lightup;
    using Xunit;

    public class ITypeParameterSymbolExtensionsTests
    {
        [Fact]
        public void TestNull()
        {
            ITypeParameterSymbol? symbol = null;
            Assert.Throws<NullReferenceException>(() => ITypeParameterSymbolExtensions.HasUnmanagedTypeConstraint(symbol));
        }
    }
}
