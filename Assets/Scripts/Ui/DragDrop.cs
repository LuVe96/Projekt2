﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    [HideInInspector]
    public bool shouldGoInDragMode = false;
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
        shouldGoInDragMode = true;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (shouldGoInDragMode)
        {
            DragModeActive = true;
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            Debug.Log("OnDrag");
        }
          
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (shouldGoInDragMode)
        {
            Debug.Log("OnEndDrag");
            canvasGroup.blocksRaycasts = true;
            DragModeActive = false;
            Time.timeScale = 1;

            var slotHander = GetComponent<InventorySlotHandler>();
            if (!slotHander.isEquipped)
            {
                //FindObjectOfType<InventoryUiHandler>().ToggleInventory(true);
                Destroy(gameObject);
            }
        }
           
    }

    private void OnDisable()
    {
        canvasGroup.blocksRaycasts = true;
        DragModeActive = false;
        Time.timeScale = 1;
    }
}
