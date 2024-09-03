using System.Linq;
using HarmonyLib;

[HarmonyPatch(typeof(TuneRadioMinigame), nameof(TuneRadioMinigame.Update))]
public class CommunicationQuickFix
{
    [HarmonyPostfix]
    public static void Postfix(TuneRadioMinigame __instance)
    {
        if (__instance.Tolerance == 0.4f)
        {
            return;
        }
        PlayerControl leech = PlayerControl.AllPlayerControls.ToArray().Where((player) => player.Data.Role is LeechRole).FirstOrDefault();
        if (leech == null) return;

        LeechRole role = (LeechRole) leech.Data.Role;
        if (role.sabotageQuickFix)
        {
            __instance.Tolerance = 0.4f;
        }
    }
}

[HarmonyPatch(typeof(AuthGame), nameof(AuthGame.Enter))]
public class CommunicationQuickFix2
{
    [HarmonyPostfix]
    public static void Postfix(AuthGame __instance)
    {
        PlayerControl leech = PlayerControl.AllPlayerControls.ToArray().Where((player) => player.Data.Role is LeechRole).FirstOrDefault();
        if (leech == null) return;

        LeechRole role = (LeechRole) leech.Data.Role;
        if(role.sabotageQuickFix)
        {
            if(__instance.amClosing == Minigame.CloseState.Closing)
            {
                ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Comms, 16);
            }
        }
    }
}