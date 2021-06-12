using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem.Quest
{
    [System.Serializable]
    public class InventoryActionData : ActionNodeData
    {
        InventorySelectionType inventoryAction = InventorySelectionType.Add;
        LootItem lootItem;
        int count;

        public InventoryActionData(string id, InventorySelectionType inventoryAction, LootItem lootItem, int count)
        {
            this.uID = id;
            this.inventoryAction = inventoryAction;
            this.lootItem = lootItem;
            this.count = count;
        }

        public override void executeAction()
        {
            for (int i = 0; i < count; i++)
            {
                if(inventoryAction == InventorySelectionType.Add)
                {
                    Inventory.Instance.Add(lootItem);
                } else
                {
                    Inventory.Instance.Remove(lootItem);
                }
               
            }

        }
    }

    public enum InventorySelectionType
    {
        Add, Remove
    }
}