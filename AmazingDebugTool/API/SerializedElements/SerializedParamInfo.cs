using System.Reflection;

namespace JITDebugTool.API.SerializedElements
{
    internal class SerializedParamInfo(ParameterInfo parameter) : TypedElement(parameter.ParameterType)
    {
        public string Name { get; } = parameter.Name;
    }
}
