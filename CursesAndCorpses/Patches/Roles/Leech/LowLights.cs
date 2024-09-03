
using System.Linq;
using HarmonyLib;

[HarmonyPatch(typeof(ShipStatus), nameof(ShipStatus.CalculateLightRadius))]
public static class LowLights {
    public static void Postfix(ShipStatus __instance, [HarmonyArgument(0)] NetworkedPlayerInfo player, ref float __result) {
        PlayerControl leech = PlayerControl.AllPlayerControls.ToArray().Where((player) => player.Data.Role is LeechRole).FirstOrDefault();

        if(leech != null) {
            LeechRole role = (LeechRole) leech.Data.Role;

            if(role.hasIncreasedVision) {
                __result += 0.25f;
            }
        }
    }
}