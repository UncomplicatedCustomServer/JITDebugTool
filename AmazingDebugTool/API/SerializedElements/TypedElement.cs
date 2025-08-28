using System;

namespace JITDebugTool.API.SerializedElements
{
    internal abstract class TypedElement(Type type)
    {
        public string FullType { get; } = type.FullName;
    }
}
