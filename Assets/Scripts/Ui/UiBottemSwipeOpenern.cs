using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiBottemSwipeOpenern : MonoBehaviour ,IEndDragHandler, IDragHandler, IBeginDragHandler
{



    public float precentThreshold = 0.2f;
    public float easing = 0.5f;
    public GameObject inventoryItems;

    private float bevoreDargPosY;
    private Vector3 locationOpen;
    private Vector3 locationClosed;

    //[HideInInspector]
    //private Vector3 startLocation;

    public CanvasScaler canvasScaler;
    private float scaledScreenHeight;
    private RectTransform rectTransform;
    private float scalefactor;

    void Start()
    {
        //panelLocation = transform.position;
        //startLocation = panelLocation;
        scalefactor = Screen.height/canvasScaler.referenceResolution.y;

        scaledScreenHeight = Screen.height/* * scalefactor*/ /*/ canvasScaler.scaleFactor*/;
        transform.position = new Vector3(transform.position.x, -Screen.height / 2 + /*Screen.height * scalefactor / 3.5f*/ 800 * scalefactor , transform.position.z);
        locationClosed = transform.position;
        locationOpen = transform.position + new Vector3(0, scaledScreenHeight / 2 /*- Screen.height / 3.5f, 0*/);
        rectTransform = GetComponent<RectTransform>();
        inventoryItems.SetActive(true);

    }

    private void Update()
    {
        if(transform.position == locationClosed)
        {
            Time.timeScale = 1;
        } else
        {
            Time.timeScale = 0;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        bevoreDargPosY = eventData.pressPosition.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //float difference = eventData.pressPosition.x - eventData.position.x;
        //if ((difference < 0 && currentPage <= 1) || (difference > 0 && currentPage >= pagesCount))
        //{
        //    return;
        //}
        //transform.position = panelLocation - new Vector3(difference, 0, 0);
        rectTransform.anchoredPosition += new Vector2(0, eventData.delta.y /*/ canvasScaler.scaleFactor*/);

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        float percentage = (eventData.pressPosition.y - eventData.position.y) / scaledScreenHeight;

        if (eventData.position.y > bevoreDargPosY)
        {
            if (Mathf.Abs(percentage) >= precentThreshold)
            {
                StartCoroutine(SmoothMove(transform.position, locationOpen, easing));
            }
            else
            {
                StartCoroutine(SmoothMove(transform.position, locationClosed, easing));
            }

        }
        else
        {
            if (Mathf.Abs(percentage) >= precentThreshold)
            {
                StartCoroutine(SmoothMove(transform.position, locationClosed, easing));

            }
            else
            {
                StartCoroutine(SmoothMove(transform.position, locationOpen, easing));
            }
        }


    }

    IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.unscaledDeltaTime / seconds;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }


}
