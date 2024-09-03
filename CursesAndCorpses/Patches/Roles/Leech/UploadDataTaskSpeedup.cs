using System.Linq;
using HarmonyLib;
using UnityEngine;

[HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
public class UploadDataTaskSpeedup
{
    private static void Postfix(HudManager __instance)
    {
        PlayerControl leech = PlayerControl.AllPlayerControls.ToArray().Where((player) => player.Data.Role is LeechRole).FirstOrDefault();
        if (leech == null) return;

        LeechRole role = (LeechRole) leech.Data.Role;
        if (role.uploadBuff)
        {
            var uploadData = __instance.transform.parent.GetComponentInChildren<UploadDataGame>();
            if(uploadData != null)
            {
                uploadData.timer += 1 * Time.deltaTime;
            }
        }
    }
}