using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotHandler : MonoBehaviour
{

    LootItem item;
    public Image icon;
    public Text amountText;

    public void AddItem( LootItem newItem, int amount)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        if(amount == 1)
        {
            amountText.text = null;
        }  else
        {
            amountText.text = amount.ToString();
        }

    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        amountText.text = null;
    }
}
