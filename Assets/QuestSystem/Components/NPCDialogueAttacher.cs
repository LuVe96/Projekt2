using QuestSystem.Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueAttacher : MonoBehaviour
{
    List<Dialogue> dialogues = new List<Dialogue>() ;
    PlayerConversant dialogManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        dialogManager = FindObjectOfType<PlayerConversant>();
    }

    public void AddDialogue(Dialogue dialogue)
    {
        dialogues.Add(dialogue);
        foreach (DialogueNode item in dialogue.GetAllNodes())
        {
            Debug.Log("Dia: " + item.Text);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {

        }
    }

}
