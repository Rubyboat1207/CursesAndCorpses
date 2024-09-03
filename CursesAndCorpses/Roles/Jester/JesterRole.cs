

using MiraAPI.Roles;
using UnityEngine;

[RegisterCustomRole]
public class JesterRole : CrewmateRole, ICustomRole
{
    public string RoleName => "Fae";
    public string RoleDescription => "Try to get voted out";
    public string RoleLongDescription => "Try to get voted out";
    public Color RoleColor => new(248/255f, 127/255f, 80/255f);
    public ModdedRoleTeams Team => ModdedRoleTeams.Neutral;

    public bool votedOut = false;

    public JesterRole() {
        this.IntroSound = CursesAndCorpsesAssets.JesterIntroSound.LoadAsset();
    }

    public override bool DidWin(GameOverReason gameOverReason)
    {
        // once mira 2.0.0 releases, replace this with the better stuff
        return (int) gameOverReason == 9;
    }
}