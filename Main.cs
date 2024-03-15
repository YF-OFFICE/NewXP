using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.Commands.Reload;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Handlers;
using NewXp.IniApi;
using PlayerRoles;
using PluginAPI.Roles;
using RemoteAdmin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Play = Exiled.Events.Handlers.Player;
using Player = Exiled.API.Features.Player;
using Server = Exiled.Events.Handlers.Server;

namespace NewXp
{
    public class exp: ICommand
    {

        public string Command { get; } = "exp";


        public string[] Aliases { get; } = new string[]
        {
            "exp","xp"
        };


        public string Description { get; } = "查询个人等级及经验";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get(sender);

            if (player == null)
            {
                response = "null";
                return false;
            }
            else if (Plugin.IniFilea.Section("LvSave").Get(player.UserId) != null)
            {
                response = $"====[XP系统*查询成功√]====\n你的经验为{Plugin.IniFilea.Section("XpSave").Get(player.UserId)}/{Plugin.XpToV(player.UserId)}\n目前等级为:{Plugin.IniFilea.Section("LvSave").Get(player.UserId)}\n============";
                return true;
            }
            else
            {
                response = "null";
                return false;
            }
        }
    }
    public class Config : IConfig
    {
        [Description("Plugin true/false")]
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        //升级需要多少经验 如:1*100
        public int XPtoLevel { get; set; } = 100;
        public int KSCP { get; set; } = 600;
        public int KDD { get; set; } = 100;
        public int KNTF{ get; set; } = 200;
        //击杀混沌
        public int KHD { get; set; } = 200;
        //击杀博士
        public int KBS { get; set; } = 100;
        //击杀九尾
        public int KJW { get; set; } = 150;
        //经验倍数
        public int Point { get; set; } = 1;
        //可选参数xp为经验,tolv为升下一级所需经验,lv为目前等级 可以自己加color代码
        public string JoinText { get; set; } = "欢迎来到本服务器\n $你目前经验为xp/tolv 等级为lv \n祝你玩得开心";
        //是否打开exp指令
        public bool OpenCommand { get; set; } = true;

    }
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "NewXp";
        public override string Author { get; } = "tyIUc";
        public static string dic = Path.Combine(Exiled.API.Features.Paths.Configs, "NewXP") + "\\Ssave.ini";
        public static IniFile IniFilea { get; private set; }
        public override Version Version { get; } = new Version(1, 0, 0);
        public static Plugin Plugin1;
        public override void OnEnabled()
        {
            Plugin1 = this;
            Log.Warn("经验系统储存目录" + dic);
            if (!Directory.Exists(dic.Replace("\\Ssave.ini", "")))
            {
                Directory.CreateDirectory(dic.Replace("\\Ssave.ini", ""));
                Log.Warn("初始化");

            }
            if (!File.Exists(dic))
            {
                var test = new IniFile();
                test.Section("XpSave").Set("test", "test");
                test.Section("LvSave").Set("test", "test");
                test.Save(dic);

            }
            IniFilea = new IniFile(dic);
            if (Config.OpenCommand)
            {
                QueryProcessor.DotCommandHandler.RegisterCommand(new exp());
            }
            Log.Info("插件存储连接成功");
            Play.Died += die;
            Play.Joined += Join;
            Server.WaitingForPlayers += Wait;
            base.OnEnabled();
        }
        public void Wait()
        {
            IniFilea = new IniFile(dic);
            Log.Debug("已经重新加载数据");
        }
        public void Join(JoinedEventArgs ev)
        {
            if (IniFilea.Section("XpSave").Get(ev.Player.UserId) == null || IniFilea.Section("LvSave").Get(ev.Player.UserId) == null)
            {
                IniFilea.Section("XpSave").Set(ev.Player.UserId, "100");
                IniFilea.Section("LvSave").Set(ev.Player.UserId, "1");
                IniFilea.Save(dic);
            }
            ev.Player.DisplayNickname = $"[Lv.{IniFilea.Section("LvSave").Get(ev.Player.UserId)}]"+ev.Player.Nickname;
            ev.Player.Broadcast(5, Config.JoinText.Replace("xp", IniFilea.Section("XpSave").Get(ev.Player.UserId)).Replace("tolv", XpToV(ev.Player.UserId).ToString()).Replace("lv", IniFilea.Section("LvSave").Get(ev.Player.UserId))) ;
        }
        public void die(DiedEventArgs ev)
        {
            if (ev.Attacker != null && ev.Player != null && ev.Player != ev.Attacker)
            {
                switch (ev.Player.Role.Team)
                {
                    case PlayerRoles.Team.SCPs:
                        AddExp(ev.Attacker,Config.KSCP);
                        break;
                    case PlayerRoles.Team.FoundationForces:
                        if (ev.Player.Role == RoleTypeId.FacilityGuard)
                        {
                            AddExp(ev.Attacker, Config.KJW);
                        }
                        else
                        {
                            AddExp(ev.Attacker, Config.KNTF);
                        }
                        break;
                    case PlayerRoles.Team.ChaosInsurgency:
                        AddExp(ev.Attacker, Config.KHD);
                        break;
                    case PlayerRoles.Team.Scientists:
                        AddExp(ev.Attacker, Config.KBS);
                        break;
                    case PlayerRoles.Team.ClassD:
                        AddExp(ev.Attacker, Config.KDD);
                        break;
                    default:
                        break;
                }
            }

        }
        public static int XpToV(string use) => Plugin1.Config.XPtoLevel * IniFilea.Section("LvSave").Get<int>(use);
        public static void AddExp(Player e,int xp1)
        {
            var xp = xp1 * Plugin1.Config.Point;
            if (IniFilea.Section("XpSave").Get(e.UserId) == null || IniFilea.Section("LvSave").Get(e.UserId) == null)
            {
                IniFilea.Section("XpSave").Set(e.UserId, "100");
                IniFilea.Section("LvSave").Set(e.UserId, "1");
                IniFilea.Save(dic);
            }

                var xps = IniFilea.Section("XpSave").Get<int>(e.UserId);
                var lvs = IniFilea.Section("LvSave").Get<int>(e.UserId);
                if (xps + xp >= XpToV(e.UserId))
                {
                    lvs++;
                    xps = xps - XpToV(e.UserId);
                }
                else
                {
                    xps += xp;
                }
                e.ShowHint($"增加经验{xp} \n目前经验为{xps} 等级为{lvs} 距离下一级还差{XpToV(e.UserId)-xps} ComeOn!!");
                e.SendConsoleMessage($"增加经验{xp} \n目前经验为{xps} 等级为{lvs} 距离下一级还差{XpToV(e.UserId) - xps} ComeOn!!","yellow");
                e.Broadcast(7, $"增加经验{xp} \n目前经验为{xps} 等级为{lvs} 距离下一级还差{XpToV(e.UserId) - xps} ComeOn!!");
                Log.Debug($"玩家{e.Nickname}增加经验{xp} \n目前经验为{xps} 等级为{lvs} 距离下一级还差{XpToV(e.UserId) - xps} ComeOn!!");

            

         
        

        }

    }
}
