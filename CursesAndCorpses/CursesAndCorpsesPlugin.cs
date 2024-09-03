using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using MiraAPI;
using MiraAPI.PluginLoading;
using Reactor;
using Reactor.Networking.Attributes;
using Reactor.Utilities;

namespace CursesAndCorpses;

[BepInAutoPlugin]
[BepInProcess("Among Us.exe")]
[BepInDependency(ReactorPlugin.Id)]
[BepInDependency(MiraApiPlugin.Id)]
[ReactorModFlags(Reactor.Networking.ModFlags.RequireOnAllClients)]
public partial class CursesAndCorpsesPlugin : BasePlugin, IMiraPlugin
{
    public Harmony Harmony { get; } = new(Id);

    public string OptionsTitleText => "Curses And Corpses";

    public ConfigFile GetConfigFile() => Config;


    public override void Load()
    {
        Harmony.PatchAll();
    }
}
