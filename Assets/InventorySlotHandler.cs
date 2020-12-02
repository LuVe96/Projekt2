using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotHandler : MonoBehaviour
{

    LootItem item;
    public Image icon;

    private void Start()
    {
        //icon = GetComponentInChildren<Image>();
    }


    public void AddItem( LootItem newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
    }
}
