using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogHandler : MonoBehaviour
{
    public Dialog dialog;

    public void TriggerDialog()
    {
        FindObjectOfType<DialogManager>().StartDialog(dialog);
    }
}
