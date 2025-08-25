using JITDebugTool.API.SerializedElements;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace JITDebugTool.API.Features
{
#pragma warning disable IDE0306
#pragma warning disable IDE0305

    internal class LogService : WebSocketBehavior
    {
        internal static bool CanTransmit => _authed.Count > 0;

        private static readonly List<LogService> _authed = [];

        private bool _isAuthed = false;

        protected override void OnMessage(MessageEventArgs e)
        {
            if (!_isAuthed)
                if (e.Data == Plugin.Instance.Config.SocketKey)
                {
                    _isAuthed = true;
                    SendAsync($"WELCOME! {Plugin.Instance.Name} v{Plugin.Instance.Version}", delegate { });
                    SendAsync(SerializedCallEntry.Serialize(new SerializedWelcome()), delegate { });
                } 
                else
                {
                    SendAsync("WRONG KEY!", delegate { });
                }
            else
            {
                if (e.Data == "GET_EVENT_CHRONO")
                {
                    Exiled.API.Features.Log.Info("GETTING CHRONO v2!");
                    try
                    {
                        Task.Run(() =>
                        {
                            try
                            {
                                Queue<CallEntry> queue = new(Plugin.Instance.writer.fullLogs.ToArray());

                                Exiled.API.Features.Log.Info($"PREPPING LOGS AS {queue.Count}! ({Plugin.Instance.writer.fullLogs.Count})");

                                while (queue.Count > 0)
                                {
                                    List<SerializedCallEntry> elements = [];
                                    while (queue.TryDequeue(out CallEntry entry) && elements.Count < 15 && entry is not null)
                                        elements.Add(new(entry));

                                    SendAsync(SerializedCallEntry.Serialize(elements), delegate { });
                                }
                            }
                            catch (Exception ex)
                            {
                                Exiled.API.Features.Log.Error(ex);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        Exiled.API.Features.Log.Error(ex);
                    }
                }
                else if (e.Data == "SUBSCRIBE")
                    _authed.Add(this);
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            _authed.Remove(this);
        }

        public static void Broadcast(string message)
        {
            foreach (LogService log in _authed)
                if (log.Context.WebSocket.IsAlive)
                    log.SendAsync(message, delegate { });
        }
    }
}
