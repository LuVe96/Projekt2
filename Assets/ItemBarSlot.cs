﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemBarSlot : MonoBehaviour, IDropHandler
{

    private void Update()
    {
        if (DragDrop.DragModeActive)
        {
            GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
        }
        else
        {
            GetComponent<Image>().color = new Color(255, 255, 255, 0);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            /// set position of item
            var rect = eventData.pointerDrag.GetComponent<RectTransform>();
            eventData.pointerDrag.transform.parent = transform;
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(0, 0);
            var dragDrop = eventData.pointerDrag.GetComponent<DragDrop>();
            
            dragDrop.enabled = false;

        }
    }
}
