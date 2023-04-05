using HarmonyLib;
using UnityEngine;

namespace BetterControls.PlayerInputPatch
{
    public static class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(PlayerInput), "MyInput")]
        [HarmonyPrefix]
        public static bool MyInputPrefix(PlayerInput __instance)
        {
            if (OtherInput.Instance.OtherUiActive() && !Map.Instance.active)
            {
                AccessTools.Method(typeof(PlayerInput), "StopInput").Invoke(__instance, null);

                // Skip original method
                return false;
            }

            if (Input.GetKeyDown(NewInputs.Drop.Value))
            {
                if (InventoryUI.Instance.gameObject.activeInHierarchy)
                {
                    if (InventoryUI.Instance.currentMouseItem != null)
                    {
                        InventoryUI.Instance.DropItem(null);
                    }
                }
                else if (Hotbar.Instance.currentItem != null)
                {
                    InventoryUI.Instance.currentMouseItem = Hotbar.Instance.currentItem;
                    InventoryUI.Instance.DropItem(null);
                    Hotbar.Instance.UseItem(Hotbar.Instance.currentItem.amount);
                }
            }

            return true;
        }
    }
}
