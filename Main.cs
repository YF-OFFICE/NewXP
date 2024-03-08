using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Playert = Exiled.Events.Handlers.Player;

namespace NewXp
{
    public class Config : IConfig
    {
        [Description("Plugin true/false")]
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
    }
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "NewXp";
        public override string Author { get; } = "tyIUc";
        public static string dic  = Path.Combine(Exiled.API.Features.Paths.Configs, "NewXP")+"\\"; 
        public override Version Version { get; } = new Version(1, 0, 0);
        public override void OnDisabled()
        {
            base.OnDisabled();
        }
        public override void OnEnabled()
        {     Log.Warn(dic);
            base.OnEnabled();
        }


    }
}
