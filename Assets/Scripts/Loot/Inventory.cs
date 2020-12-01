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

    public List<LootItem> items = new List<LootItem>();

    public void Add(LootItem item) {
        items.Add(item);
    }

    public void Remove(LootItem item)
    {
        items.Remove(item);
    }
}
