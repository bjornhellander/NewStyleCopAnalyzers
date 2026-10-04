# C# 10 review #162: attributes on lambdas

## Done on this branch

- SA1008: test showing that a space is required between the attribute list and the parameter list
  (`[Obsolete]() => 1` → `[Obsolete] () => 1`).
- SA1017: test showing that a space before the closing attribute bracket is reported (`[Obsolete ] () => 1`).

No code changes were needed for these.

## Needs a decision: SA1133 and SA1134 on lambda attributes

Both rules already skip attribute lists on parameters and type parameters
(`attributeList.Parent.IsKind(SyntaxKind.Parameter) || attributeList.Parent.IsKind(SyntaxKind.TypeParameter)`), but not
attribute lists on lambdas. Attributes on lambda parameters (`([NotNull] string s) => s`) are therefore not reported, but
attributes on the lambda itself are.

### SA1134 (each attribute on its own line)

Reports `[Obsolete] () => 1`, `[Obsolete, CLSCompliant(false)] () => 1` and `[return: NotNull] () => string.Empty`. The
code fix then produces this, with indentation that doesn't fit the surrounding code:

```csharp
        var a =
    [Obsolete]
    () => 1;
```

### SA1133 (each attribute in its own set of brackets)

Reports `[Obsolete, CLSCompliant(false)] () => 1`. The code fix splits it over two lines without indentation:

```csharp
        var b = [Obsolete]
[CLSCompliant(false)] () => 1;
```

### Options

1. **Skip lambda attribute lists in both rules** (add `ParenthesizedLambdaExpression` and `SimpleLambdaExpression`
   parents next to the existing parameter checks). Treats lambda attributes like parameter attributes: inline, in an
   expression. Simplest, and avoids the poor fix results. My suggestion for SA1134.
2. **Keep SA1133 for lambdas, but fix its code fix** to produce `[Obsolete][CLSCompliant(false)] () => 1` on the same
   line when the attribute list is not on its own line. Keeps the "one attribute per brackets" rule consistent
   everywhere.
3. **Keep both as they are**, and only fix the code fixes' indentation.

Tests would go in `SA1133CSharp10UnitTests` / `SA1134CSharp10UnitTests` (new files) once the behavior is decided.
