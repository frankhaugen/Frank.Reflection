using System.Reflection;

namespace Frank.Reflection;

public static class ParameterInfoExtensions
{
    public static string GetDisplayName(this ParameterInfo parameterInfo)
    {
        return $"{parameterInfo.Name} : {parameterInfo.ParameterType.GetFriendlyName()}";
    }
    
    public static string GetFullDisplayName(this ParameterInfo parameterInfo)
    {
        return $"{parameterInfo.Name} : {parameterInfo.ParameterType.GetFullFriendlyName()}";
    }
}