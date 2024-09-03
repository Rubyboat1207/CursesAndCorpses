
using System.Collections.Generic;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using UnityEngine;

[RegisterCustomRole]
class WitchRole : ImpostorRole, ICustomRole
{
    public string RoleName => "Witch";

    public string RoleDescription => "Curse Crewmates";

    public string RoleLongDescription => "Use your curse ability to cause crewmates to die after the meeting.";

    public Color RoleColor => Color.red;

    public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;

    public List<PlayerControl> cursedPlayers = new();

    public WitchRole() {
        this.IntroSound = CursesAndCorpsesAssets.WitchIntroSound.LoadAsset();
    }

    [MethodRpc((uint) RpcHandler.CustomRpcCalls.WitchCurse)]    
    public static void RpcCursePlayer(PlayerControl witch, PlayerControl target) => ((WitchRole) witch.Data.Role).cursedPlayers.Add(target);
}