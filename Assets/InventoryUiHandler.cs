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

        slots = itemsParent.GetComponentsInChildren<InventorySlotHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI()
    {
        for (int i = 0; i < inventory.items.Count; i++)
        {
            var groupedItems = inventory.items[i];
            slots[i].AddItem(groupedItems.items[0], groupedItems.items.Count);
        }
    }
}
