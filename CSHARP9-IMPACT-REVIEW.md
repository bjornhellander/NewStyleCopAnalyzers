# C# 9 impact review

## Pattern matching enhancements

**Docs:** [Relational patterns](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/patterns#relational-patterns), [Logical patterns](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/patterns#logical-patterns)

SA1000, SA1008 and SA1012 already have CSharp9 tests for `and`/`or`/`not` and for parenthesized patterns.

### Parenthesized patterns: SA1119 is inconsistent

**Priority:** Medium. **Code change:** decision needed. **Docs:** [Parenthesized pattern](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/patterns#parenthesized-pattern)

SA1119 only registers `ParenthesizedExpression`. Inside a pattern, `(0)` parses as a constant pattern wrapping a
parenthesized *expression*, while `(int)` and `(> 0)` parse as a `ParenthesizedPattern`. The result:

| Code | Today |
|---|---|
| `i is not (0)`, `i is(1)`, `i is 1 or(2)` | SA1119 |
| `o is (int)`, `i is (> 0)`, `o is not (string)` | nothing |
| `o is (int or long) and not string` | nothing (parentheses are required) |

**Suggested change:** report a `ParenthesizedPattern` whose inner pattern is not an `and` or `or` pattern. Parentheses
around a binary pattern either change the meaning or clarify precedence, so they stay. Parentheses around any other
pattern (type, constant, relational, `not`, declaration, recursive) are redundant:

```csharp
context.RegisterSyntaxNodeAction(ParenthesizedPatternAction, SyntaxKindEx.ParenthesizedPattern);

private static void HandleParenthesizedPattern(SyntaxNodeAnalysisContext context)
{
    var node = (ParenthesizedPatternSyntaxWrapper)context.Node;
    if (node.Pattern.SyntaxNode is BinaryPatternSyntax) // via BinaryPatternSyntaxWrapper.IsInstance
    {
        return;
    }

    ReportDiagnostic(context, ...); // both SA1119 and SA1119_p, like the expression case
}
```

`SA1119CodeFixProvider` currently only handles `ParenthesizedExpressionSyntax` and needs a branch for the pattern
wrapper. That branch also needs the existing "insert a space after a keyword" workaround, so that `is(int)` becomes
`is int`.

**Tests:** whatever is decided, add to `SA1119CSharp9UnitTests`:

- Fix tests for what is already reported: `i is not (0)` → `i is not 0`, and `i is(1)` → `i is 1`. These pin the
  keyword-spacing behavior of `GetReplacement` (`SA1119CodeFixProvider.cs:73-81`).
- A no-diagnostic test for `o is (int or long) and not string`.
- If the change is made, fix tests for `o is (int)`, `i is (> 0)` and `o is not (string)`.

### Logical patterns: SA1408 does not cover mixed `and`/`or`

**Priority:** Low. **Code change:** decision needed.

`i is 1 or 2 and 3` produces no diagnostic. `and` binds tighter than `or`, just as `&&` binds tighter than `||`. SA1408
("Conditional expressions should declare precedence") reports the `&&`/`||` form but has no pattern equivalent.

**Suggested change:** if wanted, extend SA1408 to report an `AndPattern` that is a direct operand of an `OrPattern`.
This needs `SyntaxKindEx.OrPattern` and `AndPattern`, which are missing today (9031/9032, check the values). The code fix
would wrap the `and` pattern in parentheses (`1 or (2 and 3)`). This depends on the SA1119 subsection above: SA1119
must not then report those parentheses, which the "inner pattern is a binary pattern" exemption already ensures.

**Tests:** if implemented, add `SA1408CSharp9UnitTests` (new file) with a fix test, plus a no-diagnostic case for
`i is (1 or 2) and 3`. If rejected, add a short note to `documentation/SA1408.md` instead.

### Relational patterns: no spacing rule for the operator

**Priority:** Low. **Code change:** decision needed.

`i is >0` and `i is > 0` are both accepted. SA1003 only looks at binary, unary and assignment expressions, and a
`RelationalPattern` is none of these. Multiple spaces (`>  0`) are still caught by SA1025. The SA1000 tests' fixed code
happens to use the `is >1` form, so these tests may need updating if this changes.

**Suggested change:** if wanted, have SA1003 require a space after the operator token of a `RelationalPattern`
(`<`, `<=`, `>`, `>=`), with the usual "preceded by a space unless after `(`" handling. This needs
`SyntaxKindEx.RelationalPattern` (9029, check the value).

**Tests:** if implemented, add `SA1003CSharp9UnitTests` cases for `is >0` → `is > 0`, for `case <0:`, and for a switch
expression arm `<0 =>`. If rejected, add no test.

### Type patterns: pin SA1121 and spacing

**Priority:** Low. **Code change:** none expected. **Docs:** [Type pattern](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/patterns#type-pattern)

These already work (confirmed) but are untested:

- SA1121 reports `System.Int32` in `o is System.Int32 or System.String` and in a `case System.Int16:` type pattern.
- SA1009 reports `(int or long)and`.
- SA1024 reports `case > 5 :`.

**Tests:**

- `SA1121CSharp9UnitTests` (new file): a fix test for an `or` type pattern and a `case` type pattern.
- `SA1009CSharp9UnitTests`: a fix test for `)and`.
- `SA1024CSharp9UnitTests`: a fix test for `case > 5 :`.

## Native sized integers

**Docs:** [Native sized integers](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/integral-numeric-types#native-sized-integers)

### Pin SA1121 and SA1129

**Priority:** Low. **Code change:** none expected.

In C# 9 and 10, `nint`/`nuint` and `IntPtr`/`UIntPtr` are different spellings with different operators. SA1121
correctly reports neither `IntPtr` nor `nint` (confirmed). SA1129 reports both `new nint()` and `new IntPtr()`.

In C# 11 with .NET 7, `nint` became a true alias of `IntPtr`. Whether SA1121 should then suggest `nint` is a question
for the C# 11 review, not this one.

**Tests:**

- `SA1121CSharp9UnitTests`: no diagnostic for fields, parameters, casts and `sizeof` using `nint`, `nuint`, `IntPtr` and
  `System.UIntPtr`.
- `SA1129CSharp9UnitTests`: a fix test for `[|new nint()|]` → `default(nint)`. Check the expected fix output: it may be
  `default` or `0` depending on the fixer's constant handling.

## Function pointers

**Docs:** [Function pointers](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/unsafe-code#function-pointers)

Other function pointer rules work and are tested or confirmed:

- SA1001 for commas in parameters and calling-convention lists.
- SA1010 and SA1011 for `unmanaged[Cdecl]`.
- SA1023 for `*`.
- SA1121 for `delegate*<System.Int32, void>`.
- SA1008 for `p (1)`.

### SA1014 and SA1015 ignore the `<`/`>` of a function pointer

**Priority:** Medium. **Code change:** yes.

`delegate* < int, void > E;` only produces SA1023 for the space after `*`. The spaces inside the brackets are not
reported. SA1014 only accepts `TypeArgumentList` and `TypeParameterList` parents
(`SA1014OpeningGenericBracketsMustBeSpacedCorrectly.cs:68-71`), and SA1015 likewise (`SA1015...cs:82`). The function
pointer brackets belong to `FunctionPointerParameterList`, which isn't in `SyntaxKindEx` yet.

**Suggested change:** add `SyntaxKindEx.FunctionPointerParameterList = (SyntaxKind)9058` (check the value against
Roslyn's `SyntaxKind`), and add it as an accepted parent in both analyzers:

```csharp
switch (token.Parent.Kind())
{
case SyntaxKind.TypeArgumentList:
case SyntaxKind.TypeParameterList:
case SyntaxKindEx.FunctionPointerParameterList:
    break;
```

In SA1014, the token before `<` is `*` or `]` (from `unmanaged[Cdecl]`) or a calling-convention keyword (`managed`,
`unmanaged`). None of these should be followed by a space before `<`. Check that the existing "preceded" logic gives
that result.

**Tests:** `SA1014CSharp9UnitTests` and `SA1015CSharp9UnitTests` (new files), with `VerifyCSharpFixAsync`:

```csharp
var testCode = @"public unsafe class TestClass
{
    private delegate*{|#0:<|} int, void> a;
    private delegate* unmanaged[Cdecl]{|#1:<|} int, void> b;
}";
var fixedCode = @"public unsafe class TestClass
{
    private delegate*<int, void> a;
    private delegate* unmanaged[Cdecl]<int, void> b;
}";
var expected = new[]
{
    Diagnostic().WithArguments("followed").WithLocation(0),
    Diagnostic().WithArguments("followed").WithLocation(1),
};
```

The SA1015 test should cover `void >` (not preceded) and `void>a;` (followed). Use `[|>|]` there if the descriptors
take no arguments.

## Suppress emitting localsinit flag

**Docs:** [Suppress emitting localsinit flag](https://github.com/dotnet/csharplang/blob/main/proposals/csharp-9.0/skip-localsinit.md)

### No action

**Priority:** none.

`[SkipLocalsInit]` is an ordinary attribute with no new syntax. A probe produced no StyleCop diagnostics, and no rule
looks at the attribute by name. Nothing to test. Delete this section once agreed.

## Module initializers

**Docs:** [Module initializers](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/attributes/general#moduleinitializer-attribute)

### No action

**Priority:** none.

`[ModuleInitializer]` on an `internal static void` method is an ordinary attribute with no new syntax. A probe produced
no StyleCop diagnostics, and no rule looks at the attribute by name. Nothing to test. Delete this section once agreed.

## New features for partial methods

**Docs:** [Partial members](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/partial-member)

### SA1600 and SA1601 both report accessible partial methods

**Priority:** High. **Code change:** yes.

C# 9 allows partial methods with access modifiers (`public partial int M(out int x);`). Both parts of such a method now
get SA1600 *and* SA1601:

```
L5:28  SA1600 Elements should be documented          || public partial int M1(out int x);
L5:28  SA1601 Partial elements should be documented  || public partial int M1(out int x);
L27:28 SA1600 Elements should be documented          || public partial int M1(out int x)
L27:28 SA1601 Partial elements should be documented  || public partial int M1(out int x)
```

`SA1600ElementsMustBeDocumented.HandleBaseTypeDeclaration` skips partial types ("Handled by SA1601"), but
`HandleMethodDeclaration` does not skip partial methods. There is a TODO about this at
`StyleCop.Analyzers/DocumentationRules/SA1600ElementsMustBeDocumented.cs:199`. Before C# 9 this never showed up,
because partial methods were always implicitly private and private members need no documentation by default.

**Suggested change:**

```csharp
MethodDeclarationSyntax declaration = (MethodDeclarationSyntax)context.Node;
if (declaration.Modifiers.Any(SyntaxKind.PartialKeyword))
{
    // Handled by SA1601
    return;
}
```

Before removing the TODO, check whether SA1600's code fix (adding a documentation stub) should also be offered for
SA1601.

**Tests:**

- `SA1600CSharp9UnitTests`: a `public partial int M(out int x);` declaration and its implementation produce no SA1600.
- `SA1601CSharp9UnitTests` (new file): both parts report SA1601 when undocumented. A documented defining declaration
  silences the diagnostic on that part.
- `SA1601CSharp9UnitTests`: `partial void M();` and `private partial bool M();` are not reported with default
  settings, and both are reported when `documentPrivateElements` is `true`.

Use `[|M|]` markers, since no arguments are involved.

### Pin SA1400 and SA1202

**Priority:** Low. **Code change:** none expected.

Confirmed:

- SA1400 does not report `partial void M();`. This is correct: in C# 9, adding `private` would make an implementation
  mandatory.
- SA1202 orders partial methods by their explicit accessibility (`public partial int M4();` after a `private partial`
  is reported).
- SA1206 reports `partial public` (also a compiler error).

**Tests:**

- `SA1400CSharp9UnitTests` (new file): no diagnostic for `partial void M();` with and without an implementation part.
- `SA1202CSharp9UnitTests` (new file): a fix test reordering `private partial bool M3();` and `public partial int M4();`.

## Target-typed `new` expressions

**Docs:** [Target-typed new expressions](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/new-operator#target-typed-new)

SA1116, SA1117, SA1118 and SA1129 already have CSharp9 `new(...)` tests. SA1413 and SA1101 were confirmed to work for
`new(...) { ... }` initializers.

### SA1110–SA1115 ignore `new(...)` argument lists

**Priority:** High. **Code change:** yes.

SA1116, SA1117 and SA1118 handle `ImplicitObjectCreationExpression`. SA1110, SA1111, SA1112, SA1113, SA1114 and SA1115
only register `SyntaxKind.ObjectCreationExpression`. Confirmed silent:

```csharp
C c = new
    (1, 2);        // expected SA1110
C d = new(1, 2
    );             // expected SA1111, only SA1009 reported
List<int> q = new(
    );             // expected SA1112, only SA1009 reported
C e = new(
    1
    , 2);          // expected SA1113, only SA1001 reported
C f = new(

    1, 2);         // expected SA1114
C g = new(
    1,

    2);            // expected SA1115
```

**Suggested change:** register `SyntaxKindEx.ImplicitObjectCreationExpression` in each analyzer, as in
`SA1116SplitParametersMustStartOnLineAfterDeclaration.HandleImplicitObjectCreationExpression`:

```csharp
private static void HandleImplicitObjectCreationExpression(SyntaxNodeAnalysisContext context)
{
    var implicitObjectCreation = (ImplicitObjectCreationExpressionSyntaxWrapper)context.Node;
    HandleArgumentListSyntax(context, implicitObjectCreation.ArgumentList);
}
```

SA1110 is different from the others because it checks the open parenthesis against the token before it. Here that
token is the `new` keyword:

```csharp
private static void HandleImplicitObjectCreationExpression(SyntaxNodeAnalysisContext context)
{
    var implicitObjectCreation = (ImplicitObjectCreationExpressionSyntaxWrapper)context.Node;
    var openParen = implicitObjectCreation.ArgumentList.OpenParenToken;
    if (!openParen.IsMissing)
    {
        CheckIfLocationOfPreviousTokenAndOpenTokenAreTheSame(context, openParen, preserveLayout: false);
    }
}
```

**Tests:** one `TestTargetTypedNewExpression...Async` per rule in `StyleCop.Analyzers.Test.CSharp9/ReadabilityRules`
(SA1110 and SA1111 extend existing files; SA1112–SA1115 are new files). Use `VerifyCSharpFixAsync` for SA1110, SA1111,
SA1112 and SA1113, all of which use `TokenSpacingCodeFixProvider`:

```csharp
[Fact]
public async Task TestTargetTypedNewExpressionAsync()
{
    var testCode = @"
class Foo
{
    public Foo(int a, int b)
    {
    }

    public void Method()
    {
        Foo x = new
            [|(|]1, 2);
    }
}";

    var fixedCode = @"
class Foo
{
    public Foo(int a, int b)
    {
    }

    public void Method()
    {
        Foo x = new(1, 2);
    }
}";

    await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
}
```

### Pin SA1000 for `new (`

**Priority:** Low. **Code change:** none expected.

SA1000 already reports "The keyword 'new' should not be followed by a space" for `new (1, 2)`, but
`SA1000CSharp9UnitTests.TestTargetTypedNewAsync` only covers the no-space form.

**Tests:** add a fix test for `new (1, 2)` → `new(1, 2)` to `SA1000CSharp9UnitTests`.

## Static anonymous functions

**Docs:** [Static anonymous functions](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions#static-lambdas)

### SA1130's code fix drops `static`

**Priority:** High. **Code change:** yes.

SA1130 reports `static delegate (int x) { return x; }` and `static delegate { }`, which is correct. But
`SA1130CodeFixProvider` builds the replacement with `SyntaxFactory.SimpleLambdaExpression(anonymousMethod.AsyncKeyword, ...)`
and `SyntaxFactory.ParenthesizedLambdaExpression(anonymousMethod.AsyncKeyword, ...)`
(`StyleCop.Analyzers.CodeFixes/ReadabilityRules/SA1130CodeFixProvider.cs:170,176`). Only `async` is carried over, so
the fix silently turns a static anonymous method into a non-static lambda. That still compiles, but the guarantee
against capturing variables is lost.

**Suggested change:** add a lightup extension for `AnonymousFunctionExpressionSyntax.Modifiers` and `WithModifiers`
(the compile-time Roslyn 1.2.1 only has `AsyncKeyword`). Model it on `Lightup/MemberDeclarationSyntaxExtensions.cs`.
`Syntax.xml:1405` already lists the field. Then copy the modifiers over:

```csharp
// Lightup/AnonymousFunctionExpressionSyntaxExtensions.cs
internal static class AnonymousFunctionExpressionSyntaxExtensions
{
    private static readonly Func<AnonymousFunctionExpressionSyntax, SyntaxTokenList> ModifiersAccessor;
    private static readonly Func<AnonymousFunctionExpressionSyntax, SyntaxTokenList, AnonymousFunctionExpressionSyntax> WithModifiersAccessor;

    static AnonymousFunctionExpressionSyntaxExtensions()
    {
        ModifiersAccessor = LightupHelpers.CreateSyntaxPropertyAccessor<AnonymousFunctionExpressionSyntax, SyntaxTokenList>(typeof(AnonymousFunctionExpressionSyntax), nameof(Modifiers));
        WithModifiersAccessor = LightupHelpers.CreateSyntaxWithPropertyAccessor<AnonymousFunctionExpressionSyntax, SyntaxTokenList>(typeof(AnonymousFunctionExpressionSyntax), nameof(Modifiers));
    }

    public static SyntaxTokenList Modifiers(this AnonymousFunctionExpressionSyntax syntax) => ModifiersAccessor(syntax);

    public static AnonymousFunctionExpressionSyntax WithModifiers(this AnonymousFunctionExpressionSyntax syntax, SyntaxTokenList modifiers) => WithModifiersAccessor(syntax, modifiers);
}

// SA1130CodeFixProvider, after lambdaExpression is built:
var modifiers = anonymousMethod.Modifiers();
if (modifiers.Any(SyntaxKind.StaticKeyword))
{
    lambdaExpression = (LambdaExpressionSyntax)lambdaExpression.WithModifiers(modifiers);
}
```

Check what the Roslyn 1.2.1 fallback of the property accessor returns. It should be an empty list. Also check that the
`static` token keeps its trailing space.

**Tests:** in `StyleCop.Analyzers.Test.CSharp9/ReadabilityRules/SA1130CSharp9UnitTests.cs` (new file):

```csharp
[Fact]
public async Task TestStaticAnonymousMethodAsync()
{
    var testCode = @"using System;
public class TestClass
{
    public void TestMethod()
    {
        Func<int, int> a = static [|delegate|] (int x) { return x; };
        Action b = static [|delegate|] { };
    }
}";

    var fixedCode = @"using System;
public class TestClass
{
    public void TestMethod()
    {
        Func<int, int> a = static x => { return x; };
        Action b = static () => { };
    }
}";

    await VerifyCSharpFixAsync(testCode, DiagnosticResult.EmptyDiagnosticResults, fixedCode, CancellationToken.None).ConfigureAwait(true);
}
```

Check which token the diagnostic is reported on before finalizing the markers.

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

## Target-typed conditional expressions

**Docs:** [Conditional operator](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/conditional-operator)

### No action

**Priority:** none.

`b ? 1 : null` is a typing change only and parses exactly as before. A probe produced no StyleCop diagnostics. Nothing
to test. Delete this section once agreed.

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
