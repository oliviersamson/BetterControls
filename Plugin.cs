using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace BetterControls
{
    public static class Globals
    {
        public const string PLUGIN_GUID = "muck.mrboxxy.bettermultiplayer";
        public const string PLUGIN_NAME = "BetterMultiplayer";
        public const string PLUGIN_VERSION = "0.3.0";
    }

    [BepInPlugin(Globals.PLUGIN_GUID, Globals.PLUGIN_NAME, Globals.PLUGIN_VERSION)]
    [BepInProcess("Muck.exe")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;

        public Harmony harmony;

        private void Awake()
        {
            // Plugin startup logic
            Log = base.Logger;

            harmony = new Harmony(Globals.PLUGIN_NAME);
        }
    }
}
