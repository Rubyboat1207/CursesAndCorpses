using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using MiraAPI.Roles;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

[HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
public class AssasinPatches {
    public static GameObject GenerateButton(PlayerVoteArea area, Sprite sprite, Vector3 location, Vector3 scale, UnityAction call) {
        var confirmButton = area.Buttons.transform.GetChild(0).gameObject;

        var parent = confirmButton.transform.parent.parent;

        var button = GameObject.Instantiate(confirmButton, area.transform);
        var buttonRenderer = button.GetComponent<SpriteRenderer>();
        
        buttonRenderer.sprite = sprite;
        button.transform.localPosition = location;
        button.transform.localScale = scale;

        button.layer = 5;
        button.transform.parent = parent;

        var btnClickEvent = new Button.ButtonClickedEvent();
        btnClickEvent.AddListener(call);
        button.GetComponent<PassiveButton>().OnClick = btnClickEvent;

        var buttonCollider = button.GetComponent<BoxCollider2D>();
        buttonCollider.size = buttonRenderer.sprite.bounds.size;
        buttonCollider.offset = Vector2.zero;
        Object.Destroy(button.transform.GetChild(0).gameObject);

        return button;
    }

    public static void UpdateGuessIndex(TMPro.TextMeshPro text, bool increment) {
        var roles = RoleManager.Instance.AllRoles.Where(r => r.NiceName != "STRMISS" && r.TeamType != RoleTeamTypes.Impostor).ToList();

        RoleBehaviour currentGuess = null;
        int idx = 0;
        foreach(var role in roles) {
            if(role.NiceName == "STRMISS") {
                idx++;
                continue;
            }
            if(role.TeamType == RoleTeamTypes.Impostor) {
                idx++;
                continue;
            }
            if(role.NiceName == text.text) {
                currentGuess = role;
                break;
            }
            idx++;
        }
        if(currentGuess == null) {
            Debug.Log("was null");
            currentGuess = roles[0];
            idx = 0;
        }

        Debug.Log(currentGuess.NiceName);

        int offset = increment ? 1 : -1;

        Debug.Log("Len: " + roles.Count + " idx: " + idx);

        RoleBehaviour nextGuess;

        if(idx + offset >= roles.Count) {
            nextGuess = roles[0];
        }
        else if(idx + offset < 0) {
            nextGuess = roles[roles.Count - 1];
        }else {
            nextGuess = roles[idx + offset];
        }


        text.text = nextGuess.NiceName;
        if(nextGuess is ICustomRole customRole) {
            text.color = customRole.RoleColor;
        }else {
            text.color = nextGuess.TeamColor;
        }
    }

    public static void Guess(PlayerControl target, string friendlyRoleName) {
        if(target.Data.IsDead) {
            return;
        }
        if(target.Data.Role.NiceName == friendlyRoleName) {
            PlayerControl.LocalPlayer.RpcMurderPlayer(target, true);
        }else {
            PlayerControl.LocalPlayer.RpcMurderPlayer(PlayerControl.LocalPlayer, true);
        }
    }

    public static void CreateAssasinButton(PlayerVoteArea voteArea) {
        var nameText = Object.Instantiate(voteArea.NameText, voteArea.transform);
        voteArea.NameText.transform.localPosition = new Vector3(0.55f, 0.12f, -0.1f);
        nameText.transform.localPosition = new Vector3(0.55f, -0.12f, -0.1f);
        nameText.text = "Guess";
        
        GenerateButton(
            voteArea,
            CursesAndCorpsesAssets.PreviousAssasinButton.LoadAsset(),
            new Vector3(-0.5f, 0.15f, -2f), // position
            new Vector3(0.8f, 0.8f, 0.8f), // scale
            (System.Action) (() => UpdateGuessIndex(nameText, false))
        );

        GenerateButton(
            voteArea,
            CursesAndCorpsesAssets.NextAssasinButton.LoadAsset(),
            new Vector3(-0.2f, 0.15f, -2f), // position
            new Vector3(0.8f, 0.8f, 0.8f), // scale
            (System.Action) (() => UpdateGuessIndex(nameText, true))
        );

        var player = new List<PlayerControl>(PlayerControl.AllPlayerControls.ToArray()).Where(pc => pc.PlayerId == voteArea.TargetPlayerId).FirstOrDefault();

        GenerateButton(
            voteArea,
            CursesAndCorpsesAssets.GuessAssasinButton.LoadAsset(),
            new Vector3(-0.35f, -0.15f, -2f), // position
            new Vector3(0.8f, 0.8f, 0.8f), // scale
            (System.Action) (() => Guess(player, nameText.text))
        );
    }

    public static void Postfix(MeetingHud __instance) {
        if(PlayerControl.LocalPlayer.Data.IsDead) {
            return;
        }
        if(!PlayerControl.LocalPlayer.Data.Role.IsImpostor) {
            return;
        }
        
        foreach(var voteArea in __instance.playerStates) {
            CreateAssasinButton(voteArea);
        }
    }
}