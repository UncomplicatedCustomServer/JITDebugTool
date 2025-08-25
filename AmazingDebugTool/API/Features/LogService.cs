using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace JITDebugTool.API.Features
{
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
                    _authed.Add(this);

                    SendAsync($"WELCOME! {Plugin.Instance.Name} v{Plugin.Instance.Version}", delegate { });
                } 
                else
                {
                    SendAsync("WRONG KEY!", delegate { });
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
