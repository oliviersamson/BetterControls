using HarmonyLib;
using UnityEngine;

namespace BetterControls.ChatBoxPatch
{
    public class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(ChatBox), "UserInput")]
        [HarmonyPrefix]
        [HarmonyPriority(Priority.Low)]
        public static bool UpdateMapPrefix(ChatBox __instance)
        {
            if (__instance.typing)
            {
                if (!__instance.inputField.isFocused)
                {
                    __instance.inputField.Select();
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    __instance.SendMessage(__instance.inputField.text);
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    AccessTools.Method(typeof(ChatBox), "ClearMessage").Invoke(__instance, null);
                    __instance.typing = false;
                    __instance.CancelInvoke("HideChat");
                    __instance.Invoke("HideChat", 5f);
                }
            }
            else if (Input.GetKeyDown(NewInputs.Chat.Value))
            {
                AccessTools.Method(typeof(ChatBox), "ShowChat").Invoke(__instance, null);
                __instance.inputField.interactable = true;
                __instance.inputField.Select();
                __instance.typing = true;
            }

            return false;
        }
    }
}
