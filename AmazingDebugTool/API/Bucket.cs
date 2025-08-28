using JITDebugTool.API.SerializedElements;
using System.Collections.Generic;

namespace JITDebugTool.API
{
    internal class Bucket
    {
        public static readonly Dictionary<string, SerializedMethod> methods = [];

        public static readonly Dictionary<string, SerializedMethod> stackTraceMethodCache = [];
    }
}
