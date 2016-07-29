
## Status

[![Issue Board](https://badge.waffle.io/ivanz/dotnet-test-mspec.svg?title=Issue%20Board)](http://waffle.io/ivanz/dotnet-test-mspec)


[![Build status](https://ci.appveyor.com/api/projects/status/k5o2bvt24a775h96?svg=true)](https://ci.appveyor.com/project/machine-specifications/dotnet-test-mspec)


## What is this?

`dotnet-test-mspec` adds support for Machine.Specification tests to `dotnet test` and support running them on the following frameworks:

* .NET Core 1.0+
* .NET 4.5+

Requires Machine Specifications >= 0.10 (which itself is the first version to support targeting .Net Core)

## Get started

The only action that you need to take is to install the `dotnet-test-mspec` NuGet package in each project with MSpec tests and set the `testRunner` to `mspec`:

**project.json**

```json
  "testRunner": "mspec",
  "dependencies": {
        "Machine.Specifications": "0.*",
        "Machine.Specifications": "0.*",
        "dotnet-test-mspec": {
            "version": "*",
            "type": "build"
        }
    }
```

Then you can use `dotnet test` as usual:

```cmd
> dotnet test

Specs in Something.Or.Other.dll:
SampleSpec
> should be hello
> should be world
Contexts: 1, Specifications: 2, Time: 0.07 seconds

SUMMARY: Total: 1 targets, Passed: 1, Failed: 0.
```
