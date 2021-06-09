using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem.Quest
{
    //[CreateAssetMenu(fileName = "New QuestDialogueNode", menuName = "QuestSystem/QuestDialogueNode", order = 0)]
    public class QuestDialogueNodeData : MainNodeData
    {
        [SerializeField] Dialogue.Dialogue dialogue;
        [SerializeField] NPCDialogueAttacher nPCDialogueAttacher;

        public QuestDialogueNodeData(string id, NodeHasFinished nodeHasFinished) : base(id, nodeHasFinished)
        {
        }

        public Dialogue.Dialogue Dialogue { get => dialogue; set => dialogue = value; }
        public NPCDialogueAttacher NPCDialogueAttacher { get => nPCDialogueAttacher; set => nPCDialogueAttacher = value; }

        public override void execute()
        {
            Debug.Log("Execute QuestDialogueNode");
            DialogueContainer container = new DialogueContainer(dialogue, DialogueHasFinished);
            nPCDialogueAttacher.AddDialogue(container);
        }

        private void DialogueHasFinished(int nextChildIndex)
        {
            Debug.Log("Dialoge Has finished: " + nextChildIndex);
        }

        private void OnEnable()
        {
        }
    }

    public delegate void DialogueHasFinished(int nextChildIndex);

    public class DialogueContainer
    {
        public Dialogue.Dialogue dialogue;
        public DialogueHasFinished dialogueHasFinished;

        public DialogueContainer(Dialogue.Dialogue dialogue, DialogueHasFinished dialogueHasFinished)
        {
            this.dialogue = dialogue;
            this.dialogueHasFinished = dialogueHasFinished;
        }
    }
}