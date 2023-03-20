using HarmonyLib;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BetterControls.InventoryCellPatch
{
    class PrefixesAndPostfixes
    {
        [HarmonyPatch(typeof(InventoryCell), "OnPointerDown")]
        [HarmonyPrefix]
        public static bool OnPointerDownPrefix(InventoryCell __instance, PointerEventData eventData, ref bool ___ready)
        {
            if (!___ready)
            {
                return false;
            }            

            if (Input.GetKey(KeyCode.LeftShift) && eventData.button == PointerEventData.InputButton.Right 
                && __instance.cellType != InventoryCell.CellType.Crafting)
            {
                ___ready = false;
                __instance.Invoke("GetReady", Time.deltaTime * 2f);

                __instance.ShiftRightClick();

                // Skip original
                return false;
            }
            else if (Input.GetKey(NewInputs.Drop.Value) && __instance.currentItem != null)
            {
                ___ready = false;
                __instance.Invoke("GetReady", Time.deltaTime * 2f);

                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    InventoryItem inventoryItem;
                    InventoryItem inventoryItem2;
                    if (__instance.currentItem.amount > 1)
                    {
                        int num = __instance.currentItem.amount / 2;
                        int num2 = __instance.currentItem.amount - num;
                        inventoryItem = ScriptableObject.CreateInstance<InventoryItem>();
                        inventoryItem.Copy(__instance.currentItem, num);
                        inventoryItem2 = ScriptableObject.CreateInstance<InventoryItem>();
                        inventoryItem2.Copy(__instance.currentItem, num2);
                    }
                    else
                    {
                        inventoryItem = null;
                        inventoryItem2 = __instance.currentItem;
                    }

                    InventoryUI.Instance.currentMouseItem = inventoryItem2;
                    InventoryUI.Instance.DropItem(null);

                    __instance.currentItem = inventoryItem;
                    __instance.UpdateCell();
                }
                else
                {
                    InventoryUI.Instance.currentMouseItem = __instance.currentItem;
                    InventoryUI.Instance.DropItem(null);
                    __instance.RemoveItem();
                }

                if (__instance.cellType == InventoryCell.CellType.Chest)
                {
                    int itemId = -1;
                    int num3 = 0;
                    if (__instance.currentItem)
                    {
                        itemId = __instance.currentItem.id;
                        num3 = __instance.currentItem.amount;
                    }                    
                    ClientSend.ChestUpdate(OtherInput.Instance.currentChest.id, __instance.cellId, itemId, num3);
                }

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