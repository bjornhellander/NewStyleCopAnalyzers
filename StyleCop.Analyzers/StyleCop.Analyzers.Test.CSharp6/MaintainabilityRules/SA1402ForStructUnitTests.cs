// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.MaintainabilityRules
{
    public class SA1402ForStructUnitTests : SA1402ForBlockDeclarationUnitTestsBase
    {
        public override string Keyword => "struct";

        protected override bool IsConfiguredAsTopLevelTypeByDefault => false;
    }
}
