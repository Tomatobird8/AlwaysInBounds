using GameNetcodeStuff;
using HarmonyLib;

namespace AlwaysInBounds.Patches
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    public class KillPlayerPatch
    {
        [HarmonyPatch("KillPlayer")]
        [HarmonyPrefix]
        private static bool KillPlayerMethodPatch(PlayerControllerB __instance, ref bool spawnBody)
        {
            spawnBody = true;
            return true;
        }
    }
}
