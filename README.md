=============NewXP`SCPSL插件=============

<a href="https://github.com/YF-OFFICE/NewXP/releases"><img src="https://img.shields.io/github/v/release/YF-OFFICE/NewXP?display_name=tag&style=for-the-badge&logo=gitbook&label=Release" href="https://github.com/YF-OFFICE/NewXP/releases" alt="GitHub Releases"></a>
<img src="https://img.shields.io/github/downloads/YF-OFFICE/NewXP/total?style=for-the-badge&logo=github" alt="Downloads">

本插件运用Ini文件存储玩家经验 自带广播，经验自定义配置，经验倍数功能 

在config里可以修改
注释:
~~~~   //升级需要多少经验 如:1*100
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
~~~~

安装教程:下载[最新版本dll](https://github.com/YF-OFFICE/XpSystem/releases)丢到ex plugin文件夹重启服务器即可 ！！！！！！So easy

如有问题和建议可以在[Iss](https://github.com/YF-OFFICE/XpSystem/issues)里提出 不定时偷看

本项目由[YFOFFICE](https://github.com/YF-OFFICE)独家打造 ！！！！！！！

### 开发者

感谢以下开发者对 NewXP作出的贡献：

<a href="https://github.com/YF-OFFICE/NewXP/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=YF-OFFICE/NewXP&max=1000" alt="contributors" />
</a>



==================我是底线=============



