using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Il2CppSystem.Linq;
using UnityEngine;

[HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
public class AddCurseEmblem
{
    public static void Postfix(MeetingHud __instance)
    {
        List<WitchRole> witches = PlayerControl.AllPlayerControls.ToArray()
            .Where((player) => !player.Data.IsDead && player.Data.Role is WitchRole)
            .Select(pc => (WitchRole) pc.Data.Role).ToList();
        
        List<byte> cursedPlayerIds = witches.SelectMany(w => w.cursedPlayers).Select(pc => pc.PlayerId).ToList();

        foreach (var state in __instance.playerStates)
        {
            if(cursedPlayerIds.Contains(state.TargetPlayerId))
            {
                RectTransform originalRectTransform = state.NameText.GetComponent<RectTransform>();

                GameObject templateGO = new GameObject("CurseEmblem");
                templateGO.AddComponent<RectTransform>();
                var templateImg = templateGO.AddComponent<SpriteRenderer>();

                Object.Destroy(templateGO);

                templateImg.sprite = CursesAndCorpsesAssets.CurseEmblem.LoadAsset();
                GameObject curseEmblemGO = Object.Instantiate(templateGO, state.transform, false);
                curseEmblemGO.layer = 5;

                var transform = curseEmblemGO.GetComponent<RectTransform>();

                transform.localPosition = new Vector3(1,0,0);
                transform.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            }
        }
    }
}