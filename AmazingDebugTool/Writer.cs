using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Concurrent;
using Exiled.API.Features;
using JITDebugTool.API.Features;
using System.Collections.Generic;
using System.Threading.Tasks;
using JITDebugTool.API.SerializedElements;
using System.Linq;
using Utf8Json.Resolvers.Internal;

namespace JITDebugTool
{
#pragma warning disable CS4014
    internal class Writer()
    {
        internal readonly ConcurrentQueue<CallEntry> fullLogs = [];

        private readonly ConcurrentQueue<CallEntry> _builder = [];

        public void Start()
        {
            Task.Run(Action);
        }

        public CallEntry PreWrite()
        {
            CallEntry entry = new();
            _builder.Enqueue(entry);
            fullLogs.Enqueue(entry);
            return entry;
        }

        public static void Write(CallEntry entry, object obj, Stopwatch stopwatch, MethodInfo method, StackTrace stackTrace)
        {
            try
            {
                entry.Populate(method, stopwatch, stackTrace, obj);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private async Task Action()
        {
            while (!Round.IsEnded)
            {
                if (!LogService.CanTransmit)
                    goto rtx;

                if (_builder.Count == 0)
                    goto rtx;

                try
                {
                    List<CallEntry> elements = [];

                    while (_builder.TryPeek(out CallEntry entry) && entry.IsReady && _builder.TryDequeue(out CallEntry _) && elements.Count < 11)
                        elements.Add(entry);

                    Task.Run(() =>
                    {
                        List<SerializedCallEntry> entries = [.. elements.Select<CallEntry, SerializedCallEntry>(e => new(e))];

                        LogService.Broadcast(SerializedCallEntry.Serialize(entries));
                    });
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }

                rtx:
                await Task.Delay(10);
            }
        }
    }
}