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

namespace JITDebugTool
{
#pragma warning disable CS4014
    internal class Writer()
    {
        private readonly ConcurrentQueue<CallEntry> _builder = new();

        public void Start()
        {
            Task.Run(Action);
        }

        public void Write(object obj, Stopwatch stopwatch, MethodInfo method, object[] _, StackTrace stackTrace)
        {
            try
            {
                _builder.Enqueue(new(method, stopwatch, stackTrace, obj));
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

                    while (_builder.TryDequeue(out CallEntry entry) && elements.Count < 11)
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