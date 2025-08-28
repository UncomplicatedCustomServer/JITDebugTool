using System.Linq;
using System.Reflection;
using System.Text;

namespace JITDebugTool.API.Extensions
{
    internal static class MethodBaseExtension
    {
        public static string GetSignature(this MethodBase method)
        {
            if (method == null)
                return string.Empty;

            StringBuilder sb = new();

            if (method.DeclaringType != null)
                sb.Append(method.DeclaringType.FullName).Append(".");

            sb.Append(method.Name);

            sb.Append("(");

            var parameters = method.GetParameters();
            if (parameters.Length > 0)
            {
                var paramTypes = parameters.Select(p => p.ParameterType.FullName);
                sb.Append(string.Join(", ", paramTypes));
            }

            sb.Append(")");

            if (method.IsGenericMethod)
            {
                var genericArgs = method.GetGenericArguments().Select(arg => arg.Name);
                sb.Append("<").Append(string.Join(", ", genericArgs)).Append(">");
            }

            return sb.ToString();
        }
    }
}
