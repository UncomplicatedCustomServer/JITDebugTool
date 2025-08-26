using System.Linq;
using System.Text;

namespace JITDebugTool
{
    internal static class StringExtension
    {
        public static string ToFormattedGenericName(this string str)
        {
            if (str == null)
                return string.Empty;

            int genericMarkerIndex = str.IndexOf('`');
            if (genericMarkerIndex == -1)
            {
                return str;
            }

            int argsStartIndex = str.IndexOf('[', genericMarkerIndex);
            if (argsStartIndex == -1)
            {
                return str;
            }

            string typeName = str.Substring(0, genericMarkerIndex);
            string argsCountStr = str.Substring(genericMarkerIndex + 1, argsStartIndex - (genericMarkerIndex + 1));

            string argsString = str.Substring(argsStartIndex + 2, str.Length - argsStartIndex - 4);

            var sb = new StringBuilder();
            sb.Append(typeName);
            sb.Append('[').Append(argsCountStr).Append(']');
            sb.Append('<');

            int bracketLevel = 0;
            int lastSplitIndex = 0;

            for (int i = 0; i < argsString.Length; i++)
            {
                char c = argsString[i];
                if (c == '[')
                {
                    bracketLevel++;
                }
                else if (c == ']')
                {
                    bracketLevel--;
                }
                else if (c == ',' && bracketLevel == 0)
                {
                    string argPart = argsString.Substring(lastSplitIndex, i - lastSplitIndex).Trim();
                    sb.Append(GetSimpleTypeName(argPart));
                    sb.Append(", ");
                    lastSplitIndex = i + 1;
                }
            }

            string lastArgPart = argsString.Substring(lastSplitIndex).Trim();
            sb.Append(GetSimpleTypeName(lastArgPart));

            sb.Append('>');

            string result = sb.ToString();

            if (result.Contains(","))
                return $"{result.Split(',')[0]}>{result.Split('>').Last()}";

            return sb.ToString();
        }

        private static string GetSimpleTypeName(string mangledTypeName)
        {
            int commaIndex = mangledTypeName.IndexOf(',');
            if (commaIndex != -1)
            {
                mangledTypeName = mangledTypeName.Substring(0, commaIndex);
            }

            int lastDotIndex = mangledTypeName.LastIndexOf('.');
            if (lastDotIndex != -1)
            {
                return mangledTypeName.Substring(lastDotIndex + 1);
            }

            return mangledTypeName;
        }
    }
}
