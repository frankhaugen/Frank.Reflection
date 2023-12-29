# Frank.Reflection

Initially a slimmed down "fork" of Namotion.Refection, (also MIT licensed), but it is evolving into something else.

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

## License

[MIT](LICENSE)