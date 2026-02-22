using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace AlwaysInBounds.Patches
{
    [HarmonyPatch(typeof(OutOfBoundsTrigger))]
    public class OutOfBoundsTriggerPatch
    {
        [HarmonyPatch("OnTriggerEnter")]
        [HarmonyPrefix]
        private static bool OnTriggerEnterPatch(OutOfBoundsTrigger __instance, ref Collider other)
        {
            if (other.tag != "Player" || (__instance.disableWhenRoundStarts && !__instance.playersManager.inShipPhase))
            {
                return true;
            }
            PlayerControllerB playerController = other.GetComponent<PlayerControllerB>();
            if (playerController == null || GameNetworkManager.Instance.localPlayerController != playerController || !__instance.playersManager.shipDoorsEnabled)
            {
                return true;
            }
            if (!StartOfRound.Instance.isChallengeFile)
            {
                playerController.ResetFallGravity();
                switch (AlwaysInBounds.tpLocationType)
                {
                    case AlwaysInBounds.LocationType.Ship:
                        playerController.TeleportPlayer(__instance.playersManager.outsideShipSpawnPosition.position, false, 0f, false, true);
                        break;
                    case AlwaysInBounds.LocationType.Entrance:
                        playerController.TeleportPlayer(Object.FindObjectOfType<EntranceTeleport>().entrancePoint.position, false, 0f, false, true);
                        break;
                    case AlwaysInBounds.LocationType.Dynamic:
                        if (playerController.isInsideFactory)
                        {
                            playerController.TeleportPlayer(Object.FindObjectOfType<EntranceTeleport>().entrancePoint.position, false, 0f, false, true);
                        }
                        else
                        {
                            playerController.TeleportPlayer(__instance.playersManager.outsideShipSpawnPosition.position, false, 0f, false, true);
                        }
                        break;
                    default:
                        return true;
                }
                playerController.isInsideFactory = false;
                return false;
            }
            return true;
        }
    }
}
