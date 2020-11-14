using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIndicator : MonoBehaviour
{

    private GameObject indicatorCanvas;
    public Image imagePrefab;
    public Color stdColor;
    public Color focusedColor;

    private Image image = null;

    private void Start()
    {
        indicatorCanvas = GameObject.Find("EnemyIndicatorCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        if (image != null)
        {
            float minX = image.GetPixelAdjustedRect().width / 2;
            float maxX = Screen.width - minX;

            float minY = image.GetPixelAdjustedRect().height / 2;
            float maxY = Screen.height - minY;

            Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);

            if (pos.x < minX || pos.x > maxX || pos.y < minY || pos.y > maxY)
            {
                image.gameObject.SetActive(true);
            }
            else
            {
                image.gameObject.SetActive(false);
            }

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            image.transform.position = pos;
        } 
    }

    public void setIndicator(bool activate)
    {
        if (activate)
        {
            if(image == null)
                image = Instantiate(imagePrefab, indicatorCanvas.transform);
        }
        else
        {
            if (image != null)
            {
                Destroy(image.gameObject);
                image = null;
            }

        }

    }

    public void setFocused(bool focused)
    {
        if(image != null)
        {
            image.color = focused ? focusedColor : focusedColor;
        }
           
    }
}
