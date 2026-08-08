# New StyleCop Analyzers for the .NET Compiler Platform

This repository contains an implementation of the StyleCop rules using the .NET Compiler Platform.
Where possible, code fixes are also provided to simplify the process of correcting violations.

**This repository has been created by cloning the [StyleCopAnalyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers) project and merging some of the pull requests that were created.
Hopefully as a temporary solution until maintenance of the original project starts working well again.**

## Using NewStyleCop.Analyzers

The way to use the analyzers is to add the nuget package [NewStyleCop.Analyzers](http://www.nuget.org/packages/NewStyleCop.Analyzers/)
to the project where you want to enforce StyleCop rules.

The severity of individual rules may be configured using [rule set files](https://docs.microsoft.com/en-us/visualstudio/code-quality/using-rule-sets-to-group-code-analysis-rules).
**Settings.StyleCop** is not supported, but a **stylecop.json** file may be used to
customize the behavior of certain rules. See [Configuration.md](https://github.com/bjornhellander/NewStyleCopAnalyzers/tree/master/documentation/Configuration.md) for more information.

For documentation and reasoning on the rules themselves, see the [Documentation](https://github.com/bjornhellander/NewStyleCopAnalyzers/tree/master/DOCUMENTATION.md).

For users upgrading from StyleCop Classic, see [KnownChanges.md](https://github.com/bjornhellander/NewStyleCopAnalyzers/tree/master/documentation/KnownChanges.md)
for information about known differences which you may notice when switching to StyleCop Analyzers.

## Contributing

See [Contributing](https://github.com/bjornhellander/NewStyleCopAnalyzers/tree/master/CONTRIBUTING.md)

## Current status

An up-to-date list of which StyleCop rules are implemented and which have code fixes can be found [here](https://dotnetanalyzers.github.io/StyleCopAnalyzers/).
