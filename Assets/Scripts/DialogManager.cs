using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public GameObject dialogPanel;
    public Text nameText;
    public Text sentencesText;
    public Image charImage;

    public Sprite playerImage;
    public string playerName;

    private Queue<DialogSentence> dialogSentences;
    private string currentNPCname = "";
    private Sprite currentNPCImage;

    // Start is called before the first frame update
    void Start()
    {
        dialogSentences = new Queue<DialogSentence>();
    }

    public void StartDialog(Dialog dialog)
    {

        dialogSentences.Clear();
        currentNPCname = dialog.name;
        currentNPCImage = dialog.charImage;

        foreach (DialogSentence dialogSentence in dialog.dialogSentences)
        {
            dialogSentences.Enqueue(dialogSentence);
        }

        dialogPanel.SetActive(true);
        Time.timeScale =  0;
        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
        if( dialogSentences.Count == 0)
        {
            EndDialog();
            return;
        }

        DialogSentence dialogSentence = dialogSentences.Dequeue();

        sentencesText.text = dialogSentence.sentence;

        if(dialogSentence.dialogParticipant == DialogParticipant.NPC)
        {
            nameText.text = currentNPCname;
            nameText.alignment = TextAnchor.MiddleRight;
            charImage.sprite = currentNPCImage;
            charImage.rectTransform.pivot = new Vector2(0, 0.5f);

        }
        else{
            nameText.text = playerName;
            nameText.alignment = TextAnchor.MiddleLeft;
            charImage.sprite = playerImage;
            charImage.rectTransform.pivot = new Vector2(1, 0.5f);
        }
    }

    private void EndDialog()
    {
        dialogPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
