using System;

namespace JITDebugTool.API.SerializedElements
{
    internal abstract class TypedElement(Type type)
    {
        public string TypeFullName { get; } = type.FullName;

        public string Type { get; } = type.Name;

        public string TypeAssembly { get; } = type.Assembly.FullName;
    }
}
