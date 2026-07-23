// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Test.CSharp6.Helpers
{
    using System.Linq;
    using StyleCop.Analyzers.Lightup;
    using Xunit;

    public static class CommonMemberData
    {
        public static TheoryData<string> DataTypeDeclarationKeywords
        {
            get
            {
                var data = new TheoryData<string>()
                {
                    "class",
                    "struct",
                };

                if (LightupHelpers.SupportsCSharp9)
                {
                    data.Add("record");
                }

                if (LightupHelpers.SupportsCSharp10)
                {
                    data.Add("record class");
                    data.Add("record struct");
                }

                return data;
            }
        }

        public static TheoryData<string> ReferenceTypeDeclarationKeywords
        {
            get
            {
                var data = new TheoryData<string>()
                {
                    "class",
                };

                if (LightupHelpers.SupportsCSharp9)
                {
                    data.Add("record");
                }

                if (LightupHelpers.SupportsCSharp10)
                {
                    data.Add("record class");
                }

                return data;
            }
        }

        public static TheoryData<string> ValueTypeDeclarationKeywords
        {
            get
            {
                var data = new TheoryData<string>()
                {
                    "struct",
                };

                if (LightupHelpers.SupportsCSharp10)
                {
                    data.Add("record struct");
                }

                return data;
            }
        }

        public static TheoryData<string> RecordTypeDeclarationKeywords
        {
            get
            {
                var data = new TheoryData<string>();

                if (LightupHelpers.SupportsCSharp9)
                {
                    data.Add("record");
                }

                if (LightupHelpers.SupportsCSharp10)
                {
                    data.Add("record class");
                    data.Add("record struct");
                }

                return data;
            }
        }

        public static TheoryData<string> TypeDeclarationKeywords
        {
            get
            {
                return new(DataTypeDeclarationKeywords
                    .Concat(new TheoryData<string> { "interface" }));
            }
        }

        public static TheoryData<string> BaseTypeDeclarationKeywords
        {
            get
            {
                return new(TypeDeclarationKeywords
                    .Concat(new TheoryData<string> { "enum" }));
            }
        }

        public static TheoryData<string> AllTypeDeclarationKeywords
        {
            get
            {
                return new(BaseTypeDeclarationKeywords
                    .Concat(new TheoryData<string> { "delegate" }));
            }
        }

        public static TheoryData<string> GenericTypeDeclarationKeywords
        {
            get
            {
                return new(TypeDeclarationKeywords
                    .Concat(new TheoryData<string> { "delegate" }));
            }
        }

        public static TheoryData<string> ReferenceTypeKeywordsWhichSupportPrimaryConstructors
        {
            get
            {
                var data = new TheoryData<string>();

                if (LightupHelpers.SupportsCSharp9)
                {
                    data.Add("record");
                }

                if (LightupHelpers.SupportsCSharp10)
                {
                    data.Add("record class");
                }

                if (LightupHelpers.SupportsCSharp12)
                {
                    data.Add("class");
                }

                return data;
            }
        }

        public static TheoryData<string> TypeKeywordsWhichSupportPrimaryConstructors
        {
            get
            {
                var data = new TheoryData<string>(ReferenceTypeKeywordsWhichSupportPrimaryConstructors);

                if (LightupHelpers.SupportsCSharp10)
                {
                    data.Add("record struct");
                }

                if (LightupHelpers.SupportsCSharp12)
                {
                    data.Add("struct");
                }

                return data;
            }
        }
    }
}
