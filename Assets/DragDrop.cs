using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    [HideInInspector]
    public bool isInDragMode = false;
    private bool firstTime = true;

    private void Awake()
    {
        canvas = IngameUIManager.Instance.gameObject.GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        //GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = false;

    }

    private void Update()
    {
        //GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Dragged");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isInDragMode)
        {
            if (firstTime)
            {
                Debug.Log("Curr Pos: " + rectTransform.anchoredPosition);
                Debug.Log("new Pos: " + eventData.pointerCurrentRaycast.screenPosition);
                //rectTransform.anchoredPosition = eventData.pointerCurrentRaycast.screenPosition;
                //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
                firstTime = false;
            }
            else
            {
                rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            }
        }
          
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

}
