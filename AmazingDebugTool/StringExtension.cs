namespace JITDebugTool
{
    internal static class StringExtension
    {
        public static string FillToAlign(this string str, int length)
        {
            while (str.Length < length)
                str += ' ';

            return str;
        }
    }
}
