# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

NewStyleCop.Analyzers is a Roslyn-based implementation of the StyleCop rules (diagnostics + code fixes) for C#.
It was created by forking [DotNetAnalyzers/StyleCopAnalyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)
and merging in outstanding pull requests, as a stopgap while the upstream project's maintenance status is unclear.
Changes are intentionally kept small and close to upstream where possible.

## Commands

Building requires the .NET SDK version pinned in `global.json` (currently 10.0.202, `rollForward: feature`).

```bash
# Restore/build everything
dotnet build StyleCopAnalyzers.sln -c Debug

# Run the full test suite for one C# language-version test project
dotnet test StyleCop.Analyzers/StyleCop.Analyzers.Test.CSharp15/StyleCop.Analyzers.Test.CSharp15.csproj -c Debug

# Run a single test by fully-qualified name
dotnet test StyleCop.Analyzers/StyleCop.Analyzers.Test.CSharp6/StyleCop.Analyzers.Test.CSharp6.csproj --filter "FullyQualifiedName~SA1000UnitTests.TestCatchStatementAsync"
```

CI (`.github/workflows/build.yml`) builds/tests each `StyleCop.Analyzers.Test.CSharpN` project (N = 6..15) individually,
in both Debug and Release, on Windows. When verifying a change, prefer running the specific `TestCSharpN` project(s)
relevant to the rule/syntax touched rather than the whole matrix, and run `TestCSharp6` (or the lowest project that
compiles the affected code) plus the highest (`TestCSharp15`) if unsure.

Builds outside Visual Studio (`BuildingInsideVisualStudio != 'true'`, e.g. from the CLI) set
`TreatWarningsAsErrors=true` — CLI builds are stricter than IDE builds.

## Architecture

### Project graph (`StyleCop.Analyzers/`)

- **StyleCop.Analyzers** — the analyzers themselves (`netstandard1.1`). One rule area per folder: `SpacingRules`,
  `ReadabilityRules`, `OrderingRules`, `NamingRules`, `MaintainabilityRules`, `LayoutRules`, `DocumentationRules`,
  `SpecialRules`. Also contains `Settings` (stylecop.json parsing), `Helpers`, `LightJson` (bundled JSON parser),
  `LinqHelpers`, and `Lightup` (see below).
- **StyleCop.Analyzers.CodeFixes** — public code fix providers, mirroring the same rule-area folder layout.
- **StyleCop.Analyzers.PrivateAnalyzers** / **StyleCop.Analyzers.PrivateCodeFixes** — internal-only tooling used
  while developing this repo (not shipped in the NuGet package), e.g. `DerivedTestGenerator` (see Test chain below).
- **StyleCop.Analyzers.CodeGeneration** — Roslyn incremental source generators (`SyntaxLightupGenerator`,
  `OperationLightupGenerator`) that generate the `Lightup/.generated` wrapper code from `Lightup/Syntax.xml` and
  `Lightup/OperationInterfaces.xml`.
- **StyleCop.Analyzers.Status.Generator** — generates the public rule-implementation-status pages from the
  analyzer/codefix assemblies.
- **StyleCopTester** — a standalone console app for running the analyzers against arbitrary external solutions
  (useful for manually validating a rule change against real-world code).
- **StyleCop.Analyzers.Test.CSharp6** through **StyleCop.Analyzers.Test.CSharp15** — the test suite, split per
  C# language version (see below).

### Lightup: multi-version Roslyn compatibility

`StyleCop.Analyzers` targets `netstandard1.1` and depends on an old `Microsoft.CodeAnalysis.CSharp` (1.2.1) so the
compiled analyzer works across a wide range of Visual Studio / Roslyn host versions. To use APIs and syntax added in
*newer* Roslyn/C# versions without a hard dependency on them, the `Lightup` folder provides reflection-backed wrapper
types and extension methods (e.g. `BaseNamespaceDeclarationSyntaxWrapper`, `IFieldSymbolExtensions`). Wrappers for
syntax/operation nodes are source-generated (`Lightup/.generated`, `EmitCompilerGeneratedFiles=true`) from
`Lightup/Syntax.xml` and `Lightup/OperationInterfaces.xml` via `StyleCop.Analyzers.CodeGeneration`; hand-written
lightup helpers live alongside them. When a rule needs to inspect syntax/semantics introduced in a C# version newer
than the analyzer's compile-time Roslyn reference, add/extend a Lightup wrapper rather than referencing the syntax
API directly.

### Test chain across C# language versions

Each `StyleCop.Analyzers.Test.CSharpN` project project-references `StyleCop.Analyzers.Test.CSharp(N-1)`, forming a
chain from `CSharp6` (the base) up to `CSharp15`. Put a test in the **lowest** `CSharpN` project whose language
version can express the syntax under test — e.g. a test needing pattern matching goes in `CSharp7`, not `CSharp6`.
A source generator (`PrivateAnalyzers/DerivedTestGenerator.cs`) automatically emits derived subclasses of every test
class from the previous project into the current one, so tests defined in `CSharp6` actually execute (recompiled
against the newer language version) in every downstream project without duplication. This means running only
`TestCSharp15` effectively re-runs the entire test suite under the newest compiler, while running `TestCSharp6` runs
only the base-level tests.

