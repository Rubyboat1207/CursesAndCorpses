using HarmonyLib;
using UnityEngine;


[HarmonyPatch(typeof(ExileController), nameof(ExileController.Begin))]
public class VotedOutPatch {
    private static void Postfix(ExileController __instance, [HarmonyArgument(0)] ExileController.InitProperties init)
    {
        if(init != null && init.networkedPlayer != null) {
            if(init.networkedPlayer.Role is JesterRole jester) {
                jester.votedOut = true;

                // JesterRole.RpcVotedOut(init.networkedPlayer);
                GameManager.Instance.RpcEndGame((GameOverReason) 9, false);
            }
        }
    }
}