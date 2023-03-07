using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
