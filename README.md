# New StyleCop Analyzers for the .NET Compiler Platform

This repository contains an implementation of the StyleCop rules using the .NET Compiler Platform. Where possible, code fixes are also provided to simplify the process of correcting violations.

**This project has been created by cloning the [StyleCopAnalyzers](https://github.com/DotNetAnalyzers/StyleCopAnalyzers) project and merging some of the pull requests that were created. Hopefully as a temporary solution until maintenance of the original project starts working well again. Changes are kept small until the status of the original project is clearer.**

## Using NewStyleCop.Analyzers

The preferable way to use the analyzers is to add the nuget package [NewStyleCop.Analyzers](http://www.nuget.org/packages/NewStyleCop.Analyzers/)
to the project where you want to enforce StyleCop rules.

The severity of individual rules may be configured using [rule set files](https://docs.microsoft.com/en-us/visualstudio/code-quality/using-rule-sets-to-group-code-analysis-rules)
in Visual Studio 2015 or newer. **Settings.StyleCop** is not supported, but a **stylecop.json** file may be used to
customize the behavior of certain rules. See [Configuration.md](documentation/Configuration.md) for more information.

For documentation and reasoning on the rules themselves, see the [Documentation](DOCUMENTATION.md).

For users upgrading from StyleCop Classic, see [KnownChanges.md](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/tree/master/documentation/KnownChanges.md)
for information about known differences which you may notice when switching to StyleCop Analyzers.

### C# language versions
Not all versions of StyleCop.Analyzers support all features of each C# language version. The table below shows the minimum version of StyleCop.Analyzers required for proper support of a C# language version.

| C# version | StyleCop.Analyzers version | Visual Studio version |
|------------|----------------------------|-----------------------|
| 1.0 - 6.0  | v1.0.2 or higher           | VS2015+               |
| 7.0 - 7.3  | v1.1.0-beta or higher      | VS2017+               |
|    8.0     | v1.2.0-beta or higher      | VS2019                |

## Team Considerations

If you use older versions of Visual Studio in addition to Visual Studio 2015 or Visual Studio 2017, you may still install these analyzers. They will be automatically disabled when you open the project back up in Visual Studio 2013 or earlier.

## Contributing

See [Contributing](CONTRIBUTING.md)

## Current status

An up-to-date list of which StyleCop rules are implemented and which have code fixes can be found [here](https://dotnetanalyzers.github.io/StyleCopAnalyzers/).
