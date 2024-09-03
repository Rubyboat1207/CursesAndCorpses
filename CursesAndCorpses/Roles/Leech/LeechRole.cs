using System;
using System.Collections.Generic;
using System.Linq;
using MiraAPI.Roles;
using Reactor.Networking.Attributes;
using UnityEngine;

[RegisterCustomRole]
public class LeechRole : CrewmateRole, ICustomRole
{
    public string RoleName => "Leech";

    public string RoleDescription => "Feed off the souls of the dead.";

    public string RoleLongDescription => "Feed to give benifets to your friends";

    public Color RoleColor => new(0.37f, 0.39f, 0.41f, 1f);

    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;

    public bool hasIncreasedVision = false;
    public bool uploadBuff = false;
    public bool sabotageQuickFix = false;
    public List<LeechBenefit> CompletedBenifits = new();

    public LeechRole() {
        this.IntroSound = CursesAndCorpsesAssets.LeechIntroSound.LoadAsset();
    }
    

    [MethodRpc((uint) RpcHandler.CustomRpcCalls.LeechAbility)]
    public static void RpcActivateBenefit(PlayerControl player, int benefitIdx) {
        if(player.Data.Role is not LeechRole) {
            return;
        }
        LeechRole role = (LeechRole) player.Data.Role;
        LeechBenefit benefit = LeechBenefit.AllBenifits[benefitIdx];
        role.CompletedBenifits.Add(benefit);

        benefit.Action.Invoke(role);
        UpdateHUD(role.CompletedBenifits, benefit);
    }

    [MethodRpc((uint) RpcHandler.CustomRpcCalls.SoulLeech)]
    public static void RpcSoulLeech(PlayerControl _, byte bodyId) {
        Debug.Log("body byte: " + bodyId);
        DeadBody body = GameObject.FindObjectsOfType<DeadBody>().Where(b => b.ParentId == bodyId).FirstOrDefault();
        Debug.Log("body byte: " + body.ParentId);
        var reminant = new GameObject("BodyReminant");

        reminant.transform.position = body.transform.position;
        reminant.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        var renderer = reminant.AddComponent<SpriteRenderer>();

        renderer.sprite = CursesAndCorpsesAssets.SoulReminantSprite.LoadAsset();

        Destroy(body.gameObject);
    }

    public void SoulLeech(DeadBody deadBody) {
        Debug.Log("Sending RPC with: " + deadBody.ParentId);
        RpcSoulLeech(Player, deadBody.ParentId);

        var avaliableBenifits = LeechBenefit.AllBenifits.Where(b => (!CompletedBenifits.Contains(b)) && b.isAllowedToRun(this)).ToList();

        var benifit = avaliableBenifits[UnityEngine.Random.RandomRange(0, avaliableBenifits.Count)];

        CompletedBenifits.Add(benifit);
        Debug.Log("Benifit" + LeechBenefit.AllBenifits.IndexOf(benifit));

        RpcActivateBenefit(Player, LeechBenefit.AllBenifits.IndexOf(benifit));
    }

    public static void UpdateHUD(List<LeechBenefit> completed, LeechBenefit addedBenifit)
    {
        if (addedBenifit.Sprite == null)
        {
            return;
        }
        var hudManager = HudManager.Instance;

        GameObject leechHudStuff = GameObject.Find("LeechHUDStuff");

        if(!leechHudStuff)
        {
            leechHudStuff = new GameObject("LeechHUDStuff");

            leechHudStuff.AddComponent<RectTransform>();
            leechHudStuff.transform.SetParent(hudManager.TaskStuff.transform.GetChild(0));

            leechHudStuff.transform.localPosition = new Vector3(4.2f, -0.3f, 0);
        }

        GameObject leechBenifitEmblem = new GameObject("BenifitEmblem");
        var transform = leechBenifitEmblem.AddComponent<RectTransform>();
        var emblemRenderer = leechBenifitEmblem.AddComponent<SpriteRenderer>();

        emblemRenderer.sprite = addedBenifit.Sprite.LoadAsset();
        leechBenifitEmblem.layer = 5;
        transform.SetParent(leechHudStuff.transform);

        transform.localPosition = Vector3.zero;
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        transform.localPosition += new Vector3(0.6f, 0, 0) * (completed.Where(b => b.Sprite != null).Count() - 1);
    }
}