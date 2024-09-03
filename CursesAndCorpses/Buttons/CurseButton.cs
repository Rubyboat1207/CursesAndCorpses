

using MiraAPI.Hud;
using MiraAPI.Utilities.Assets;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using MiraAPI.GameOptions;

[RegisterButton]
public class CurseButton : CustomActionButton<PlayerControl>
{
    public override string Name => "Curse";

    public override float Cooldown => 15f;

    public override float EffectDuration => 0;

    public override int MaxUses => 0;

    public override LoadableAsset<Sprite> Sprite => CursesAndCorpsesAssets.CurseButton;

    public override bool Enabled(RoleBehaviour role)
    {
        return role is WitchRole;
    }

    public override PlayerControl GetTarget()
    {
        List<PlayerControl> potentialTargets = new(PlayerControl.AllPlayerControls.ToArray());

        WitchRole role = (WitchRole) PlayerControl.LocalPlayer.Data.Role;

        potentialTargets = potentialTargets.Where(pc => !role.cursedPlayers.Contains(pc) && !pc.Data.Role.IsImpostor).ToList();

        var localTruePosition = PlayerControl.LocalPlayer.GetTruePosition();

        potentialTargets.Sort((a, b) => Vector3.Distance(a.GetTruePosition(), localTruePosition).CompareTo(Vector3.Distance(b.GetTruePosition(), localTruePosition)));

        var potentialTarget = potentialTargets.FirstOrDefault();

        if(potentialTarget != null && Vector3.Distance(potentialTarget.GetTruePosition(), localTruePosition) < role.GetAbilityDistance()) {
            return potentialTargets.FirstOrDefault();
        }


        return null;
    }

    public override void SetOutline(bool active)
    {
        Target?.cosmetics.SetOutline(active, new Il2CppSystem.Nullable<Color>(Color.red));
    }

    protected override void OnClick()
    {
        if(Target == null) {
            return;
        }
        WitchRole.RpcCursePlayer(PlayerControl.LocalPlayer, Target);
    }
}