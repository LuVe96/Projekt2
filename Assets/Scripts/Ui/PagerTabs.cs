using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PagerTabs : MonoBehaviour
{

    private List<GameObject> buttons = new List<GameObject>();
    private int? currentPage = null;
    private PageSwiper pageSwiper;
    public bool noPager;

    void Start()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            var btn = transform.GetChild(i).gameObject;
            if (btn.name != "CloseButton")
            {
                buttons.Add(transform.GetChild(i).gameObject);
            }
        }
        if (noPager) return;
        pageSwiper = transform.parent.Find("Pager").GetComponent<PageSwiper>();
    }


    void Update()
    {
        if (noPager) return;

        int page = pageSwiper.currentPage;

        if(page != currentPage || currentPage == null)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].GetComponentInChildren<Text>().fontStyle = FontStyle.Normal;

                if ((i + 1) == page)
                {
                    buttons[i].GetComponentInChildren<Text>().fontStyle = FontStyle.BoldAndItalic;
                }
            }
            currentPage = page;
        }     
    }

    public void ButtonClicked(int num)
    {
        pageSwiper.MoveToPage(num);
    }
}
