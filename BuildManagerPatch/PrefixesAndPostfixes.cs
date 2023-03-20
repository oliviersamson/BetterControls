using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace BetterControls.BuildManagerPatch
{
    public static class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(BuildManager), "Awake")]
        [HarmonyPostfix]
        public static void AwakePostfix(BuildManager __instance)
        {
            TextMeshProUGUI rotateText = __instance.rotateText.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            rotateText.SetText($"Press "+ NewInputs.Rotate.Value.ToString() + " to rotate build");
        }
    }
}
