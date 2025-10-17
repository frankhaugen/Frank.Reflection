using Frank.Reflection.Dump;
using Frank.Reflection.Tests.TestingInfrastructure;
using FluentAssertions;

namespace Frank.Reflection.Tests.Dump;

public class DumpExtensionsTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public void Dump()
    {
        var data = new Person
        {
            Name = "Frank",
            Age = 30,
            Address = new Address
            {
                Street = "Street",
                Number = 1
            }
        };
        
        outputHelper.WriteLine(data.DumpClass());
    }
    
    [Fact]
    public void DumpWithDepth()
    {
        var data = new Person
        {
            Name = "Frank",
            Age = 30,
            Address = new Address
            {
                Street = "Street",
                Number = 1
            }
        };
        
        outputHelper.WriteLine(data.DumpVar());
    }
    
    [Fact]
    public void DumpWithDepthAndIndent()
    {
        var data = new Person
        {
            Name = "Frank",
            Age = 30,
            Address = new Address
            {
                Street = "Street",
                Number = 1
            }
        };
        
        outputHelper.WriteLine(data.DumpVar());
    }
    
    [Fact]
    public void DumpEnumerable()
    {
        var people = new List<Person>
        {
            new Person { Name = "Frank", Age = 30, Address = new Address { Street = "Street", Number = 1 } },
            new Person { Name = "Alice", Age = 25, Address = new Address { Street = "Avenue", Number = 2 } },
            new Person { Name = "Bob", Age = 35, Address = new Address { Street = "Boulevard", Number = 3 } }
        };
        
        var result = people.DumpEnumerable(p => p.Name);
        outputHelper.WriteLine(result);
        
        // Verify the result contains expected elements
        result.Should().Contain("public class People : IEnumerable<Person>");
        result.Should().Contain("yield return GetFrank();");
        result.Should().Contain("yield return GetAlice();");
        result.Should().Contain("yield return GetBob();");
        result.Should().Contain("public static Person GetFrank()");
        result.Should().Contain("public static Person GetAlice()");
        result.Should().Contain("public static Person GetBob()");
    }
}