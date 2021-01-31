using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUiHandler : MonoBehaviour
{

    private Inventory inventory;
    public Transform itemsParent;
    private InventorySlotHandler[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.Instance;
        inventory.onItemChangedCallback += UpdateUI;

        UpdateSlots();

    }

    public void UpdateSlots()
    {
        slots = itemsParent.GetComponentsInChildren<InventorySlotHandler>();
        Inventory.Instance.UpdateInventory();

    }

    public void ToggleInventory(bool open)
    {
        //transform.Find("TabbedPager").gameObject.SetActive(open);
        transform.Find("Inventory").gameObject.SetActive(open);
        Time.timeScale = open ? 0 : 1;
    }

    void UpdateUI()
    {
        var inventoryItems = inventory.items.FindAll(i => i.items.Count > 0);
        var emptyItems = inventory.items.FindAll(i => i.items.Count <= 0);

        for (int i = 0; i < slots.Length; i++)
        {

            if (i < inventoryItems.Count)
            {
                var groupedItems = inventoryItems[i];
                slots[i].AddItem(groupedItems.referenzItem, groupedItems.items.Count);
                foreach (var equippedSlot in groupedItems.equiptedSlots)
                {
                    if(equippedSlot != null)
                    {
                        equippedSlot.AddItem(groupedItems.referenzItem, groupedItems.items.Count);
                    }
                }
            }
            else {
                slots[i].ClearSlot();
            }

        }

        foreach (var groupedItems in emptyItems)
        {
            foreach (var equippedSlot in groupedItems.equiptedSlots)
            {
                if (equippedSlot != null)
                {
                    equippedSlot.AddItem(groupedItems.referenzItem, groupedItems.items.Count);
                }
            }
        }
    }
}
