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
                c.GetComponent<Button>().interactable = choice.IsInventoyValid();
            }
        }

        private void SetupLayout(bool asChoice)
        {
            choicesPanel.SetActive(asChoice);
            sentencesText.gameObject.SetActive(!asChoice);
            nextButton.gameObject.SetActive(!asChoice);

            if (currentNode.IsPlayerSpeaking)
            {
                nameText.text = playerName;
                nameText.alignment = TextAnchor.MiddleLeft;
                charImage.sprite = playerImage;
                charImage.rectTransform.pivot = new Vector2(1, 0.5f);
            }
            else
            {
                nameText.text = currentNPCname;
                nameText.alignment = TextAnchor.MiddleRight;
                charImage.sprite = currentNPCImage;
                charImage.rectTransform.pivot = new Vector2(0, 0.5f);
            }
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
                DialogueNode[] children = currentDialogue.GetAllValideChildren(currentNode).ToArray();
                if (children.Length > 1)
                {
                    currentNode = children[0];
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
                currentNpc.DialogueHasFinished(currentDialogue.name, currentNode.UniqueID);
                QuestButton.Instance.HideQuestButton();
                currentNpc = null;
            }

        }

        public bool HasNext()
        {
            return currentDialogue.GetAllValideChildren(currentNode).Count() > 0 ;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "NPC")
            {
                try
                {
                    currentNpc = other.GetComponent<NPCDialogueAttacher>();
                    if (currentNpc.Dialogues[0].startInstant)
                    {
                        StartDialogue();
                    } else
                    {
                        QuestButton.Instance.showButtonAsType(QuestButtonType.Talk, StartDialogue);
                    }
                    
                }
                catch (Exception)
                {
                    Debug.LogWarning("NO Dialoge found");
                }

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "NPC")
            {
                QuestButton.Instance.HideQuestButton();
                currentNpc = null;
            }
        }

        private void StartDialogue()
        {
            StartDialogue(currentNpc.Dialogues[0].dialogue, currentNpc.npcName, currentNpc.npcImage);
        }
    } 
}
