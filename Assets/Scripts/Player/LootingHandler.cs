using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootingHandler : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Loot")
        {
            Inventory.Instance.Add(other.gameObject.GetComponent<LootObjectHandler>().item);
            Destroy(other.gameObject);
        }
    }
}
