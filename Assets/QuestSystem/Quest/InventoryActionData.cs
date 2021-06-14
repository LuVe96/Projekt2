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
        int count = 1;

        public InventoryActionData(string id)
        {
            this.uID = id;
        }

        public InventorySelectionType InventoryAction { get => inventoryAction; set => inventoryAction = value; }
        public LootItem LootItem { get => lootItem; set => lootItem = value; }
        public int Count { get => count; set => count = value; }

        public override void executeAction()
        {
            for (int i = 0; i < Count; i++)
            {
                if(InventoryAction == InventorySelectionType.Add)
                {
                    Inventory.Instance.Add(LootItem);
                } else
                {
                    Inventory.Instance.Remove(LootItem);
                }
               
            }

        }
    }

    public enum InventorySelectionType
    {
        Add, Remove
    }
}