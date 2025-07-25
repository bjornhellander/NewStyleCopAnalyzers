﻿// Copyright (c) Contributors to the New StyleCop Analyzers project.
// Licensed under the MIT License. See LICENSE in the project root for license information.

#nullable disable

namespace StyleCop.Analyzers.Helpers
{
    using System;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using StyleCop.Analyzers.Helpers.ObjectPools;

    /// <summary>
    /// Helper class used for working with file headers.
    /// </summary>
    internal static class FileHeaderHelpers
    {
        /// <summary>
        /// Parses a comment-only file header.
        /// </summary>
        /// <param name="root">The root of the syntax tree.</param>
        /// <returns>The copyright string, as parsed from the file header.</returns>
        internal static FileHeader ParseFileHeader(SyntaxNode root)
        {
            var firstToken = root.GetFirstToken(includeZeroWidth: true);
            var firstNonWhitespaceTrivia = TriviaHelper.IndexOfFirstNonWhitespaceTrivia(firstToken.LeadingTrivia, true);

            if (firstNonWhitespaceTrivia == -1)
            {
                return FileHeader.MissingFileHeader;
            }

            var sb = StringBuilderPool.Allocate();
            var endOfLineCount = 0;
            var done = false;
            var fileHeaderStart = int.MaxValue;
            var fileHeaderEnd = int.MinValue;

            for (var i = firstNonWhitespaceTrivia; !done && (i < firstToken.LeadingTrivia.Count); i++)
            {
                var trivia = firstToken.LeadingTrivia[i];

                switch (trivia.Kind())
                {
                case SyntaxKind.WhitespaceTrivia:
                    endOfLineCount = 0;
                    break;
                case SyntaxKind.SingleLineCommentTrivia:
                    endOfLineCount = 0;

                    var commentString = trivia.ToFullString();

                    fileHeaderStart = Math.Min(trivia.FullSpan.Start, fileHeaderStart);
                    fileHeaderEnd = trivia.FullSpan.End;

                    sb.Append(commentString.Substring(2).Trim()).Append('\n');
                    break;
                case SyntaxKind.MultiLineCommentTrivia:
                    // only process a MultiLineCommentTrivia if no SingleLineCommentTrivia have been processed
                    if (sb.Length == 0)
                    {
                        var triviaString = trivia.ToFullString();

                        var startIndex = triviaString.IndexOf("/*", StringComparison.Ordinal) + 2;
                        var endIndex = triviaString.LastIndexOf("*/", StringComparison.Ordinal);
                        if (endIndex == -1)
                        {
                            // While editing, it is possible to have a multiline comment trivia that does not contain the closing '*/' yet.
                            return FileHeader.MissingFileHeader;
                        }

                        var commentContext = triviaString.Substring(startIndex, endIndex - startIndex).Trim();

                        var triviaStringParts = commentContext.Replace("\r\n", "\n").Split('\n');

                        foreach (var part in triviaStringParts)
                        {
                            var trimmedPart = part.TrimStart(' ', '*');
                            sb.Append(trimmedPart).Append('\n');
                        }

                        fileHeaderStart = trivia.FullSpan.Start;
                        fileHeaderEnd = trivia.FullSpan.End;
                    }

                    done = true;
                    break;
                case SyntaxKind.EndOfLineTrivia:
                    endOfLineCount++;
                    done = endOfLineCount > 1;
                    break;
                default:
                    done = (fileHeaderStart < fileHeaderEnd) || !trivia.IsDirective;
                    break;
                }
            }

            if (fileHeaderStart > fileHeaderEnd)
            {
                StringBuilderPool.Free(sb);
                return FileHeader.MissingFileHeader;
            }

            if (sb.Length > 0)
            {
                // remove the final newline, which is always in '\n' form
                const int EolLength = 1;
                sb.Remove(sb.Length - EolLength, EolLength);
            }

            return new FileHeader(StringBuilderPool.ReturnAndFree(sb), fileHeaderStart, fileHeaderEnd);
        }

