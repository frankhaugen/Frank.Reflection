# Frank.Reflection

Initially a slimmed down "fork" of Namotion.Refection, (also MIT licensed), but it is evolving into something else.
___
[![GitHub License](https://img.shields.io/github/license/frankhaugen/Frank.Reflection)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Frank.Reflection.svg)](https://www.nuget.org/packages/Frank.Reflection)
[![NuGet](https://img.shields.io/nuget/dt/Frank.Reflection.svg)](https://www.nuget.org/packages/Frank.Reflection)

![GitHub contributors](https://img.shields.io/github/contributors/frankhaugen/Frank.Reflection)
![GitHub Release Date - Published_At](https://img.shields.io/github/release-date/frankhaugen/Frank.Reflection)
![GitHub last commit](https://img.shields.io/github/last-commit/frankhaugen/Frank.Reflection)
![GitHub commit activity](https://img.shields.io/github/commit-activity/m/frankhaugen/Frank.Reflection)
![GitHub pull requests](https://img.shields.io/github/issues-pr/frankhaugen/Frank.Reflection)
![GitHub issues](https://img.shields.io/github/issues/frankhaugen/Frank.Reflection)
![GitHub closed issues](https://img.shields.io/github/issues-closed/frankhaugen/Frank.Reflection)
___

## Installation

### NuGet

```bash
dotnet add package Frank.Reflection
```

## Usage

### Get the name of a type

```csharp
var name = typeof(Person).GetDisplayName();
```

### Check if a type has a property by name

```csharp
var hasProperty = typeof(Person).HasProperty("Name");
```

### Try to get the value of a property

```csharp
var person = new Person { Name = "Bill" };
var hasValue = person.TryGetPropertyValue<T>("Name", out var value);

if (hasValue)
{
    Console.WriteLine(value);
}
```
___
## Frank.Reflection.Dump

A library to dump types to a string that is compilable into a new type. This is useful for generating code from types, for example when generating code for a collection of suppliers to use in a test.

___
[![NuGet](https://img.shields.io/nuget/v/Frank.Reflection.Dump.svg)](https://www.nuget.org/packages/Frank.Reflection.Dump)
[![NuGet](https://img.shields.io/nuget/dt/Frank.Reflection.Dump.svg)](https://www.nuget.org/packages/Frank.Reflection.Dump)
___

### Usage

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

## License

[MIT](LICENSE)