using HarmonyLib;
using UnityEngine;

namespace BetterControls.HotbarPatch
{
    class PrefixesAndPostfixes
    {

        [HarmonyPatch(typeof(Hotbar), "Update")]
        [HarmonyPrefix]
        public static bool UpdatePrefix(ref Hotbar __instance, ref int ___currentActive, ref int __state)
        {
            __state = ___currentActive;

            if (Input.GetKeyDown(NewInputs.Hotbar.LastSelected.Value))
            {
                ___currentActive = __instance.GetLastActive().Value;
                __instance.UpdateHotbar();

                // Skip original
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(Hotbar), "Update")]
        [HarmonyPostfix]
        public static void UpdatePostfix(ref Hotbar __instance, ref int ___currentActive, ref int __state)
        {
            if (__state != ___currentActive)
            {
                __instance.SetLastActive(__state);
            }
        }
    }
}
