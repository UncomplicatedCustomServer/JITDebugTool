using System.Collections.Generic;

namespace JITDebugTool.API.SerializedElements
{
    internal class SerializedPluginData(List<string> types, long runningSince, int totalPatchedTypes)
    {
        public List<string> Types { get; } = types;

        public List<SerializedMethod> Methods { get; } = [];

        public List<SerializedMethod> NotPatchedMethods { get; } = [];

        public long RunningSince { get; } = runningSince;

        public int TotalPatchedMethods { get; internal set; } = 0;

        public int TotalPatchedTypes { get; } = totalPatchedTypes;
    }
}
