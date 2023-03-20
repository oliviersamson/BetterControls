using System.Collections.Generic;

namespace UnityEngine
{
    public static class ExtendFurnaceUI
    {
        public static List<KeyValuePair<int, int>> GetAvailableCells(this FurnaceUI furnaceUI, InventoryItem item)
        {
            List<KeyValuePair<int, int>> cellsAndAmounts = new List<KeyValuePair<int, int>>();

            InventoryCell emptyInventoryCell = null;

            int amountToPlace = item.amount;
            foreach (InventoryCell cell in furnaceUI.synchedCells)
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
                cellsAndAmounts.Add(new KeyValuePair<int, int>(emptyInventoryCell.cellId, amountToPlace));
            }

            return cellsAndAmounts;
        }

        public static void AddItemToFurnace(this FurnaceUI furnaceUI, InventoryItem item)
        {
            if (item == null)
            {
                return;
            }

            List<KeyValuePair<int, int>> cellsAndAmounts = furnaceUI.GetAvailableCells(item);

            foreach (KeyValuePair<int, int> cellAndAmount in cellsAndAmounts)
            {
                item.amount -= cellAndAmount.Value;

                int newAmount = cellAndAmount.Value;
                if (furnaceUI.synchedCells[cellAndAmount.Key].currentItem != null)
                {
                    newAmount += furnaceUI.synchedCells[cellAndAmount.Key].currentItem.amount;
                }

                InventoryItem newItem = ScriptableObject.CreateInstance<InventoryItem>();
                newItem.Copy(item, newAmount);

                //ScriptableObject.Destroy(chestUI.cells[cellAndAmount.Key].currentItem);
                furnaceUI.synchedCells[cellAndAmount.Key].currentItem = newItem;
                furnaceUI.synchedCells[cellAndAmount.Key].UpdateCell();
                furnaceUI.synchedCells[cellAndAmount.Key].Invoke("GetReady", (float)(NetStatus.GetPing() * 3) * 0.01f);

                ClientSend.ChestUpdate(OtherInput.Instance.currentChest.id, cellAndAmount.Key, newItem.id, newItem.amount);
            }
        }
    }
}
