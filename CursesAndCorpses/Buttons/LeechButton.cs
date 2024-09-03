using MiraAPI.Hud;
using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using System.Linq;
using UnityEngine;

[RegisterButton]
public class LeechButton : CustomActionButton<DeadBody>
{
    public override string Name => "Leech";

    public override float Cooldown => 15;

    public override float EffectDuration => 0;

    public override int MaxUses => 0;

    public override LoadableAsset<Sprite> Sprite => CursesAndCorpsesAssets.LeechButtonSprite;

    public override bool Enabled(RoleBehaviour role)
    {
        return role is LeechRole;
    }

    public override DeadBody GetTarget()
    {
        var deadBodies = GameObject.FindObjectsOfType<DeadBody>()
            .Where(body => Vector3.Distance(body.TruePosition, PlayerControl.LocalPlayer.transform.position) < PlayerControl.LocalPlayer.Data.Role.GetAbilityDistance()).ToList();

        deadBodies.Sort((a, b) =>
                Vector3.Distance(a.TruePosition, PlayerControl.LocalPlayer.transform.position)
                    .CompareTo(Vector3.Distance(b.TruePosition, PlayerControl.LocalPlayer.transform.position)));

        return deadBodies.FirstOrDefault();
    }

    public override void SetOutline(bool active)
    {
        if(Target == null)
        {
            return;
        }
        SpriteRenderer spriteRenderer = Target.bodyRenderers.LastOrDefault();
        spriteRenderer.material.SetFloat("_Outline", active ? 1 : 0);
        spriteRenderer.material.SetColor("_OutlineColor", Color.blue);
    }

    protected override void OnClick()
    {
        LeechRole role = (LeechRole) PlayerControl.LocalPlayer.Data.Role;

        if(Target != null) {
            role.SoulLeech(Target);
        }
    }
}