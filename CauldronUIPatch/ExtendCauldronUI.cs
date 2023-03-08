using BetterControls.InventoryCellPatch;
using System.Collections.Generic;

namespace UnityEngine
{
    public static class ExtendCauldronUI
    {
        public static List<KeyValuePair<int, int>> GetAvailableCells(this CauldronUI cauldronUI, InventoryItem item)
        {
            List<KeyValuePair<int, int>> cellsAndAmounts = new List<KeyValuePair<int, int>>();

            int amountToPlace = item.amount;
            foreach (InventoryCell cell in cauldronUI.synchedCells)
            {
                if (cell.IsItemCompatibleWithCell(item))
                {
                    if (cell.currentItem == null)
                    {
                        cellsAndAmounts.Add(new KeyValuePair<int, int>(cell.cellId, amountToPlace));
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

        public static void AddItemToCauldron(this CauldronUI cauldronUI, InventoryItem item)
        {
            if (item == null)
            {
                return;
            }

            List<KeyValuePair<int, int>> cellsAndAmounts = cauldronUI.GetAvailableCells(item);

            foreach (KeyValuePair<int, int> cellAndAmount in cellsAndAmounts)
            {
                item.amount -= cellAndAmount.Value;

                int newAmount = cellAndAmount.Value;
                if (cauldronUI.synchedCells[cellAndAmount.Key].currentItem != null)
                {
                    newAmount += cauldronUI.synchedCells[cellAndAmount.Key].currentItem.amount;
                }

                InventoryItem newItem = ScriptableObject.CreateInstance<InventoryItem>();
                newItem.Copy(item, newAmount);

                //ScriptableObject.Destroy(chestUI.cells[cellAndAmount.Key].currentItem);
                cauldronUI.synchedCells[cellAndAmount.Key].currentItem = newItem;
                cauldronUI.synchedCells[cellAndAmount.Key].UpdateCell();
                cauldronUI.synchedCells[cellAndAmount.Key].Invoke("GetReady", (float)(NetStatus.GetPing() * 3) * 0.01f);

                ClientSend.ChestUpdate(OtherInput.Instance.currentChest.id, cellAndAmount.Key, newItem.id, newItem.amount);
            }
        }
    }
}
