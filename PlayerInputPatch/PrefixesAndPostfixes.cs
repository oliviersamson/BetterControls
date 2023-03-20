using HarmonyLib;
using UnityEngine;

namespace BetterControls.PlayerInputPatch
{
    public static class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(PlayerInput), "MyInput")]
        [HarmonyPrefix]
        public static void MyInputPrefix()
        {
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
        }
    }
}
