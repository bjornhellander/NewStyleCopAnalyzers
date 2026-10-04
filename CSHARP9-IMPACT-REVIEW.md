# C# 9 impact review

## Attributes on local functions

**Docs:** [Local function declarations](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/statements#1364-local-function-declarations)

### SA1137 ignores attribute lists on local functions

**Priority:** Low. **Code change:** yes.

SA1137 compares a local function only with the other statements in its block, never with its own attribute lists. An
attribute list indented differently from its local function is therefore not reported, while the same layout on a method
is.

**Suggested change:** add a `LocalFunctionStatement` case to `AddMemberAndAttributes` in
`SA1137ElementsShouldHaveTheSameIndentation.cs`, and use it from `HandleBlock`, so the attribute lists are checked
together with the statements.

**Tests:** a fix test in `SA1137CSharp9UnitTests` with a misaligned attribute list on a local function that is the only
statement in its block, and one on a local function that follows another statement.
