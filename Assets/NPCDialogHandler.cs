using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogHandler : MonoBehaviour
{
    public Dialog[] dialogs;

    public void TriggerDialog(DialogID id)
    {
        foreach (var dialog in dialogs)
        {
            if(dialog.dialogID == id)
            {
                FindObjectOfType<DialogManager>().StartDialog(dialog);
                return;
            }
        }

    }
}


