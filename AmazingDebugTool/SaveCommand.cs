using CommandSystem;
using System;

namespace AmazingDebugTool
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class SaveCommand : ICommand
    {
        public string Command => "adt-save";

        public string[] Aliases => [];

        public string Description => "Save the detailed log before it makes the RAM explode haha";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Plugin.Instance.Writer.Save();
            response = "Saved!";
            return true;
        }
    }
}
