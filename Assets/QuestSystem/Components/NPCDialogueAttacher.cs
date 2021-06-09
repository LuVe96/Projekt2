using QuestSystem.Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueAttacher : MonoBehaviour
{
    List<Dialogue> dialogues = new List<Dialogue>() ;
    PlayerConversant dialogManager;

    public Sprite npcImage;
    public string npcName;

    public List<Dialogue> Dialogues { get => dialogues; }

    // Start is called before the first frame update
    void Start()
    {
        dialogManager = FindObjectOfType<PlayerConversant>();
    }

    public void AddDialogue(Dialogue dialogue)
    {
        Dialogues.Add(dialogue);
        //foreach (DialogueNode item in dialogue.GetAllNodes())
        //{
        //    Debug.Log("Dia: " + item.Text);
        //}
    }

    internal void DialogueHasFinished()
    {
        throw new NotImplementedException();
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "Player")
    //    {
    //        dialogManager.StartDialogue(dialogues[0], npcName, npcImage);
    //    }
    //}

}
