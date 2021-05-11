using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogHandler : MonoBehaviour
{
    public List<Dialog> dialogs;


    public void TriggerDialog()
    {
        foreach (var dialog in dialogs)
        {

            if (FindObjectOfType<QuestManager>().CanQuestStart(dialog.dialogID))
            {
                FindObjectOfType<DialogManager>().StartDialog(dialog);
                dialogs.Remove(dialog);
                return;
            }
        }

    }
}


