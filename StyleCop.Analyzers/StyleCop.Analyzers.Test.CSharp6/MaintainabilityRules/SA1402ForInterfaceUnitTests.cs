// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.MaintainabilityRules
{
    public class SA1402ForInterfaceUnitTests : SA1402ForBlockDeclarationUnitTestsBase
    {
        public override string Keyword => "interface";

        protected override bool IsConfiguredAsTopLevelTypeByDefault => false;

        protected override string MemberModifier => string.Empty;

        protected override bool SupportsStaticMemberUsageInBodies => false;
    }
}
