# C# 9 impact review

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
