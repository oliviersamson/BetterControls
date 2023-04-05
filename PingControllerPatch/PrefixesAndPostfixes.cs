using HarmonyLib;
using UnityEngine;

namespace BetterControls.PingControllerPatch
{
    public class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(PingController), "Update")]
        [HarmonyPrefix]
        public static bool UpdatePrefix(PingController __instance)
        {
            if (!OtherInput.Instance.OtherUiActive() && Input.GetKeyDown(NewInputs.Ping.Value))
            {
                AccessTools.Method(typeof(PingController), "LocalPing").Invoke(__instance, null);
            }

            // Skip original method
            return false;
        }
    }
}
