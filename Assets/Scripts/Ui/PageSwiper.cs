﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation;
    public float precentThreshold = 0.2f;
    public float easing = 0.5f;

    [HideInInspector]
    public int currentPage;
    private int pagesCount;
    private Vector3 startLocation;

    public RectTransform[] pages;
    public CanvasScaler canvasScaler;
    private float scaledScreenWidth;

    void Start()
    {
        panelLocation = transform.position;
        startLocation = panelLocation;
        currentPage = 1;
        pagesCount = transform.childCount;

        scaledScreenWidth = Screen.width / canvasScaler.scaleFactor;

        for (int i = 0; i < pages.Length; i++)
        {
            if (i == 0) continue;

            pages[i].offsetMin = new Vector2(scaledScreenWidth * i , 0);
            pages[i].offsetMax = new Vector2(scaledScreenWidth * i, 0);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        float difference = eventData.pressPosition.x - eventData.position.x;
        if ((difference < 0 && currentPage <= 1) || (difference > 0 && currentPage >= pagesCount))
        {
            return;
        }
        transform.position = panelLocation - new Vector3(difference, 0, 0);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float percentage = (eventData.pressPosition.x - eventData.position.x) / scaledScreenWidth;
        if(Mathf.Abs(percentage) >= precentThreshold)
        {
            Vector3 newLocation = panelLocation;
            if(percentage  > 0 && !(currentPage >= pagesCount))
            {
                newLocation += new Vector3(-Screen.width, 0, 0);
            } else if(percentage < 0 && !(currentPage <= 1))
            {
                newLocation += new Vector3(Screen.width, 0, 0);
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        } else
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
       
    }

    void CalcCurrentPage()
    {
        var dif = startLocation.x - transform.position.x;
        currentPage = (int)Mathf.Round(dif / scaledScreenWidth) + 1;

    }

    IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;
        while(t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        CalcCurrentPage();
    }

    public void MoveToPage(int page)
    {
        Vector3 newLocation = new Vector3(startLocation.x - (((float)page - 1) * Screen.width), startLocation.y, startLocation.z);
        StartCoroutine(SmoothMove(transform.position, newLocation, easing));
    }
}
