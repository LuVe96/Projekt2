using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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


    public GameObject menus;
    public Text text;
    public Button button_1;
    public Button buttonMainMenu;

    private MenuType currentMenuType;

    private void Start()
    {
        button_1.onClick.AddListener(ButtenOneClicked);
        buttonMainMenu.onClick.AddListener(ButtonMainMenuClicked);
    }

    public void OpenPauseMenu()
    {
        OpenMenu(MenuType.Pause);
    }

    public void OpenMenu(MenuType menuType)
    {
        menus.SetActive(true);
        currentMenuType = menuType;
        Time.timeScale = 0;

        switch (menuType)
        {
            case MenuType.Pause:
                text.text = "Pause";
                button_1.GetComponentInChildren<Text>().text = "Weiterspielen";
                break;
            case MenuType.Loose:
                text.text = "Du bist gestorben.";
                button_1.GetComponentInChildren<Text>().text = "Neustarten";
                break;
            case MenuType.Win:
                text.text = "Du hast das Level erfolgreich durchgespielt";
                button_1.GetComponentInChildren<Text>().text = "Weiterspielen";
                break;
            default:
                break;
        }

    }

    private void ButtenOneClicked()
    {
        if(currentMenuType == MenuType.Loose)
        {
            SceneManager.LoadScene(1); // Restart
        } else
        {
            Time.timeScale = 1;
            menus.SetActive(false);
        }

    }

    private void ButtonMainMenuClicked()
    {
        SceneManager.LoadScene(0);
    }

    //public void RestartGame()
    //{
    //    SceneManager.LoadScene(0);
    //}


    public enum MenuType
    {
        Pause, Loose, Win
    }

}
