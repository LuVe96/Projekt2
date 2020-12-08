using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {

            if (i < inventory.items.Count)
            {
                var groupedItems = inventory.items[i];
                slots[i].AddItem(groupedItems.items[0], groupedItems.items.Count);
                foreach (var equippedSlot in groupedItems.equiptedSlots)
                {
                    if(equippedSlot != null)
                    {
                        equippedSlot.AddItem(groupedItems.items[0], groupedItems.items.Count);
                    }
                }
            }
            else {
                slots[i].ClearSlot();
            }

        }
    }
}
