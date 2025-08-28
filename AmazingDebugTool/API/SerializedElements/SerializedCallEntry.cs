using JITDebugTool.API.Features;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JITDebugTool.API.Extensions;

namespace JITDebugTool.API.SerializedElements
{
    internal class SerializedCallEntry
    {
        public string Id { get; }

        public string Method { get; }

        public string Assembly { get; }

        public List<SerializedStackTraceEntry> StackTrace { get; }

        public SerializedStopwatch Stopwatch { get; }

        public string SpecificId { get; }

        public double RamUsage { get; }

        public long ExecutionTime { get; }

        public SerializedCallEntry(CallEntry entry)
        {
            Id = entry.Id.ToString();
            Method = entry.Method.GetSignature();
            Assembly = entry.Object?.GetType().Assembly.FullName ?? entry.Method.DeclaringType.Assembly.FullName;
            StackTrace = SerializeStackTrace(entry.StackTrace);
            Stopwatch = new(entry.Stopwatch);

            SpecificId = Method;

            Process process = Process.GetCurrentProcess();
            RamUsage = process.PrivateMemorySize64 / (1024.0 * 1024);

            ExecutionTime = entry.Time.ToUnixTimeMilliseconds();
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                },
                Formatting = Formatting.Indented
            });
        }

        private static List<SerializedStackTraceEntry> SerializeStackTrace(StackTrace source)
        {
            if (source == null || source.FrameCount < 3)
                return [];

            return [.. source.GetFrames().Skip(2).Select(f => new SerializedStackTraceEntry(f))];
        }
    }
}
