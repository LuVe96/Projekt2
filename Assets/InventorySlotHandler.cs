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

    private void Awake()
    {
    }

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

    private void OnLongPress()
    {
        var clonedItem = Instantiate(gameObject, transform.parent);
        clonedItem.transform.SetSiblingIndex(transform.GetSiblingIndex());
        clonedItem.GetComponent<InventorySlotHandler>().item = item;

        transform.parent = GameObject.Find("IngameUICanvas").transform;
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetComponent<RectTransform>().rect.height);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GetComponent<RectTransform>().rect.width);
        GetComponent<DragDrop>().ActivateDragMode();

        GetComponent<InventorySlotHandler>().enabled = false;
        MenuManager.Instance.ToggleInventory(false);
    }

    private void OnShortPress()
    {
        GameObject.Find("DebugText").GetComponent<Text>().text += "  |ShortPress|";

    }

    private void Update()
    {
        if (item == null) return;

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

    ///OnPointer only works with mouse click
    public void OnPointerDown(PointerEventData eventData)
    {
        isLongPressed = true;
        isClicked = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isLongPressed = false;
    }

}
