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

        private void Awake()
        {
            currentNode = currentDialogue.GetRootNode();
            nextButton.onClick.AddListener(Next);
        }

        public void StartDialogue()
        {
            dialogPanel.SetActive(true);
        }

        public string GetText()
        {
            if (currentDialogue == null) return "";
            return currentNode.Text;
        }

        public void Next()
        {
            DialogueNode[] children = currentDialogue.GetAllChildren(currentNode).ToArray();
            currentNode = children[0];
        }

        public bool HasNext()
        {
            return currentDialogue.GetAllChildren(currentNode).Count() > 0 ;
        }
    } 
}
