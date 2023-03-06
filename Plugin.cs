using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace BetterControls
{
    public static class Globals
    {
        public const string PLUGIN_GUID = "muck.mrboxxy.bettercontrols";
        public const string PLUGIN_NAME = "BetterControls";
        public const string PLUGIN_VERSION = "0.3.0";
    }

    [BepInPlugin(Globals.PLUGIN_GUID, Globals.PLUGIN_NAME, Globals.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource Log;

        public Harmony harmony;

        private void Awake()
        {
            // Plugin startup logic
            Log = base.Logger;

            harmony = new Harmony(Globals.PLUGIN_NAME);

            // this line is very important, anyone using this as an example shouldn't forget to copy-paste this as well!
            ControlsConfig.Config.SaveOnConfigSet = true;

            harmony.PatchAll(typeof(ControlsConfig));

            harmony.PatchAll(typeof(PingControllerPatch.PrefixesAndPostfixes));
            Log.LogInfo("Patched PingController.Update()");
        }
    }
}
