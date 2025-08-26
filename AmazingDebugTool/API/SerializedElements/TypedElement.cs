using System;

namespace JITDebugTool.API.SerializedElements
{
    internal abstract class TypedElement(Type type)
    {
        public string FullType { get; } = type.FullName;

        public string Class { get; } = type.Name;

        public string Namespace { get; } = type.Namespace;

        public string TypeAssembly { get; } = type.Assembly.FullName;
    }
}