        /// <summary>
        /// Parses an XML-based file header.
        /// </summary>
        /// <param name="root">The root of the syntax tree.</param>
        /// <returns>The parsed file header.</returns>
        internal static XmlFileHeader ParseXmlFileHeader(SyntaxNode root)
        {
            var firstToken = root.GetFirstToken(includeZeroWidth: true);
            string xmlString;
            int fileHeaderStart;
            int fileHeaderEnd;

            var firstNonWhitespaceTrivia = TriviaHelper.IndexOfFirstNonWhitespaceTrivia(firstToken.LeadingTrivia, true);
            if (firstNonWhitespaceTrivia == -1)
            {
                return XmlFileHeader.MissingFileHeader;
            }

            switch (firstToken.LeadingTrivia[firstNonWhitespaceTrivia].Kind())
            {
            case SyntaxKind.SingleLineCommentTrivia:
                xmlString = ProcessSingleLineCommentsHeader(firstToken.LeadingTrivia, firstNonWhitespaceTrivia, out fileHeaderStart, out fileHeaderEnd);
                break;

            case SyntaxKind.MultiLineCommentTrivia:
                xmlString = ProcessMultiLineCommentsHeader(firstToken.LeadingTrivia[firstNonWhitespaceTrivia], out fileHeaderStart, out fileHeaderEnd);
                break;

            default:
                return XmlFileHeader.MissingFileHeader;
            }

            if (fileHeaderStart > fileHeaderEnd)
            {
                return XmlFileHeader.MissingFileHeader;
            }

            try
            {
                var parsedFileHeaderXml = XElement.Parse(xmlString);

                // a header without any XML tags is malformed.
                if (!parsedFileHeaderXml.Descendants().Any())
                {
                    return XmlFileHeader.MalformedFileHeader;
                }

                return new XmlFileHeader(parsedFileHeaderXml, fileHeaderStart, fileHeaderEnd);
            }
            catch (XmlException)
            {
                return XmlFileHeader.MalformedFileHeader;
            }
        }

        private static string ProcessSingleLineCommentsHeader(SyntaxTriviaList triviaList, int startIndex, out int fileHeaderStart, out int fileHeaderEnd)
        {
            var sb = StringBuilderPool.Allocate();
            var endOfLineCount = 0;
            var done = false;

            fileHeaderStart = int.MaxValue;
            fileHeaderEnd = int.MinValue;

            // wrap the XML from the file header in a single root element to make XML parsing work.
            sb.Append("<root>\n");

            int i;
            for (i = startIndex; !done && (i < triviaList.Count); i++)
            {
                var trivia = triviaList[i];

                switch (trivia.Kind())
                {
                case SyntaxKind.WhitespaceTrivia:
                    endOfLineCount = 0;
                    break;

                case SyntaxKind.SingleLineCommentTrivia:
                    endOfLineCount = 0;

                    var commentString = trivia.ToFullString();

                    // ignore borders
                    if (commentString.StartsWith("//-", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }

                    fileHeaderStart = Math.Min(trivia.FullSpan.Start, fileHeaderStart);
                    fileHeaderEnd = trivia.FullSpan.End;

                    sb.Append(commentString.Substring(2)).Append('\n');
                    break;

                case SyntaxKind.EndOfLineTrivia:
                    endOfLineCount++;
                    done = endOfLineCount > 1;
                    break;

                default:
                    done = (fileHeaderStart < fileHeaderEnd) || !trivia.IsDirective;
                    break;
                }
            }

            sb.Append("</root>\n");
            return StringBuilderPool.ReturnAndFree(sb);
        }

        private static string ProcessMultiLineCommentsHeader(SyntaxTrivia multiLineComment, out int fileHeaderStart, out int fileHeaderEnd)
        {
            var sb = StringBuilderPool.Allocate();

            // wrap the XML from the file header in a single root element to make XML parsing work.
            sb.Append("<root>\n");

            fileHeaderStart = multiLineComment.FullSpan.Start;
            fileHeaderEnd = multiLineComment.FullSpan.End;

            var rawCommentString = multiLineComment.ToFullString();
            var commentText = rawCommentString.Substring(2, rawCommentString.Length - 4);
            var commentLines = commentText.Replace("\r\n", "\n").Split('\n');

            /* TODO: Ignore borders ??? */

            foreach (var commentLine in commentLines)
            {
                sb.Append(commentLine.TrimStart(' ', '*')).Append('\n');
            }

            sb.Append("</root>\n");
            return StringBuilderPool.ReturnAndFree(sb);
        }
    }
}
