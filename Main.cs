using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs.Player;
using NewXp.IniApi;
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
        public static string dic  = Path.Combine(Exiled.API.Features.Paths.Configs, "NewXP")+ "\\Ssave.ini";
        public static IniFile IniFilea { get; private set; } 
        public override Version Version { get; } = new Version(1, 0, 0);
        public override void OnEnabled()
        {    
            Log.Warn("经验系统储存目录"+dic);
            if (!Directory.Exists(dic.Replace("\\Ssave.ini", "")))
            {
                Directory.CreateDirectory(dic.Replace("\\Ssave.ini", ""));
                Log.Warn("初始化");
            
            }
            if (!File.Exists(dic))
            { 
               var test = new IniFile();
            test.Section("XpSave").Set("test","test");
            test.Section("LvSave").Set("test","test");
            test.Save(dic);
               
            }
           IniFilea = new IniFile(dic);
         
            Log.Info("插件存储连接成功");
            

            base.OnEnabled();
        }


    }
}
