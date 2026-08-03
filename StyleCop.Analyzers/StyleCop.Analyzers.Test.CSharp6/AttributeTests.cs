// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6
{
    using Xunit;

    public class AttributeTests
    {
        [Fact]
        public void TestNoCodeFixAttributeReason()
        {
            string reason = "Reason";
            var attribute = new NoCodeFixAttribute(reason);
            Assert.Same(reason, attribute.Reason);
        }

        [Fact]
        public void TestNoDiagnosticAttributeReason()
        {
            string reason = "Reason";
            var attribute = new NoDiagnosticAttribute(reason);
            Assert.Same(reason, attribute.Reason);
        }
    }
}
