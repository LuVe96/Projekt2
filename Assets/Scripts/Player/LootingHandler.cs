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
            var item = other.GetComponent<LootObjectHandler>().item;
            Inventory.Instance.Add(item);
            //if(item is QuestItem)
            //{
            //    FindObjectOfType<QuestManager>().ArchiveEndQuest((item as QuestItem).endQuestId);
            //}
            Destroy(other.gameObject);
        }
    }
}
