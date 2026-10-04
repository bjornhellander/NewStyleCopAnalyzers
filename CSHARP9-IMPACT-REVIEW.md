# C# 9 impact review

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
