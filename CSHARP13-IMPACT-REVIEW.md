# C# 13 impact review

Source: [What's new in C# 13](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13)

C# 13 features, and the concrete impact each has on the existing StyleCop.Analyzers rule set. Findings are based on
reading the current analyzer implementations, not just the language spec, and are grounded with file/line references.
New tests belong in `StyleCop.Analyzers.Test.CSharp13` (or higher, per the "lowest project that can express the
syntax" rule in `CLAUDE.md`) unless noted otherwise. C# 13 is a released language version, so preview-only features
(e.g. the `field` contextual keyword, which requires `LangVersion=preview` rather than plain `LangVersion=13`) are
out of scope here; preview functionality is only tracked once it targets the in-progress language version (C# 15).

## Summary

| Feature | New syntax? | Rules requiring code changes | Rules needing regression tests only |
|---|---|---|---|
| Partial properties / indexers | Existing node kinds reused (`PropertyDeclaration`, `IndexerDeclaration` without a body) | SA1601, SA1605, SA1607, SA1619 (and `PartialElementDocumentationSummaryBase`) | SA1600, SA1201/SA1202/SA1206, SA1205 |
| `allows ref struct` constraint | **New syntax node**, no Lightup wrapper exists | none identified as broken, but needs a `Lightup` audit | SA1127, SA1000, SA1024, SA1013/SA1015 |
| `ref struct` implementing interfaces | Existing `BaseList` reused | none identified | SA1201 (interface member ordering), SA1206, documentation rules on ref-struct interface members |
| `params` collections | No new syntax (same `params` modifier + parameter type) | none identified | SA1611/SA1615 doc rules, SA1117/SA1116 parameter layout, SA1026 (if array-specific) |
| New `lock` object (`System.Threading.Lock`) | No new syntax, only semantic/binding change | none identified | SA1503/SA1519/SA1501 (braces around `lock`), SA1000 (`lock` keyword spacing) |
| Implicit index (`^`) in object initializers | Reuses existing `ImplicitElementAccessSyntax` + `IndexExpression` | none identified | SA1010/SA1011 (bracket spacing), SA1008/SA1009, SA1117 |

## Details

### 1. Partial properties and indexers — real gaps, need fixes + tests

This is the highest-impact change. StyleCop already has a dedicated "partial element" documentation family
(SA1600/SA1601/SA1605/SA1607/SA1618/SA1619) built around the fact that a partial method has a *declaring
declaration* (no body) and an *implementing declaration* (with body), each requiring separate treatment (e.g. a
non-primary part may use `<content>` instead of `<summary>`). C# 13 introduces the exact same declaring/implementing
split for **properties and indexers**, but the code was never extended to cover them:

- `SA1601PartialElementsMustBeDocumented` (`StyleCop.Analyzers/DocumentationRules/SA1601PartialElementsMustBeDocumented.cs:92-101`)
  registers only `SyntaxKinds.TypeDeclaration` and `SyntaxKind.MethodDeclaration`. There is no
  `PropertyDeclarationAction` / `IndexerDeclarationAction`.
- `PartialElementDocumentationSummaryBase` (`.../DocumentationRules/PartialElementDocumentationSummaryBase.cs:34-44`,
  used by SA1605 and SA1607) has the same gap — it only registers `TypeDeclaration` and `MethodDeclaration`, and its
  private `IsPartialMethodDefinition` helper (line 61) hardcodes `SyntaxKind.MethodDeclaration`.
- `SA1619GenericTypeParametersMustBeDocumentedPartialClass` only handles `TypeDeclarationSyntax` — not directly
  affected since properties/indexers can't have type parameter lists, so this one is fine as-is.

Effect today: a partial property/indexer falls through to the generic `SA1600ElementsMustBeDocumented` handlers
(`SA1600ElementsMustBeDocumented.cs:212-250`), which do **not** special-case `partial` at all (unlike
`HandleBaseTypeDeclaration`, which explicitly defers to SA1601 at line 135-139). Consequences:
  - Both the declaring and implementing declaration of a partial property are required to carry a full `<summary>`
    independently — the "second part may use `<content>`" convention that partial classes/methods get is unavailable
    for partial properties/indexers.
  - `<include>` expansion doesn't get the declaring-part-specific workaround that
    `PartialElementDocumentationSummaryBase.ExpandDocumentation` (line 168) applies for partial methods (Roslyn can't
    expand `<include>` on a declaration-only method); the same Roslyn limitation likely applies to declaration-only
    partial properties and would need the same workaround.

**Recommended fix**: extend `PartialElementDocumentationSummaryBase` and `SA1601` to also register
`SyntaxKind.PropertyDeclaration` / `SyntaxKind.IndexerDeclaration`, guarded by "has no accessor bodies/is the
declaring declaration" the same way method declarations are guarded by `Body == null`. Add unit tests in
`StyleCop.Analyzers.Test.CSharp13` mirroring the existing partial-method tests for SA1601/SA1605/SA1607, but with
`partial int Foo { get; set; }` / `partial int this[int index] { get; set; }`.

Also check, but did not confirm a bug in, **SA1205 `PartialElementsMustDeclareAccess`**
(`OrderingRules/SA1205PartialElementsMustDeclareAccess.cs:53-67`) — it only inspects `TypeDeclarationSyntax`, so it
has never covered partial methods either; this looks like a pre-existing, intentional scope limit (the rule title in
upstream StyleCop was always type-focused) rather than a new gap, but it's worth a regression test showing partial
properties are (correctly, or not) ignored by SA1205 today.

Rules that are *not* affected because they operate generically on any `MemberDeclarationSyntax`/modifiers, but
deserve a **new-syntax regression test** simply because "partial property/indexer" is a shape that never existed in
the test suite before:
  - SA1600 (generic doc-required rule — will now actually fire twice, once per part, for undocumented partial
    properties; worth locking in that behavior explicitly since it differs from partial methods only by omission,
    not by design).
  - SA1201/SA1202 element-ordering rules, and SA1206 (declaration keyword order, which already lists
    `SyntaxKind.PartialKeyword` in `ModifierOrderHelper.GetModifierType`,
    `Helpers/ModifierOrderHelper.cs:68`) — should just work, but had no test coverage for `partial` properties before.

### 2. `allows ref struct` generic constraint — new syntax with no Lightup wrapper

Confirmed by inspecting `Lightup/SyntaxKindEx.cs` (full file read) and `Lightup/Syntax.xml`: there is **no** constant
or generated wrapper for the new `AllowsConstraintClauseSyntax` / `RefStructConstraintSyntax` node that backs
`where T : allows ref struct`. Every other C# 6–13 syntax addition the repo already supports (patterns, `scoped`,
`required`, file-scoped namespaces, collection expressions, etc.) has a corresponding entry there — this one is
simply missing, most likely because it was never needed for anything (StyleCop has no rule that specifically
validates constraint *contents*), but it means any future rule work involving generic constraints must special-case
this node the same way `ClassOrStructConstraintSyntaxExtensions.cs` does for `class`/`struct`/`T?`.

No currently-shipping rule appears to break, because the rules that touch `TypeParameterConstraintClauseSyntax`
operate on the *whole clause* rather than switching over individual `Constraints` entries:
  - `SA1127GenericTypeConstraintsMustBeOnOwnLine` (`ReadabilityRules/SA1127GenericTypeConstraintsMustBeOnOwnLine.cs:47-54`)
    only looks at `syntax.WhereKeyword`, so `allows ref struct` is inert to it.
  - `SA1000KeywordsMustBeSpacedCorrectly` treats `SyntaxKind.RefKeyword` as "always require a following space"
    unconditionally for every `ref` token in the file (`SpacingRules/SA1000KeywordsMustBeSpacedCorrectly.cs:104`),
    so the `ref` in `allows ref struct` is covered incidentally, not by design.
  - `struct` (the second word of the anti-constraint) and `allows` (the contextual keyword introducing it) are not
    in SA1000's keyword switch at all, meaning today nothing enforces "no space before the comma"/"one space after
    allows" specifically for this construct — likely fine since generic whitespace/token rules (SA1001 comma
    spacing, SA1025 single-space rules) still apply to the surrounding tokens, but this has zero test coverage.

**Recommended action**: no code fix required, but add regression tests exercising
`class C<T> where T : allows ref struct { }` (and multi-constraint forms, e.g. `where T : SomeInterface, allows ref
struct`) against SA1127, SA1000, SA1001, SA1024 (colon spacing), and SA1206, to lock in that this brand-new node
shape doesn't crash or misformat under any existing rule that walks constraint clauses/tokens.

### 3. `ref struct` types implementing interfaces

No new syntax node — a `ref struct`'s `BaseList` is the same `BaseListSyntax` any class/struct uses. Rules that key
off "does this struct implement an interface" (SA1201 ordering of interface implementations, documentation rules
for interface member implementations, SA1206 modifier order for `readonly`/`ref` on struct members) should work
unmodified, but none of the current test suites exercise a `ref struct ... : ISomething` declaration or the new
"`ref struct` can have `default interface method` bodies but can only be reached through a type parameter with
`allows ref struct`" restriction. Add regression tests for:
  - SA1201/SA1202 ordering when a `ref struct` implements an interface (methods, explicit interface
    implementations).
  - Documentation rules (SA1600 family) on a `ref struct`'s explicit interface member implementations.

### 4. `params` collections (`params ReadOnlySpan<T>`, `params IEnumerable<T>`, etc.)

No new syntax — the `params` modifier is unchanged; only the *type* it can precede is more permissive. Grepped the
codebase for any rule that assumes the `params` parameter is specifically an array type
(`ParamsKeyword`/`IsParams`/array-specific checks) and found none — the one hit (`SA1130UseLambdaSyntax.cs`) is
unrelated (matches on the word "params" in a doc comment). This means no rule should misbehave, but there is also
zero existing test coverage for a `params` parameter of a non-array type. Add regression tests for:
  - SA1611/SA1615/SA1617 (parameter documentation rules) with `params ReadOnlySpan<T> items`.
  - SA1117/SA1116 (parameters on same/different lines) with a `params Span<T>`/`params IEnumerable<T>` parameter.
  - Any rule that special-cases the *last* parameter of a method (none currently identified, but worth double
    checking readability rules around method signatures).

### 5. New `lock` object (`System.Threading.Lock`)

Purely a binding/codegen change — the C# `lock` statement syntax (`LockStatementSyntax`) is unchanged whether the
locked expression is a `Lock`, a reference type, or (in `unsafe` contexts) something else. The three rules that
inspect `LockStatementSyntax` (`SA1501StatementMustNotBeOnASingleLine`, `SA1503BracesMustNotBeOmitted`,
`SA1519BracesMustNotBeOmittedFromMultiLineChildStatement`) all operate purely on the statement's block/brace shape,
not on the type of the locked expression, so no change is expected. Add a regression test locking a `System.Threading.Lock`
field through each of these three rules, plus SA1000's `LockKeyword` spacing check
(`SpacingRules/SA1000KeywordsMustBeSpacedCorrectly.cs:99`), to document that the new `Lock` type doesn't change
formatting requirements.

### 6. Implicit "from the end" index (`^`) in object initializers

The bracketed index-initializer form (`Prop = { [key] = value }`) already existed since C# 6 for indexers; C# 13
only adds the `^` operator as a legal *expression* inside that bracket. Both the outer construct
(`ImplicitElementAccessSyntax`) and the operator (`SyntaxKindEx.IndexExpression`, already present at
`Lightup/SyntaxKindEx.cs:35` since it was added for C# 8 ranges/indices) are pre-existing to the Lightup layer, so no
wrapper work is needed. Recommended regression tests: SA1010/SA1011 (square bracket spacing), SA1008/SA1009
(parenthesis spacing doesn't apply, but verify no false trigger), and SA1117/SA1500 for the surrounding initializer
block, using the exact `buffer = { [^1] = 0, [^2] = 1 }` shape from the Microsoft docs example.

## Prioritized follow-ups

1. **Fix**: extend SA1601 / `PartialElementDocumentationSummaryBase` (SA1605, SA1607) to handle
   `PropertyDeclaration`/`IndexerDeclaration` the same way they handle `MethodDeclaration`, with new tests in
   `Test.CSharp13`.
2. **Regression tests only** (safe today, just uncovered): `allows ref struct` constraints through SA1127/SA1000/SA1024;
   `ref struct : IInterface` through SA1201/SA1600; `params` non-array collections through SA1611/SA1117; the new
   `Lock` type through SA1501/SA1503/SA1519/SA1000; `[^n] = value` initializers through SA1010/SA1011.
