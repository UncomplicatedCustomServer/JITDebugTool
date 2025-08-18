using Exiled.API.Interfaces;
using System.Collections.Generic;

namespace AmazingDebugTool
{
    internal class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; } = false;

        public string Plugin { get; set; } = "UncomplicatedCustomRoles";

        public List<string> IgnoreTypes { get; set; } = [];

        public List<string> IgnoreMethods { get; set; } = [];

        public bool Detailed { get; set; } = false;

        public bool Grid { get; set; } = true;

        public bool ShowArgs { get; set; } = true;
    }
}
