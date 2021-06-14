using System;
using UnityEngine;

namespace QuestSystem.Quest
{
    public interface IUnsubscribeEvent
    {
        void UnsubscribeEvent();
    }

    [Serializable]
    public class InventoryRequireData : RequirementNodeData, IUnsubscribeEvent
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

        public void UnsubscribeEvent()
        {
            Inventory.OnInventoryChanged -= InventoryHasChanged;
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