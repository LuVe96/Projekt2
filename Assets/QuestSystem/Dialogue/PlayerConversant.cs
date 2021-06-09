using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {

        [SerializeField] GameObject dialogPanel;
        [SerializeField] Text nameText;
        [SerializeField] Text sentencesText;
        [SerializeField] Image charImage;
        [SerializeField] Button nextButton;

        [SerializeField] Sprite playerImage;
        [SerializeField] string playerName;

        private Queue<DialogSentence> dialogSentences;
        private string currentNPCname = "";
        private Sprite currentNPCImage;


        [SerializeField] Dialogue currentDialogue;
        DialogueNode currentNode = null;
        NPCDialogueAttacher currentNpc = null;

        private void Start()
        {
            nextButton.onClick.AddListener(Next);
        }

        public void StartDialogue(Dialogue dialogue, string npcName, Sprite npcImage)
        {
            currentNPCname = npcName;
            currentNPCImage = npcImage;
            currentDialogue = dialogue;
            currentNode = currentDialogue.GetRootNode();
            dialogPanel.SetActive(true);
            ShowSentence();
        }

        private void ShowSentence()
        {
            if (currentNode.IsPlayerSpeaking)
            {
                nameText.text = playerName;
                charImage.sprite = playerImage;
            }
            else
            {
                nameText.text = currentNPCname;
                charImage.sprite = currentNPCImage;
            }

            sentencesText.text = currentNode.Text;
        }

        public void Next()
        {
            if (HasNext())
            {
                DialogueNode[] children = currentDialogue.GetAllChildren(currentNode).ToArray();
                currentNode = children[0];
                ShowSentence();
            } else
            {
                dialogPanel.SetActive(false);
                currentNpc.DialogueHasFinished();
                currentNpc = null;
            }

        }

        public bool HasNext()
        {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0 ;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "NPC")
            {
                try
                {
                    currentNpc = other.GetComponent<NPCDialogueAttacher>();
                    StartDialogue(currentNpc.Dialogues[0], currentNpc.npcName, currentNpc.npcImage);
                }
                catch (Exception)
                {
                    Debug.LogWarning("NO Dialoge found");
                }

            }
        }
    } 
}
