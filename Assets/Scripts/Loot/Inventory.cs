using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static Inventory Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);  // the Singelton Obj gets not deleted when change szene
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public List<GroupedItems> items = new List<GroupedItems>();

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public void Add(LootItem item) {

        bool itemAdded = false;
        foreach (var it in items)
        {
            if(item.id == it.id)
            {
                it.items.Add(item);
                itemAdded = true;
                break;
            }
        }

        /// create new grouped Items
        if (!itemAdded)
        {
            items.Add(new GroupedItems(item));
        }
        
        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void Remove(LootItem item)
    {
        foreach (var it in items)
        {
            if (item.id == it.id)
            {
                it.items.Remove(item);
                //if(it.items.Count <= 0)
                //{
                //    foreach (var equippedSlot in it.equiptedSlots)
                //    {
                //        if (equippedSlot != null)
                //        {
                //            Destroy(equippedSlot.gameObject);
                //        } 

                //    }
                //    ////items.Remove(it);
                //}
                it.equiptedSlots.RemoveAll(i => i == null);
                break;
            }
        }

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void UpdateInventory()
    {
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void SetEquippedSlot(LootItem item, InventorySlotHandler equippedSlot)
    {
        foreach (var groupedItem in items)
        {
            if(groupedItem.id == item.id)
            {
                groupedItem.equiptedSlots.Add(equippedSlot);
            }
        }
    }
}

public struct GroupedItems
{
    public string id;
    public List<LootItem> items;
    public LootItem referenzItem;
    public List<InventorySlotHandler> equiptedSlots;

    public GroupedItems(LootItem item)
    {
        this.id = item.id;
        equiptedSlots = new List<InventorySlotHandler>();
        items = new List<LootItem>();
        items.Add(item);
        referenzItem = item;
    }


}