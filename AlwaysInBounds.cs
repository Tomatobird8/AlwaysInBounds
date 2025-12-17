using AlwaysInBounds.Patches;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;

namespace AlwaysInBounds
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class AlwaysInBounds : BaseUnityPlugin
    {
        public static AlwaysInBounds Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger { get; private set; } = null!;
        internal static Harmony? Harmony { get; set; }

        public static LocationType tpLocationType;
        public static bool spawnbody;

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            string value = Config.Bind<string>("General", "TpLocationConfig", "Dynamic", new ConfigDescription("Where to teleport player when player falls out of bounds", new AcceptableValueList<string>(new string[3] {"Ship", "Entrance", "Dynamic" }), Array.Empty<object>())).Value;
            spawnbody = Config.Bind<bool>("General", "SpawnPlayerBody", true, "Should KillPlayer method be patched to always spawn body?").Value;

            tpLocationType = (value == "Ship") ? LocationType.Ship : (value == "Entrance") ? LocationType.Entrance : LocationType.Dynamic;

            Patch();

            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
        }

        internal static void Patch()
        {
            Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

            Logger.LogDebug("Patching...");

            Harmony.PatchAll(typeof(OutOfBoundsTriggerPatch));

            if (spawnbody)
            {
                Harmony.PatchAll(typeof(KillPlayerPatch));
            }

            Logger.LogDebug("Finished patching!");
        }

        public enum LocationType
        {
            Ship,
            Entrance,
            Dynamic
        }
    }
}
