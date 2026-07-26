// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.Lightup
{
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using StyleCop.Analyzers.Lightup;
    using Xunit;

    public class MethodKindExUnitTests
    {
        private static readonly Dictionary<MethodKind, string> MethodKindToName;
        private static readonly Dictionary<string, MethodKind> NameToMethodKind;

        static MethodKindExUnitTests()
        {
            MethodKindToName = new Dictionary<MethodKind, string>();
            NameToMethodKind = new Dictionary<string, MethodKind>();

            foreach (var field in typeof(MethodKind).GetTypeInfo().DeclaredFields)
            {
                if (!field.IsStatic)
                {
                    continue;
                }

                var value = (MethodKind)field.GetRawConstantValue();
                var name = field.Name;
                if (!MethodKindToName.ContainsKey(value))
                {
                    MethodKindToName[value] = name;
                }

                NameToMethodKind.Add(name, value);
            }
        }

        public static TheoryData<string, MethodKind> MethodKinds
        {
            get
            {
                var data = new TheoryData<string, MethodKind>();
                foreach (var field in typeof(MethodKindEx).GetTypeInfo().DeclaredFields)
                {
                    data.Add(field.Name, (MethodKind)field.GetRawConstantValue());
                }

                return data;
            }
        }

        [Theory]
        [MemberData(nameof(MethodKinds))]
        public void TestMethodKind(string name, MethodKind methodKind)
        {
            if (MethodKindToName.TryGetValue(methodKind, out var expectedName))
            {
                Assert.Equal(expectedName, name);
            }
            else
            {
                Assert.False(NameToMethodKind.TryGetValue(name, out _));
            }
        }
    }
}
