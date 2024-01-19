# Frank.Reflection.Dump.Dump

A library to dump types to a string that is compilable into a new type. This is useful for generating code from types, for example when generating code for a collection of suppliers to use in a test.

___
[![GitHub License](https://img.shields.io/github/license/frankhaugen/Frank.Reflection.Dump)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Frank.Reflection.Dump.svg)](https://www.nuget.org/packages/Frank.Reflection.Dump)
[![NuGet](https://img.shields.io/nuget/dt/Frank.Reflection.Dump.svg)](https://www.nuget.org/packages/Frank.Reflection.Dump)

![GitHub contributors](https://img.shields.io/github/contributors/frankhaugen/Frank.Reflection.Dump)
![GitHub Release Date - Published_At](https://img.shields.io/github/release-date/frankhaugen/Frank.Reflection.Dump)
![GitHub last commit](https://img.shields.io/github/last-commit/frankhaugen/Frank.Reflection.Dump)
![GitHub commit activity](https://img.shields.io/github/commit-activity/m/frankhaugen/Frank.Reflection.Dump)
![GitHub pull requests](https://img.shields.io/github/issues-pr/frankhaugen/Frank.Reflection.Dump)
![GitHub issues](https://img.shields.io/github/issues/frankhaugen/Frank.Reflection.Dump)
![GitHub closed issues](https://img.shields.io/github/issues-closed/frankhaugen/Frank.Reflection.Dump)
___

## Usage

```csharp
var type = typeof(Persons);
var dump = type.DumpClass();
```
___
```csharp
namespace GeneratedCode;

public static class GeneratedPerson
{
    public static Person Get()
    {
        return new Person
        {
            Name = "Frank",
            Age = 30,
            Address = new Address
            {
                Street = "Street",
                Number = 1
            }
        };
    }
}
```