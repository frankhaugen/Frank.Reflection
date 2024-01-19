using Frank.Reflection.Dump;
using Frank.Reflection.Tests.TestingInfrastructure;

using Xunit.Abstractions;

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
}