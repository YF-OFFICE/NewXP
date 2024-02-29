using CommandSystem.Commands.RemoteAdmin.ServerEvent;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events;
using Exiled.Events.EventArgs.Player;
using MEC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestingPlugin
{
    public enum killXP
    { 
      Normal,
      SCP,
      NTF,
      Chaos,
      DD,
      Scientist
    }
    public class Mconfig
    { 
         public Dictionary<killXP,int> keyValuePairs  = new Dictionary<killXP, int>() { { killXP.Normal,100} };
        public Dictionary<string, int> LevelSave = new Dictionary<string , int>() { { "tset@strie",1} };
        public Dictionary<string, int> XpSave = new Dictionary<string, int>() { { "tset@strie", 100 } };
    }
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public string JoinText { get; set; } = "欢迎加入本服务器 你目前的等级为@lev 经验为@xp 祝你游玩愉快";
        public string NewJoinText { get; set; } = "欢迎加入本服务器 你目前的等级为@lev 经验为@xp 祝你游玩愉快";
        public int UpLevel { get; set; } = 200;
        public Mconfig Mconfig { get; set; } = new Mconfig();

    }
    public class Class2: Plugin<Config>
    {
        public override string Author => "YF-OFFICE";
        public override Version Version => new Version(1, 0, 0);
        public override string Name => "XpSystem";
        public static Class2 plugin;
        public override void OnEnabled()
        {
            plugin = this;
            Exiled.Events.Handlers.Server.RoundStarted += this.RoundStarted;
            Exiled.Events.Handlers.Player.Died += this.Died;
            Exiled.Events.Handlers.Player.Joined+= this.join;
            Log.Info("加载插件中");
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= this.RoundStarted;
            Exiled.Events.Handlers.Player.Died -= this.Died;
            Exiled.Events.Handlers.Player.Joined -= this.join;
            plugin = null;
            Log.Info("插件关闭了");
            base.OnDisabled();
        }
        public void RoundStarted()
        {
         
        }
        public void join(JoinedEventArgs ev)
        {
            if (this.Config.Mconfig.LevelSave.TryGetValue(ev.Player.UserId, out int va) && this.Config.Mconfig.XpSave.TryGetValue(ev.Player.UserId, out int va1))
            {
                ev.Player.Broadcast(7, this.Config.JoinText.Replace("@lev", va.ToString()).Replace("@xp", va1.ToString()));
                   ev.Player.DisplayNickname = $"[Lv.{va}]"+ev.Player.Nickname;
            }
            else
            {
                this.Config.Mconfig.LevelSave.Add(ev.Player.UserId,1);
                this.Config.Mconfig.XpSave.Add(ev.Player.UserId,100);
                 ev.Player.Broadcast(7, this.Config.NewJoinText.Replace("@lev", "1").Replace("@xp", "100"));
                ev.Player.DisplayNickname = $"[Lv.1]" + ev.Player.Nickname;
            }
            
        }
        public static void AddXp(Player Add,killXP killXP)
        {
            var config = plugin.Config;
            config.Mconfig.XpSave.TryGetValue(Add.UserId,out int vo);
            config.Mconfig.LevelSave.TryGetValue(Add.UserId, out int vo1);
            int should = 0;
            if (config.Mconfig.keyValuePairs.TryGetValue(killXP, out int shouldadd))
            {
                should = shouldadd;
            }
            else
            {
                config.Mconfig.keyValuePairs.TryGetValue(killXP.Normal, out var shouldw);
                should = shouldw;
            }
            if (vo + should >= config.UpLevel * vo1)
            {
                vo1++;
                vo = (vo + should)-config.UpLevel * vo1;
            }
           else
            {
                vo = vo + should;
            }
            config.Mconfig.XpSave[Add.UserId] = vo;
            config.Mconfig.LevelSave[Add.UserId] = vo1;
            Add.ShowHint($"你击杀了{killXP.ToString()}增加经验{should} \n目前经验为{vo} 等级为{vo1} 距离下一级还差{config.UpLevel * vo1-vo}");
            Add.Broadcast(7,$"你击杀了{killXP.ToString()}增加经验{should} \n目前经验为{vo} 等级为{vo1} 距离下一级还差{config.UpLevel * vo1 - vo}");
            Log.Debug($"玩家{Add.Nickname} 增加经验{should} \n目前经验为{vo} 等级为{vo1} 距离下一级还差{config.UpLevel * vo1 - vo} 类型{killXP.ToString()}");
        }
        public void Died(DiedEventArgs ev)
        {
            if (ev.Attacker != ev.Player && ev.Player != null && ev.Attacker != null)
            {
                switch (ev.Player.Role.Team)
                {
                    case PlayerRoles.Team.SCPs:
                        AddXp(ev.Attacker,killXP.SCP);
                        break;
                    case PlayerRoles.Team.FoundationForces:
                        AddXp(ev.Attacker, killXP.NTF);
                        break;
                    case PlayerRoles.Team.ChaosInsurgency:
                        AddXp(ev.Attacker, killXP.Chaos);
                        break;
                    case PlayerRoles.Team.Scientists:
                        AddXp(ev.Attacker, killXP.Scientist);
                        break;
                    case PlayerRoles.Team.ClassD:
                        AddXp(ev.Attacker, killXP.DD);
                        break;
                    default:
                        break;
                }
            }


        }
    }
}
