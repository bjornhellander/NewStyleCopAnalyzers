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

### 2. Indices and ranges — SA1010 has no range or index tests

**Priority:** High. **Suspected code gap:** none, but the rule is entirely unexercised for this syntax. **Docs:** [Indices and ranges](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#range-operator-)

`SA1003`, `SA1008`, `SA1009`, `SA1011` and `SA1119` all have range tests.
`SA1010OpeningSquareBracketsMustBeSpacedCorrectly` does not: `SA1010CSharp8UnitTests.cs` exists but covers only
`stackalloc`, and no test anywhere exercises index-from-end or range arguments. The rule
has grown special cases for index initializers, list patterns and collection expressions
(`.../SpacingRules/SA1010OpeningSquareBracketsMustBeSpacedCorrectly.cs:97,115-128`) but nothing for either of those.

**Proposed:** add to `SA1010CSharp8UnitTests`, covering `x[^1]`, `x[1..2]`, `x[..^1]`, `x [^1]` (diagnostic) and the
same inside a nested expression.

### 3. Static local functions — SA1206 does not see local functions

**Priority:** High. **Suspected code gap:** SA1206. **Docs:** [Static local functions](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/local-functions)

`SA1206DeclarationKeywordsMustFollowOrder` registers a fixed list of declaration kinds
(`.../OrderingRules/SA1206DeclarationKeywordsMustFollowOrder.cs:49-66`) which does **not** include
`LocalFunctionStatement`. A local function may carry `static`, `async`, `extern` and `unsafe`, so `async static void
Local()` is not flagged today while the equivalent method declaration would be.

Whether that is a bug or an intentional scope limit is the open question — `SA1206`'s upstream description is
declaration-focused, and local functions have no access modifiers, so only the static/async ordering is at stake.

Other rules already handle local functions and only need a `static` regression test: `SA1502`
(`.../SA1502ElementMustNotBeOnASingleLine.cs`), `SA1300`, and the parameter-list family `SA1110`–`SA1117`, all of
which reference `LocalFunctionStatement` today.

**Proposed:** decide SA1206's scope, then `SA1206CSharp8UnitTests` (either the fix plus tests, or a test pinning that
local functions are ignored), plus `SA1502` and `SA1300` tests using `static` local functions.

### 4. Default interface members — SA1400 deliberately skips interfaces

**Priority:** High. **Suspected code gap:** SA1400. **Docs:** [Default interface members](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface#default-interface-members)

`SA1400AccessModifierMustBeDeclared` bails out whenever a member's parent is an interface
(`.../MaintainabilityRules/SA1400AccessModifierMustBeDeclared.cs:90,106,122,133,162`). Before C# 8 that was
unambiguously right: interface members could not have access modifiers. From C# 8 they can, and a `private` or
`static private` helper in an interface *must* be explicit. So the rule is silent on a construct it arguably now
covers.

I'd lean towards "current behaviour is still defensible" — the rule's job is to make implicit access explicit, and
interface members are still implicitly `public` — but it needs a test that states the choice rather than leaving it
undefined.

Existing coverage is thin: `SA1202CSharp8UnitTests.TestPropertiesOfInterfaceAsync` and
`SA1648CSharp8UnitTests.TestIncorrectMemberInheritDocFromStaticMemberInInterfaceAsync`. `SA1600CSharp8UnitTests.cs`
exists but adds no test.

**Proposed:** `SA1400CSharp8UnitTests` pinning the interface behaviour, plus tests for `SA1101` (`this.` inside a
default implementation), `SA1201`/`SA1204` (ordering of static, const, field and default-implemented members in an
interface), `SA1502` (single-line default body) and `SA1600`/`SA1601` (documentation of members with bodies).

### 5. `await foreach` and async streams

**Priority:** Medium. **Suspected code gap:** none. **Docs:** [Asynchronous streams](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/iteration-statements#await-foreach)

`SA1316TupleElementNamesShouldUseCorrectCasing` is the only analyzer referencing
`CommonForEachStatementSyntaxWrapper`, and it has no C# 8 test. `SA1000` handles `AwaitKeyword` and `ForEachKeyword`
separately (`SA1000KeywordsMustBeSpacedCorrectly.cs:90,95`) but has never seen them adjacent.

**Proposed:** `SA1316CSharp8UnitTests` (`await foreach (var (a, b) in ...)` with tuple element names), and
`await foreach` cases in the new `SA1000CSharp8UnitTests`; plus `SA1101` and `SA1503` regressions for the loop body.

### 6. `await using` and using declarations

**Priority:** Medium. **Suspected code gap:** none. **Docs:** [Using declarations](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/using)

`SA1503` and `SA1106` already cover the using *declaration* form. Not covered: the `await using` combination, and
`SA1000`'s treatment of `UsingKeyword` in a declaration (`SA1000KeywordsMustBeSpacedCorrectly.cs:113`), where the
keyword is followed by a type or `var` rather than `(`. The source page has no separate entry for asynchronous
disposal; the linked `using` documentation covers `await using` too.

**Proposed:** `await using` and `using var` cases in `SA1000CSharp8UnitTests`; `SA1002` semicolon test for a using
declaration.

### 7. Readonly instance members

**Priority:** Medium. **Suspected code gap:** none. **Docs:** [Readonly members](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct#readonly-instance-members)

No C# 8 tests exist for `readonly` struct members. `SA1206` handles the relevant declaration kinds already, and
classifies `readonly` as an "other" modifier, so `readonly public int Foo()` should be flagged and `public readonly
int Foo()` should not — untested either way.

`SA1214` orders readonly *fields* and should be unaffected by readonly *members*; that is worth a negative test so a
future change to the modifier handling cannot silently break it.

**Proposed:** `SA1206CSharp8UnitTests` (shared with item 3), plus `SA1201`/`SA1202`/`SA1204`/`SA1214` regressions on a
struct with readonly members.

### 8. Positional and property patterns

**Priority:** Medium. **Suspected code gap:** none. **Docs:** [Pattern matching enhancements](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/patterns)

Spacing is covered (`SA1008` has three pattern tests, `SA1012`, `SA1013`, `SA1024`, `SA1101`, `SA1122`). The
parameter-list-shaped rules are not: a positional pattern `(int x, int y)` looks like a parameter list to
`SA1111`–`SA1117`, and `SA1141UseTupleSyntax` may or may not have an opinion about tuple patterns. That resemblance is
a hypothesis, not something confirmed against the code — check it before writing the tests.

**Proposed:** C# 8 files for `SA1111`, `SA1112`, `SA1113`, `SA1115`, `SA1116`, `SA1117` with a multi-line positional
pattern, and an `SA1141` regression on a tuple pattern.
