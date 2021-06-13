using System;
using UnityEngine;

namespace QuestSystem.Quest
{
    [Serializable]
    public class InventoryRequireData : RequirementNodeData
    {

        LootItem lootItem;
        int count = 0;

        bool addedAsObserver = false;

        public InventoryRequireData(string id, LootItem lootItem, int count)
        {
            this.uID = id;
            this.lootItem = lootItem;
            this.count = count;
        }

        public override bool CheckRequirementNode()
        {
            Debug.Log("Inventory Req exe");
            if (!addedAsObserver)
            {
                Inventory.OnInventoryChanged += InventoryHasChanged;
                addedAsObserver = true;
            }
            return Inventory.Instance.CheckForItem(lootItem, count);
        }

        private void InventoryHasChanged()
        {
            if(Inventory.Instance.CheckForItem(lootItem, count))
            {
                OnRequirementCheckPassed();
            }
        }

    }
}