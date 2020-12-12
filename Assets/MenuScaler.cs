using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScaler : MonoBehaviour
{

    public float referenzWidth;
    private CanvasScaler canvasScaler;

    // Start is called before the first frame update
    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.scaleFactor = Screen.width / referenzWidth;
    }
}
