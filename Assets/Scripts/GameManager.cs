using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool playerIsDead = false;
    public GameObject menuCanvas;

    public static GameManager Instance = null;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (playerIsDead)
        {
            menuCanvas.transform.Find("LooseScreen").gameObject.SetActive(true);
        }

        if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            menuCanvas.transform.Find("WinScreen").gameObject.SetActive(true);
        }
    }
}
