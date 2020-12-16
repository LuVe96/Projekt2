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
    public Image useTimeIndicator;

    private bool isLongPressed;
    private bool isClicked;
    public float longPressTime = 1f;
    private float longPressTimeSum = 0;
    [HideInInspector]
    public bool isEquipped { get; private set; } = false;
    private bool isEmpty = false;
    private PointerEventData pointerEnterEventData;
    private Image image;

    private Color stdColor;
    public Color pressedColor;
    public Color disabledColor;


    private void Awake()
    {
        image = GetComponent<Image>();
        stdColor = image.color;
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
        }
        else
        {
            amountText.text = amount.ToString();
        }

        isEmpty = (amount <= 0);
        if(image != null) image.color = (amount <= 0) ? disabledColor : stdColor;
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
        if (isEquipped) return;

        image.color = stdColor;
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
        Debug.LogWarning("short_click");
        if (isEquipped)
        {
            /// Use Item
            item.UseItem();
            if(item is HitEffectItem i)
            {
                //var i = item as HitEffectItem;
                StartCoroutine(ShowTimeIndicator(i.effectItemDuration));
                
            }
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
                image.color = stdColor;
                longPressTimeSum = 0;
                isClicked = false;
                return;
            }
        }

        if (isClicked)
        {
            if (isLongPressed && !isEquipped)
            {
                longPressTimeSum += Time.unscaledDeltaTime;
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
        Debug.Log("opdown: " + (item == null));
        if(item != null && !isEmpty)
        {
            pointerEnterEventData = eventData;
            isLongPressed = true;
            isClicked = true;
            image.color = pressedColor;
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isLongPressed = false;
        pointerEnterEventData = null;
        if(!isEmpty) image.color = stdColor;

    }

   IEnumerator ShowTimeIndicator(float time)
    {
        useTimeIndicator.enabled = true;
        useTimeIndicator.fillAmount = 1;
        float timeSum = 0;
        while (timeSum < time)
        {
            timeSum += Time.deltaTime;
            useTimeIndicator.fillAmount = 1 - timeSum/time;
            yield return null;
        }
        useTimeIndicator.enabled = false;
        image.color = isEmpty ? disabledColor : stdColor;
        yield return null;
    }

}