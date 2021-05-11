using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHandler : MonoBehaviour
{

    public string questEndId = "";

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {

            GetComponent<NPCDialogHandler>().TriggerDialog();


            if(questEndId != "")
            {
                FindObjectOfType<QuestManager>().ArchiveEndQuest(questEndId);
            }

        }
    }
}
