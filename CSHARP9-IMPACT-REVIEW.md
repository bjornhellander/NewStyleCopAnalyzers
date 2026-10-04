# C# 9 impact review

## Static anonymous functions

**Docs:** [Static anonymous functions](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions#static-lambdas)

### Decide SA1206 for lambda modifiers, pin SA1008 and SA1410

**Priority:** Low. **Code change:** decision needed.

`async static x => ...` is not reported. SA1206 only registers declarations (methods, types, local functions...), not
anonymous functions. Methods get "'static' should appear before 'async'"; lambdas and anonymous methods can now carry
the same two modifiers.

Confirmed working but untested:

- SA1008 reports `static(x) => x` (should be preceded by a space) and `static delegate (int x)`.
- SA1410 reports `static delegate() { }`.

**Suggested change:** if wanted, register SA1206 on `SyntaxKinds.AnonymousFunctionExpression` and run the existing
modifier-order check over the lightup `Modifiers()` from the SA1130 subsection. The SA1206 code fix would need the same
lightup to rewrite the modifiers.

**Tests:**

- `SA1008CSharp9UnitTests`: a fix test for `static(x) => x`.
- `SA1410CSharp9UnitTests` (new file): a fix test showing `static delegate() { }` → `static delegate { }`, keeping
  `static`.
- If the SA1206 change is made, an `SA1206CSharp9UnitTests` fix test for `async static x => ...`.

## Covariant return types

**Docs:** [Covariant return types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/classes#1565-override-methods)

### Pin SA1648

**Priority:** Low. **Code change:** none expected.

An override with a covariant return type is still an override, and `<inheritdoc/>` on it produces no SA1648
(confirmed). The CSharp9 test framework targets a runtime without covariant-return support (CS8830). The test therefore
needs .NET 5 reference assemblies, as the rejected SA1623 commit used (`ReferenceAssemblies = ...Net50`).

**Tests:** `SA1648CSharp9UnitTests` (new file): `public override string Get()` overriding `public virtual object Get()`
with `/// <inheritdoc/>` gives no diagnostic.

## Extension `GetEnumerator` support for `foreach` loops

**Docs:** [The foreach statement](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/iteration-statements#the-foreach-statement)

### No action

**Priority:** none.

`foreach` over a type with an extension `GetEnumerator()` is a binding change only and parses exactly as before. A
probe produced no StyleCop diagnostics. Nothing to test. Delete this section once agreed.

## Lambda discard parameters

**Docs:** [Input parameters of a lambda expression](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions#input-parameters-of-a-lambda-expression)

### Pin SA1313 and SA1130

**Priority:** Low. **Code change:** none expected.

SA1313 already exempts `_` and `__` lambda parameters
(`SA1313ParameterNamesMustBeginWithLowerCaseLetter.cs:83`). C# 9 makes repeated discards legal: `(_, _) => 0`,
`(int _, int _) => 0`, `delegate (int _, int _) { ... }`. All of these were confirmed to produce no SA1313. SA1130 still
reports the anonymous-method form.

**Tests:**

- `SA1313CSharp9UnitTests`: no diagnostic for `(_, _)`, `(int _, int _)`, `(_, _, _)` and
  `delegate (int _, int _) { }`.
- `SA1130CSharp9UnitTests` (shared with the SA1130 subsection under Static anonymous functions): a fix test showing that
  `delegate (int _, int _) { return 0; }` becomes `(_, _) => { return 0; }`. This pins that the fix keeps the discards
  and doesn't invent names through `GenerateUniqueParameterNames`.

## Attributes on local functions

**Docs:** [Local function declarations](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/statements#1364-local-function-declarations)

### Pin SA1133, SA1134, SA1137 and SA1206

**Priority:** Low. **Code change:** none expected.

All of these work today (confirmed):

- SA1134 reports `[Obsolete] static void B()` on one line.
- SA1133 reports `[Obsolete, CLSCompliant(false)]`.
- SA1137 reports a local function indented differently from its attribute.
- SA1206 reports `extern static void G();` on an `extern` local function with `[DllImport]`.

Attributes on local-function parameters and `[return: ...]` produce nothing unexpected.

**Tests:** one fix test each in `SA1133CSharp9UnitTests`, `SA1134CSharp9UnitTests` and `SA1137CSharp9UnitTests`, plus an
SA1206 test for `extern static` in `SA1206CSharp9UnitTests` (new files where missing). Put each case inside a method
body.
