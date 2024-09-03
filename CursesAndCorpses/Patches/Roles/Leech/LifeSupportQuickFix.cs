using System.Linq;
using HarmonyLib;
using Rewired;

[HarmonyPatch(typeof(EnterCodeMinigame), nameof(EnterCodeMinigame.AcceptDigits))]
public class LifeSupportQuickFix
{
    [HarmonyPostfix]
    public static void Postfix(EnterCodeMinigame __instance)
    {
        var oxygen = ShipStatus.Instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
        if (oxygen.IsActive)
        {
            PlayerControl leech = PlayerControl.AllPlayerControls.ToArray().Where((player) => player.Data.Role is LeechRole).FirstOrDefault();
            if (leech == null) return;

            LeechRole role = (LeechRole) leech.Data.Role;
            if (role.sabotageQuickFix)
            {
                ShipStatus.Instance.RpcUpdateSystem(SystemTypes.LifeSupp, 16);
            }
        }
    }
}

[HarmonyPatch(typeof(KeypadGame), nameof(KeypadGame.Enter))]
public class LifeSupportQuickFix2
{
    [HarmonyPostfix]
    public static void Postfix(KeypadGame __instance)
    {
        var oxygen = ShipStatus.Instance.Systems[SystemTypes.LifeSupp].Cast<LifeSuppSystemType>();
        if (oxygen.IsActive)
        {
            PlayerControl leech = PlayerControl.AllPlayerControls.ToArray().Where((player) => player.Data.Role is LeechRole).FirstOrDefault();
            if (leech == null) return;

            LeechRole role = (LeechRole) leech.Data.Role;
            if (role.sabotageQuickFix)
            {
                ShipStatus.Instance.RpcUpdateSystem(SystemTypes.LifeSupp, 16);
            }
        }
    }
}