Tests use `Microsoft.CodeAnalysis.Testing` via the repo's `StyleCopCodeFixVerifier<TAnalyzer, TCodeFix>` (see
`Verifiers` folder in `StyleCop.Analyzers.Test.CSharp6`), with an `EmptyDiagnosticResults` result for "no diagnostic
expected" cases.

When the rule under test has a registered code fix provider, prefer verifying the fix (`VerifyCSharpFixAsync` with
the expected fixed source) over verifying only the diagnostic (`VerifyCSharpDiagnosticAsync`) — this also exercises
the fix and catches fix-specific bugs (e.g. a fix that produces wrong output only for certain syntax) that
diagnostic-only tests can't see. Fall back to diagnostic-only verification when there's no code fix provider for the
rule, or the specific scenario under test isn't fixable (e.g. the fix is unavailable for that syntax shape).

`VerifyCSharpFixAsync` also asserts that its `fixedCode` triggers zero diagnostics, so it already doubles as the
negative case for whatever valid syntax the fix produces. Don't add a separate standalone negative test (a
`VerifyCSharpDiagnosticAsync` call with `EmptyDiagnosticResults`) if its source is just that same fixed code — it's
redundant. Do keep a separate negative test when it exercises a scenario the fix doesn't produce, e.g. a different
valid construct (`default` literal vs. the fix's `default(int)`) or a precondition the analyzer treats specially
(a call to `base.Method()` from inside an actual override, vs. the fix rewriting an invalid one to `this.Method()`).

Prefer markup (`[|Xyz|]` or `{|#0:Xyz|}`) over hardcoded `.WithLocation(line, column)` / `.WithSpan(...)`
coordinates: markup keeps the expected location anchored to the source text itself, so it stays correct if a line
above it is later added or removed, whereas a hardcoded coordinate silently goes stale. Fall back to explicit
coordinates only when markup can't cleanly express the location — e.g. the diagnostic isn't a visible span of source
text (line-ending trivia), or if it would hinder readability (e.g. nesting one marker inside another).

Between the two markup forms, use plain `[|Xyz|]` whenever the test expects exactly one diagnostic, at one
location, with no `.WithArguments(...)` and no non-default descriptor — pass `DiagnosticResult.EmptyDiagnosticResults`
as the expected result and the marker alone supplies the expected diagnostic. Use `{|#0:Xyz|}` +
`Diagnostic().WithLocation(0)` instead once you need more than `[|Xyz|]` can express: attaching arguments, picking a
specific descriptor (multi-diagnostic analyzers), or referencing more than one location (numbered markers
`{|#0:...|}`, `{|#1:...|}`, ... — non-overlapping, not nested).

### Rule anatomy

Each rule follows a consistent triad, all named by diagnostic ID (`SAxxxx`/`SXxxxx`):

- Analyzer: `StyleCop.Analyzers/<RuleArea>/SAxxxx<Name>.cs` — `internal class` deriving `DiagnosticAnalyzer`,
  `DiagnosticId` constant, resources-backed `Title`/`MessageFormat`/`Description`, `HelpLink` pointing at
  `documentation/SAxxxx.md`, and category `AnalyzerCategory.<Area>`.
- Code fix (optional): `StyleCop.Analyzers.CodeFixes/<RuleArea>/SAxxxxCodeFixProvider.cs`.
- Tests: `StyleCop.Analyzers.Test.CSharp6/<RuleArea>/SAxxxxUnitTests.cs` (or a higher `CSharpN` project if the rule
  needs newer syntax), placed in the lowest project whose language version can express the tested syntax.
- Docs: `documentation/SAxxxx.md`, linked from the relevant `documentation/<RuleArea>.md` index page.
- Public API surface changes (new/changed diagnostic IDs, new public types) must be reflected in each project's
  `PublicAPI.Shipped.txt` / `PublicAPI.Unshipped.txt` (enforced by `Microsoft.CodeAnalysis.PublicApiAnalyzers`).

### Self-hosting configuration

The repo dogfoods itself: `Directory.Build.props` (under `StyleCop.Analyzers/`) references the
`NewStyleCop.Analyzers` NuGet package and points every project at `StyleCop.Analyzers/stylecop.json` for its own
style configuration, and `GenerateDocumentationFile=true` so documentation analyzers can run. `LangVersion` is
pinned to 10 for the codebase itself regardless of the C# versions being analyzed/tested.

## Documentation conventions

`DOCUMENTATION.md` and the per-area pages under `documentation/` (`SpacingRules.md`, `OrderingRules.md`, etc.) are
the index of implemented rules; `documentation/SAxxxx.md` files are the per-rule reference docs and their diagnostic
`HelpLink` targets. Keep both in sync when adding, renaming, or removing a rule.
