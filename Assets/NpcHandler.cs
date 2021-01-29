using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHandler : MonoBehaviour
{

    public DialogID[] dialogIDs;
    public string questEndId = "";

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            foreach (var dialogID in dialogIDs)
            {
                if (FindObjectOfType<QuestManager>().CanQuestStart(dialogID))
                {
                    GetComponent<NPCDialogHandler>().TriggerDialog(dialogID);
                }
            }

            if(questEndId != "")
            {
                FindObjectOfType<QuestManager>().ArchiveEndQuest(questEndId);
            }

        }
    }
}
