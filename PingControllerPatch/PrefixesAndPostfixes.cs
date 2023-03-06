using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace BetterControls.PingControllerPatch
{
    public class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(PingController), "Update")]
        [HarmonyPrefix]
        public static bool UpdatePrefix(PingController __instance)
        {
            if (Input.GetKeyDown(NewInputs.Ping.Value))
            {
                AccessTools.Method(typeof(PingController), "LocalPing").Invoke(__instance, new object[] {});
            }

            // Skip original method
            return false;
        }
    }
}
