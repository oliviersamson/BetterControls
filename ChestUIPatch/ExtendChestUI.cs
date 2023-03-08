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

            int amountToPlace = item.amount;
            foreach (InventoryCell cell in chestUI.cells)
            {
                if (cell.IsItemCompatibleWithCell(item))
                {
                    if (cell.currentItem == null)
                    {
                        cellsAndAmounts.Add(new KeyValuePair<int, int>(cell.cellId, item.amount));
                        return cellsAndAmounts;
                    }
                    else
                    {
                        if (cell.currentItem.id == item.id && cell.currentItem.amount != cell.currentItem.max)
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
            }

            return cellsAndAmounts;
        }

        public static void AddItemToChest(this ChestUI chestUI, InventoryItem item)
        {
            if (item == null)
            {
                return;
            }

            List<KeyValuePair<int, int>> cellsAndAmounts = chestUI.GetAvailableCells(item);

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
