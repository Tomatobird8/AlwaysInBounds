using HarmonyLib;

namespace AlwaysInBounds.Patches
{
    [HarmonyPatch(typeof(DeadBodyInfo))]
    public class DeadBodyPatch
    {
        [HarmonyPatch("DetectIfSeenByLocalPlayer")]
        [HarmonyPrefix]
        public static bool BodyDetectPatch(DeadBodyInfo __instance)
        {
            if (__instance.isInShip)
            {
                return false;
            }
            return true;
        }
    }
}
