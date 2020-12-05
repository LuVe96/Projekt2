using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootingHandler : MonoBehaviour
{

    private bool looting = false;

    private void Update()
    {
        looting = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        /// 'looting' -> if multible on entertriger
        if (other.tag == "Loot" && !looting)
        {
            looting = true;
            Inventory.Instance.Add(other.gameObject.GetComponent<LootObjectHandler>().item);
            Destroy(other.gameObject);
        }
    }
}
