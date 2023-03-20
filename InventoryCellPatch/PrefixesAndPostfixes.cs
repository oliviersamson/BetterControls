using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BetterControls.InventoryCellPatch
{
    class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(InventoryCell), "OnPointerDown")]
        [HarmonyPrefix]
        public static bool OnPointerDownPrefix(InventoryCell __instance, PointerEventData eventData)
        {
            if (Input.GetKey(KeyCode.LeftShift) && eventData.button == PointerEventData.InputButton.Right 
                && __instance.cellType != InventoryCell.CellType.Crafting)
            {
                __instance.ShiftRightClick();

                // Skip original
                return false;
            }

            // Skip original method
            return true;
        }

        [HarmonyPatch(typeof(InventoryCell), "ShiftClick")]
        [HarmonyPrefix]
        public static bool ShiftClickPrefix(InventoryCell __instance)
        {
            if (__instance.cellType == InventoryCell.CellType.Inventory)
            {
                InventoryItem inventoryItem = null;
                InventoryItem inventoryItem2 = __instance.currentItem;
                switch (OtherInput.Instance.craftingState)
                {
                    case OtherInput.CraftingState.Chest:

                        __instance.currentItem = inventoryItem;

                        ((ChestUI)OtherInput.Instance.chest).AddItemToChest(inventoryItem2);

                        if (__instance.currentItem != null)
                        {
                            __instance.currentItem.amount += inventoryItem2.amount;
                        }
                        else if (inventoryItem2.amount != 0)
                        {
                            __instance.currentItem = inventoryItem2;
                        }

                        __instance.UpdateCell();

                        break;

                    case OtherInput.CraftingState.Cauldron:

                        __instance.currentItem = inventoryItem;

                        ((CauldronUI)OtherInput.Instance.cauldron).AddItemToCauldron(inventoryItem2);

                        if (__instance.currentItem != null)
                        {
                            __instance.currentItem.amount += inventoryItem2.amount;
                        }
                        else if (inventoryItem2.amount != 0)
                        {
                            __instance.currentItem = inventoryItem2;
                        }

                        __instance.UpdateCell();

                        break;

                    case OtherInput.CraftingState.Furnace:

                        __instance.currentItem = inventoryItem;

                        ((FurnaceUI)OtherInput.Instance.furnace).AddItemToFurnace(inventoryItem2);

                        if (__instance.currentItem != null)
                        {
                            __instance.currentItem.amount += inventoryItem2.amount;
                        }
                        else if (inventoryItem2.amount != 0)
                        {
                            __instance.currentItem = inventoryItem2;
                        }

                        __instance.UpdateCell();

                        break;
                }

                // Skip original
                return false;
            }

            // Skip original method
            return true;
        }
    }
}