using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

[HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
public class MeetingExiledEnd
{
    private static void Postfix(ExileController __instance, [HarmonyArgument(0)] ExileController.InitProperties init)
    {
        if(init != null && init.networkedPlayer != null && init.networkedPlayer.Role is WitchRole) {
            return;
        }
        List<WitchRole> witches = PlayerControl.AllPlayerControls.ToArray()
            .Where((player) => !player.Data.IsDead && player.Data.Role is WitchRole)
            .Select(pc => (WitchRole) pc.Data.Role).ToList();
        
        List<PlayerControl> cursedPlayers = witches.SelectMany(w => w.cursedPlayers).ToList();

        foreach(PlayerControl player in cursedPlayers) {
            player.Die(DeathReason.Exile, true);
        }

        foreach(WitchRole witch in witches) {
            witch.cursedPlayers.Clear();
        }
    }
}