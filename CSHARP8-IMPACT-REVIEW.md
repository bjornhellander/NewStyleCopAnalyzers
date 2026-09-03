# C# 8 impact review

Source: [C# version 8.0](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-version-history#c-version-80)

The C# 8 language features with StyleCop work still outstanding, the rules each one touches, and what the repo
already has. Findings come from reading the analyzer implementations and the existing
`StyleCop.Analyzers.Test.CSharp8` tests, not just the language spec, and are grounded with file/line references.

The review started from all fourteen features on the source page. A feature disappears from this file once its work
is done or once it has been confirmed to need none, so an absent feature has been dealt with, not overlooked.

New tests belong in `StyleCop.Analyzers.Test.CSharp8` (the lowest project whose language version can express the
syntax, per `CLAUDE.md`), written as `public partial class SA####CSharp8UnitTests` — the derived-test generator
supplies the other half of the partial class, and the test then re-runs in CSharp9..15 automatically.

The goal is a regression test for every related rule, including the ones that turn out to need no code change; a test
that pins current correct behaviour is the point, not a formality.

Work through the numbered items one at a time. Delete an item from this file once its tests are merged. Item numbers
are stable — deleting item 6 leaves a gap rather than renumbering, so "item 6" means the same thing across
conversations. Items are ordered by priority, and each states its own; priority reflects how likely the rule is to
behave wrongly today, not how much typing the test needs. Each item links the language documentation the source page
points at for that feature.

## Items

### 1. Switch expressions — SA1000 is unverified, layout rules untested

**Priority:** High. **Suspected code gap:** SA1000. **Docs:** [Pattern matching enhancements](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/patterns)

`SA1119` and `SA1413` are well covered (11 and 1 tests). The gap is everything around the *layout* of a switch
expression, plus the keyword itself.

`SA1000KeywordsMustBeSpacedCorrectly` routes `SyntaxKind.SwitchKeyword` to `HandleRequiredSpaceToken`
(`StyleCop.Analyzers/StyleCop.Analyzers/SpacingRules/SA1000KeywordsMustBeSpacedCorrectly.cs:112-118`). That rule was
written when `switch` could only start a statement, where the keyword is *followed* by `(`. In a switch expression the
keyword is *preceded* by the governing expression and followed by `{`, so `x switch{...}` and `x  switch {...}` need a
decision and a test. `SA1000CSharp8UnitTests.cs` now exists but covers only `stackalloc` and `sizeof`, nothing about
switch expressions.

Also untested: `=>` inside switch arms (`SA1003`), the brace layout of the arm list (`SA1500`, `SA1501`, `SA1505`,
`SA1506`, `SA1508`), arm indentation (`SA1137`) and arms sharing a line (`SA1136`).

Worth confirming while here: no analyzer references `SyntaxKindEx.SwitchExpressionArm` or `SyntaxKindEx.Subpattern`.
That may be correct — or it may be why the layout rules ignore arms.

**Proposed:** switch expression keyword spacing added to `SA1000CSharp8UnitTests`, plus `SA1003`, `SA1136`, `SA1137`,
`SA1500`, `SA1501`, `SA1506` and `SA1508` C# 8 files covering a multi-line switch expression and a single-line one,
plus the same added to the existing `SA1505CSharp8UnitTests`.
