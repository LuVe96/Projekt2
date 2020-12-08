using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlotHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    LootItem item;
    public Image icon;
    public Text amountText;

    private bool isLongPressed;
    private bool isClicked;
    public float longPressTime = 1f;
    private float longPressTimeSum = 0;
    [HideInInspector]
    public bool isEquipped { get; private set; } = false;
    private PointerEventData pointerEnterEventData;


    private void Awake()
    {

    }
    public void setAsEquipped()
    {
        isEquipped = true;
        Inventory.Instance.SetEquippedSlot(item, this);
        
    }

    public void AddItem( LootItem newItem, int amount)
    {
        //if(equippedItemSlot != null){
        //    equippedItemSlot.AddItem(newItem, amount);
        //}

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
        //if (equippedItemSlot != null)
        //{
        //    equippedItemSlot.ClearSlot();
        //    Destroy(equippedItemSlot.gameObject);
        //}
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        amountText.text = null;
    }

    private void OnLongPress()
    {
        Debug.Log("OnLongPress");
        var clonedItem = Instantiate(gameObject, transform.parent);
        clonedItem.transform.SetSiblingIndex(transform.GetSiblingIndex());
        //clonedItem.GetComponent<CanvasGroup>().blocksRaycasts = true;
        //clonedItem.GetComponent<InventorySlotHandler>().equippedItemSlot = this;

        transform.SetParent(GameObject.Find("IngameUICanvas").transform);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetComponent<RectTransform>().rect.height);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GetComponent<RectTransform>().rect.width);
        GetComponent<DragDrop>().ActivateDragMode();

        //GetComponent<InventorySlotHandler>().enabled = false;
        GameObject.Find("MenuCanvas").GetComponent<InventoryUiHandler>().UpdateSlots();

        MenuManager.Instance.ToggleInventory(false);
    }

    private void OnShortPress()
    {
        if (isEquipped)
        {
            Debug.Log("Use  Item");
            /// Use Item
            item.UseItem();
        }else
        {
            Debug.Log("Open Item Menu");
            //Open Iventory Menu
        }
    }

    private void Update()
    {
        if (item == null) return;

        ///Check if Long click is over the slot
        if (pointerEnterEventData != null)
        {
            /// disable dragging while pressing
            if (isLongPressed)
            {
                pointerEnterEventData.dragging = false;
            }
            else
            {
                pointerEnterEventData.dragging = true;
            }

            if (pointerEnterEventData.pointerEnter != gameObject)
            {
                longPressTimeSum = 0;
                isClicked = false;
                return;
            }
        }

        if (isClicked)
        {
            if (isLongPressed)
            {
                longPressTimeSum += Time.deltaTime;
                if (longPressTimeSum >= longPressTime)
                {
                    OnLongPress();
                    longPressTimeSum = 0;
                    isClicked = false;
                    isLongPressed = false;
                }
            }
            else
            {
                OnShortPress();
                longPressTimeSum = 0;
                isClicked = false;
            }
        } else
        {
            longPressTimeSum = 0;
            isClicked = false;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerEnterEventData = eventData;
        isLongPressed = true;
        isClicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isLongPressed = false;
    }

}