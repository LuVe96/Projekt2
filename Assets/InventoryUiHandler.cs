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
        Debug.Log("Start Inventory");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI()
    {
        for (int i = 0; i < inventory.items.Count; i++)
        {
            slots[i].AddItem(inventory.items[i]);
        }
    }
}
