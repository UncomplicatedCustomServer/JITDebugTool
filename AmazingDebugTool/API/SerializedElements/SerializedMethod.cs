using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JITDebugTool.API.SerializedElements
{
    internal class SerializedMethod(MethodBase methodInfo) : TypedElement(methodInfo.DeclaringType)
    {
        public string Name { get; } = methodInfo.Name.Replace("_Patch2", string.Empty);

        public bool IsStatic { get; } = methodInfo.IsStatic;

        public List<SerializedParamInfo> Parameters { get; } = [.. methodInfo.GetParameters().Select(p => new SerializedParamInfo(p))];

        internal string RenderParameters()
        {
            List<string> res = [.. Parameters.Select(p => $"{p.TypeFullName} {p.Name}")];

            return string.Join( ", ", res);
        }
    }
}
