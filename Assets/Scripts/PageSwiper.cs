﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation;
    public float precentThreshold = 0.2f;
    public float easing = 0.5f;

    void Start()
    {
        panelLocation = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float difference = eventData.pressPosition.x - eventData.position.x;
        transform.position = panelLocation - new Vector3(difference, 0, 0);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float percentage = (eventData.pressPosition.x - eventData.position.x) / Screen.width;
        if(Mathf.Abs(percentage) >= precentThreshold)
        {
            Vector3 newLocation = panelLocation;
            if(percentage  > 0)
            {
                newLocation += new Vector3(-Screen.width, 0, 0);
            } else if(percentage < 0)
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

    IEnumerator SmoothMove(Vector3 startPos, Vector3 endPos, float seconds)
    {
        float t = 0f;
        while(t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
