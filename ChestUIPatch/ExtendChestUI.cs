using BetterControls;
using BetterControls.InventoryCellPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine
{
    public static class ExtendChestUI
    {
        public static List<KeyValuePair<int, int>> GetAvailableCells(this ChestUI chestUI, InventoryItem item)
        {
            List<KeyValuePair<int,int>> cellsAndAmounts = new List<KeyValuePair<int,int>>();

            InventoryCell emptyInventoryCell = null;

            int amountToPlace = item.amount;
            foreach (InventoryCell cell in chestUI.cells)
            {
                if (cell.IsItemCompatibleWithCell(item))
                {
                    if (cell.currentItem == null)
                    {
                        if (emptyInventoryCell == null)
                        {
                            emptyInventoryCell = cell;
                        }
                    }
                    else if (cell.currentItem.id == item.id && cell.currentItem.amount != cell.currentItem.max)
                    {
                        int availableSpace = cell.currentItem.max - cell.currentItem.amount;
                        if (availableSpace >= amountToPlace)
                        {
                            cellsAndAmounts.Add(new KeyValuePair<int, int>(cell.cellId, amountToPlace));
                            return cellsAndAmounts;
                        }
                        else
                        {
                            cellsAndAmounts.Add(new KeyValuePair<int, int>(cell.cellId, availableSpace));
                                
                            amountToPlace -= availableSpace;
                        } 
                    }
                }
            }

            if (amountToPlace > 0 && emptyInventoryCell != null)
            {
                Plugin.Log.LogDebug("Hello");
                Plugin.Log.LogDebug(emptyInventoryCell.cellId);
                cellsAndAmounts.Add(new KeyValuePair<int, int>(emptyInventoryCell.cellId, amountToPlace));
            }

            return cellsAndAmounts;
        }

        public static void AddItemToChest(this ChestUI chestUI, InventoryItem item)
        {
            if (item == null)
            {
                return;
            }

            Plugin.Log.LogDebug("Hello there");

            List<KeyValuePair<int, int>> cellsAndAmounts = chestUI.GetAvailableCells(item);

            Plugin.Log.LogDebug("General kenobi");

            foreach (KeyValuePair<int, int> cellAndAmount in cellsAndAmounts)
            {
                item.amount -= cellAndAmount.Value;

                int newAmount = cellAndAmount.Value;
                if (chestUI.cells[cellAndAmount.Key].currentItem != null)
                {
                    newAmount += chestUI.cells[cellAndAmount.Key].currentItem.amount;
                }

                InventoryItem newItem = ScriptableObject.CreateInstance<InventoryItem>();
                newItem.Copy(item, newAmount);

                //ScriptableObject.Destroy(chestUI.cells[cellAndAmount.Key].currentItem);
                chestUI.cells[cellAndAmount.Key].currentItem = newItem;
                chestUI.cells[cellAndAmount.Key].UpdateCell();
                chestUI.cells[cellAndAmount.Key].Invoke("GetReady", (float)(NetStatus.GetPing() * 3) * 0.01f);

                ClientSend.ChestUpdate(OtherInput.Instance.currentChest.id, cellAndAmount.Key, newItem.id, newItem.amount);
            }
        }
    }
}
