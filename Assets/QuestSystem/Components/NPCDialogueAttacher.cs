using QuestSystem.Dialogue;
using QuestSystem.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class NPCDialogueAttacher : MonoBehaviour
    {
        List<DialogueContainer> dialogues = new List<DialogueContainer>();
        PlayerConversant dialogManager;

        public Sprite npcImage;
        public string npcName;

        public List<DialogueContainer> Dialogues { get => dialogues; }

        // Start is called before the first frame update
        void Start()
        {
            dialogManager = FindObjectOfType<PlayerConversant>();
        }

        public void AddDialogue(DialogueContainer dialogue)
        {
            Dialogues.Add(dialogue);
            //foreach (DialogueNode item in dialogue.GetAllNodes())
            //{
            //    Debug.Log("Dia: " + item.Text);
            //}
        }

        internal void DialogueHasFinished(string name)
        {
            foreach (DialogueContainer container in dialogues)
            {
                if(container.dialogue.name == name)
                {
                    container.dialogueHasFinished(3);
                }
            }
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if(other.tag == "Player")
        //    {
        //        dialogManager.StartDialogue(dialogues[0], npcName, npcImage);
        //    }
        //}

    }

}