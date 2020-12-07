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
        //var clonedItem = Instantiate(gameObject, GameObject.Find("IngameUICanvas").transform);
        //clonedItem.transform.position = transform.position;
        //clonedItem.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetComponent<RectTransform>().rect.height);
        //clonedItem.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GetComponent<RectTransform>().rect.width);
        //clonedItem.GetComponent<DragDrop>().enabled = true;
        //clonedItem.GetComponent<InventorySlotHandler>().enabled = false;
        //MenuManager.Instance.ToggleInventory(false);

        var clonedItem = Instantiate(gameObject, transform.parent);
        clonedItem.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.parent = GameObject.Find("IngameUICanvas").transform;
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetComponent<RectTransform>().rect.height);
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GetComponent<RectTransform>().rect.width);
        GetComponent<DragDrop>().enabled = true;
        GetComponent<DragDrop>().isInDragMode = true;
        GetComponent<InventorySlotHandler>().enabled = false;
        MenuManager.Instance.ToggleInventory(false);
    }

    private void OnShortPress()
    {

    }

    private void Update()
    {
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
        }

    }

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
