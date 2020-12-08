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
            if(item.name == it.name)
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
            if (item.name == it.name)
            {
                it.items.Remove(item);
                if(it.items.Count <= 0)
                {
                    items.Remove(it);
                }
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
}

public struct GroupedItems
{
    public string name;
    public List<LootItem> items;

    public GroupedItems(LootItem item)
    {
        this.name = item.name;
        items = new List<LootItem>();
        items.Add(item);
    }


}