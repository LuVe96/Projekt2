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
        [SerializeField] GameObject choicesPanel;
        [SerializeField] GameObject choicesPrefab;

        [SerializeField] Sprite playerImage;
        [SerializeField] string playerName;

        private string currentNPCname = "";
        private Sprite currentNPCImage;

        Dialogue currentDialogue;
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
            SetupLayout(false);

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

        private void ShowChoices(DialogueNode[] choices)
        {
            SetupLayout(true);
            for (int i = 0; i < choicesPanel.transform.childCount; i++)
            {
                Destroy(choicesPanel.transform.GetChild(i).gameObject);
            } 

            foreach (DialogueNode choice in choices)
            {
                GameObject c = Instantiate(choicesPrefab, choicesPanel.transform);
                c.GetComponent<Button>().onClick.AddListener(delegate { OnClickChoice(choice.UniqueID); });
                c.transform.GetChild(0).GetComponent<Text>().text = choice.Text;
            }
        }

        private void SetupLayout(bool asChoice)
        {
            choicesPanel.SetActive(asChoice);
            sentencesText.gameObject.SetActive(!asChoice);
            nextButton.gameObject.SetActive(!asChoice);
        }

        private void OnClickChoice(string uid)
        {
            currentNode = currentDialogue.GetNodeById(uid);
            Next();

        }

        public void Next()
        {
            if (HasNext())
            {
                DialogueNode[] children = currentDialogue.GetAllChildren(currentNode).ToArray();
                if (children.Length > 1)
                {
                    ShowChoices(children);
                } 
                else
                {
                    currentNode = children[0];
                    ShowSentence();
                }

            } else
            {
                dialogPanel.SetActive(false);
                currentNpc.DialogueHasFinished(currentDialogue.name);
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
                    StartDialogue(currentNpc.Dialogues[0].dialogue, currentNpc.npcName, currentNpc.npcImage);
                }
                catch (Exception)
                {
                    Debug.LogWarning("NO Dialoge found");
                }

            }
        }
    } 
}
