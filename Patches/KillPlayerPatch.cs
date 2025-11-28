using HarmonyLib;

namespace AlwaysInBounds.Patches
{

    [HarmonyPatch(typeof(KillLocalPlayer))]
    public class KillPlayerPatch
    {
        [HarmonyPatch("KillPlayer")]
        [HarmonyPrefix]
        private static bool KillPlayerMethodPatch(KillLocalPlayer __instance)
        {
            __instance.dontSpawnBody = false;
            return true;
        }
    }
}
