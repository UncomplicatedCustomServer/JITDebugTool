using System.Globalization;
using System;
using System.Reflection;

namespace JITDebugTool.API.SerializedElements
{
    internal class SerializedParamInfo(ParameterInfo parameter) : TypedElement(parameter.ParameterType)
    {
        public string Name { get; } = parameter.Name;

        public bool IsMandatory { get; } = !parameter.HasDefaultValue;

        public string DefaultValue { get; } = parameter.HasDefaultValue ? GetDefaultValue(parameter) : null;

        public new string FullType { get; } = parameter.ParameterType.FullName.ToFormattedGenericName();

        private static string GetDefaultValue(ParameterInfo parameter)
        {
            if (parameter.DefaultValue is string s)
                return $"\"{s}\"";
            else if (parameter.DefaultValue == null)
                return "null";
            else if (parameter.DefaultValue is char c)
                return $"'{c}'";
            else if (parameter.DefaultValue is bool b)
                return b.ToString().ToLower();
            else if (parameter.DefaultValue.GetType().IsEnum)
                return $"{parameter.ParameterType.Name}.{parameter.DefaultValue}";
            else if (parameter.DefaultValue is IConvertible convertible)
                return convertible.ToString(CultureInfo.InvariantCulture);

            return parameter.DefaultValue.ToString();
        }
    }
}
