﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Test.CSharp8.Lightup
{
    using StyleCop.Analyzers.Lightup;
    using StyleCop.Analyzers.Test.CSharp7.Lightup;
    using StyleCop.Analyzers.Test.Lightup;

    /// <summary>
    /// This class tests edge case behavior of <see cref="LightupHelpers"/> in Roslyn 2+. It extends
    /// <see cref="LightupHelpersCSharp7UnitTests"/> since the tests defined there are valid in both environments without
    /// alteration.
    /// </summary>
    public partial class LightupHelpersCSharp8UnitTests : LightupHelpersCSharp7UnitTests
    {
    }
}
