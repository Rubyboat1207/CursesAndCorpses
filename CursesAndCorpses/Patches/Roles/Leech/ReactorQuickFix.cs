using System.Linq;
using HarmonyLib;

[HarmonyPatch(typeof(ReactorMinigame), nameof(ReactorMinigame.FixedUpdate))]
public class ReactorQuickFix
{
    [HarmonyPostfix]
    public static void Postfix(ReactorMinigame __instance)
    {
        if(__instance.isButtonDown)
        {
            PlayerControl leech = PlayerControl.AllPlayerControls.ToArray().Where((player) => player.Data.Role is LeechRole).FirstOrDefault();
            if (leech == null) return;

            LeechRole role = (LeechRole) leech.Data.Role;
            if (role.sabotageQuickFix)
            {
                ShipStatus.Instance.RpcUpdateSystem(SystemTypes.Reactor, 16);
            }
        }
    }
}