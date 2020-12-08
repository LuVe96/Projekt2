using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    [HideInInspector]
    public bool isInDragMode = false;
    [HideInInspector]
    public static bool DragModeActive { private set; get; } = false;

    private void Awake()
    {
        canvas = IngameUIManager.Instance.gameObject.GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ActivateDragMode()
    {
        DragModeActive = true;
        isInDragMode = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        DragModeActive = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isInDragMode)
        {
            DragModeActive = true;
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        }
          
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        DragModeActive = false;

        var slotHander = GetComponent<InventorySlotHandler>();
        if(!slotHander.isEquipped)
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        canvasGroup.blocksRaycasts = true;
        DragModeActive = false;
    }
}
