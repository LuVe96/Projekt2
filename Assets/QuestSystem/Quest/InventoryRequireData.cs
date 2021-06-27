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

        [SerializeField] LootItem lootItem;
        [SerializeField] int count = 1;
        [SerializeField] bool exact = false;

        bool addedAsObserver = false;

        public InventoryRequireData(string id)
        {
            this.uID = id;
        }

        public LootItem LootItem { get => lootItem; set => lootItem = value; }
        public int Count { get => count; set => count = value; }
        public bool Exact { get => exact; set => exact = value; }

        public override bool CheckRequirementNode()
        {
            Debug.Log("Inventory Req exe");
            if (!addedAsObserver)
            {
                Inventory.Instance.OnInventoryChanged += InventoryHasChanged;
                addedAsObserver = true;
            }
            return Inventory.Instance.CheckForItem(LootItem, Count, exact);
        }

        public void UnsubscribeEvent()
        {
            Inventory.Instance.OnInventoryChanged -= InventoryHasChanged;
        }

        private void InventoryHasChanged()
        {
            //if(Inventory.Instance.CheckForItem(LootItem, Count, exact))
            //{
                OnRequirementCheckPassed();
            //}
        }

    }
}