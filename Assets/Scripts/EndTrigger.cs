using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            MenuManager.Instance.OpenMenu(MenuManager.MenuType.Win);
            Destroy(gameObject);
        }
    }
}
