
namespace UnityEngine
{
    public static class ExtendInventoryCell
    {
        public static bool IsItemCompatibleWithCell(this InventoryCell inventoryCell, InventoryItem item)
        {
            if (inventoryCell.tags.Length == 0)
            {
                return true;
            }
            foreach (InventoryItem.ItemTag itemTag in inventoryCell.tags)
            {
                if (item.tag == itemTag)
                {
                    return true;
                }
            }
            return false;
        }

        public static void ShiftRightClick(this InventoryCell inventoryCell)
        {
            if (inventoryCell.currentItem == null) 
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

            switch (inventoryCell.cellType)
            {
                case InventoryCell.CellType.Chest:

                    if (!InventoryUI.Instance.CanPickup(inventoryCell.currentItem))
                    {
                        return;
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
                    AchievementManager.Instance.PickupItem(inventoryCell.currentItem);

                    break;

                case InventoryCell.CellType.Inventory:

                    switch (OtherInput.Instance.craftingState)
                    {
                        case OtherInput.CraftingState.Chest:

                            inventoryCell.currentItem = inventoryItem;

                            ((ChestUI)OtherInput.Instance.chest).AddItemToChest(inventoryItem2);

                            if (inventoryCell.currentItem != null)
                            {
                                inventoryCell.currentItem.amount += inventoryItem2.amount;
                            }
                            else if (inventoryItem2.amount != 0)
                            {
                                inventoryCell.currentItem = inventoryItem2;
                            }

                            inventoryCell.UpdateCell();

                            break;

                        case OtherInput.CraftingState.Cauldron:

                            inventoryCell.currentItem = inventoryItem;

                            ((CauldronUI)OtherInput.Instance.cauldron).AddItemToCauldron(inventoryItem2);

                            if (inventoryCell.currentItem != null)
                            {
                                inventoryCell.currentItem.amount += inventoryItem2.amount;
                            }
                            else if (inventoryItem2.amount != 0)
                            {
                                inventoryCell.currentItem = inventoryItem2;
                            }

                            inventoryCell.UpdateCell();

                            break;

                        case OtherInput.CraftingState.Furnace:

                            inventoryCell.currentItem = inventoryItem;

                            ((FurnaceUI)OtherInput.Instance.furnace).AddItemToFurnace(inventoryItem2);

                            if (inventoryCell.currentItem != null)
                            {
                                inventoryCell.currentItem.amount += inventoryItem2.amount;
                            }
                            else if (inventoryItem2.amount != 0)
                            {
                                inventoryCell.currentItem = inventoryItem2;
                            }

                            inventoryCell.UpdateCell();

                            break;
                    }

                    break;

                default:
                    break;
            }
        }
    }
}
