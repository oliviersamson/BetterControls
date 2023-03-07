using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;
using UnityEngine;

namespace BetterControls.InventoryCellPatch
{
    public static class ExtendInventoryCell
    {
        public static void ShiftRightClick(this InventoryCell inventoryCell)
        {
            switch (inventoryCell.cellType)
            {
                case InventoryCell.CellType.Chest:
                    if (!InventoryUI.Instance.CanPickup(inventoryCell.currentItem))
                    {
                        return;
                    }

                    InventoryItem inventoryItem;
                    InventoryItem inventoryItem2;
                    if (inventoryCell.currentItem.amount > 1)
                    {
                        int num = inventoryCell.currentItem.amount / 2;
                        int num2 = inventoryCell.currentItem.amount - num;
                        inventoryItem = ScriptableObject.CreateInstance<InventoryItem>();
                        inventoryItem.Copy(inventoryCell.currentItem, num);
                        inventoryItem2 = ScriptableObject.CreateInstance<InventoryItem>();
                        inventoryItem2.Copy(inventoryCell.currentItem, num2);
                    }
                    else
                    {
                        inventoryItem = null;
                        inventoryItem2 = inventoryCell.currentItem;
                    }

                    inventoryCell.currentItem = inventoryItem;
                    inventoryCell.UpdateCell();
                    InventoryUI.Instance.AddItemToInventory(inventoryItem2);

                    int itemId = -1;
                    int num3 = 0;
                    if (inventoryCell.currentItem)
                    {
                        itemId = inventoryCell.currentItem.id;
                        num3 = inventoryCell.currentItem.amount;
                    }
                    float time = 1f;
                    inventoryCell.Invoke("GetReady", time);
                    ClientSend.ChestUpdate(OtherInput.Instance.currentChest.id, inventoryCell.cellId, itemId, num3);

                    break;

                default:
                    break;
            }
        }
    }
}
