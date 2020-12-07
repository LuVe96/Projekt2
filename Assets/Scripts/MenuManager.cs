using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);  // the Singelton Obj gets not deleted when change szene
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ToggleInventory(bool open)
    {
        transform.Find("TabbedPager").gameObject.SetActive(open);
    }
}
