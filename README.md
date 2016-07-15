
[![Roadmap / Kanban Board](https://badge.waffle.io/ivanz/dotnet-test-mspec.svg?title=Roadmap%20and%20Task%20Board)](http://waffle.io/ivanz/dotnet-test-mspec)

## What is this?

It's a package that you can install to add support for MSpec (Machine.Specifications) to the .Net CLI / .Net Core / `dotnet test`.

## Get started

The only action that you need to take is to install the `dotnet-test-mspec` NuGet package in each project with MSpec tests:

**project.json**

```json
  "dependencies": {
        "Machine.Specifications.Core": "0.5.*",
        "Machine.Specifications.Should.Core": "0.5.*",
        // This comes from the build output of the project: see nuget.config
        "dotnet-test-mspec": {
            "version": "*",
            "type": "build"
        }
    }
```

Then you can use `dotnet test` as usual:

```cmd
> dotnet test

Project Machine.Specifications.Core (.NETCoreApp,Version=v1.0) was previously compiled. Skipping compilation.
Project Machine.Specifications.Should.Core (.NETCoreApp,Version=v1.0) was previously compiled. Skipping compilation.
Project Machine.Specifications.Core.Runner.DotNet.Tests (.NETCoreApp,Version=v1.0) was previously compiled. Skipping compilation.

Specs in Machine.Specifications.Core.Runner.DotNet.Tests:
SampleSpec
┬╗ should be hello
┬╗ should be world
Contexts: 1, Specifications: 2, Time: 0.07 seconds

SUMMARY: Total: 1 targets, Passed: 1, Failed: 0.
```

## Note

This tool uses the .Net Core port of Machine.Specifications here: https://einari/machine.specifications.core