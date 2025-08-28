using JITDebugTool.API.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JITDebugTool.API.SerializedElements
{
    internal class SerializedMethod(MethodBase methodInfo, bool isStackTrace = false) : TypedElement(methodInfo.DeclaringType)
    {
        public string Name { get; } = methodInfo.Name.Replace("_Patch2", string.Empty);

        public string FullName { get; } = RenderFullName(methodInfo, isStackTrace);

        public bool IsStatic { get; } = methodInfo.IsStatic;

        public string Signature { get; } = methodInfo.GetSignature();

        public List<SerializedParamInfo> Parameters { get; } = [.. methodInfo.GetParameters().Select(p => new SerializedParamInfo(p))];

        private static string RenderFullName(MethodBase methodInfo, bool isStackTrace = false)
        {
            string result = string.Empty;

            if (!isStackTrace)
                result = $"{methodInfo.DeclaringType.FullName}{(methodInfo.IsStatic ? "::" : ".")}{methodInfo.Name.Replace("_Patch2", string.Empty)}({string.Join(", ", methodInfo.GetParameters().Select(p => $"{p.ParameterType.FullName.ToFormattedGenericName()} {p.Name}"))})";
            else if (isStackTrace && methodInfo.Name.Contains("."))
                result = $"{(methodInfo.IsStatic ? methodInfo.Name.Replace($"{methodInfo.Name}.", $"{methodInfo.Name}::") : methodInfo.Name).Replace("()", string.Empty).Replace("_Patch2", string.Empty)}({string.Join(", ", methodInfo.GetParameters().Select(p => $"{p.ParameterType.FullName.ToFormattedGenericName()}"))})";
            else
                result = $"{methodInfo.DeclaringType.FullName.ToFormattedGenericName()}{(methodInfo.IsStatic ? "::" : ".")}{methodInfo.Name.Replace("_Patch2", string.Empty)}({string.Join(", ", methodInfo.GetParameters().Select(p => $"{p.ParameterType.FullName.ToFormattedGenericName()}"))})";

            return result;
        }

        internal string RenderParameters(bool isStackTrace = false)
        {
            List<string> res = [.. Parameters.Select(p => $"{p.FullType.ToFormattedGenericName()}{(isStackTrace ? string.Empty : $" {p.Name}")}")];

            return string.Join( ", ", res);
        }
    }
}
