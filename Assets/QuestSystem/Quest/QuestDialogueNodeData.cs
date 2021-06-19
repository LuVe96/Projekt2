using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem.Quest
{
    //[CreateAssetMenu(fileName = "New QuestDialogueNode", menuName = "QuestSystem/QuestDialogueNode", order = 0)]
    [System.Serializable]
    public class QuestDialogueNodeData : MainNodeData
    {
        [SerializeField] Dialogue.Dialogue dialogue;
        [SerializeField] NPCDialogueAttacher nPCDialogueAttacher;
        DialogueContainer container;
        [SerializeField] List<DialogueEndPointContainer> dialogueEndPointContainer = new List<DialogueEndPointContainer>();

        public QuestDialogueNodeData(string id) : base(id)
        {
        }

        public Dialogue.Dialogue Dialogue { get => dialogue; set => dialogue = value; }
        public NPCDialogueAttacher NPCDialogueAttacher { get => nPCDialogueAttacher; set => nPCDialogueAttacher = value; }
        public List<DialogueEndPointContainer> DialogueEndPointContainer { get => dialogueEndPointContainer; set => dialogueEndPointContainer = value; }

        protected override void executeNode()
        {
            Debug.Log("Execute QuestDialogueNode");
            container = new DialogueContainer(dialogue, DialogueHasFinished);
            nPCDialogueAttacher.AddDialogue(container);
        }

        private void DialogueHasFinished(string endPointId)
        {
            Debug.Log("Dialoge Has finished: ");
            nPCDialogueAttacher.RemoveDialogue(container);

            DialogueEndPointContainer endPointContainer = null;
            foreach (DialogueEndPointContainer endPoint in dialogueEndPointContainer)
            {
                if(endPoint.id == endPointId)
                {
                    endPointContainer = endPoint;
                }
            }
            FinishNode(endPointContainer);

        }

        public void AddDialogueEndPoint(string endPointId,string childId)
        {
            bool endPointExists = false;
            foreach (DialogueEndPointContainer container in dialogueEndPointContainer)
            {
                if(container.id == endPointId)
                {
                    container.endPointChilds.Add(childId);
                    endPointExists = true;
                }
            }
            if (!endPointExists)
            {
                dialogueEndPointContainer.Add(new DialogueEndPointContainer(endPointId, childId));
            }
        }

        public void RemoveDialogueEndPoint(string endPointId, string childId)
        {
            DialogueEndPointContainer toDelete = null;
            foreach (DialogueEndPointContainer container in dialogueEndPointContainer)
            {
                if (container.id == endPointId)
                {
                    if(childId != null)
                    {
                        container.endPointChilds.Remove(childId);
                    } else
                    {
                        container.endPointChilds.Clear();
                    }

                    if(container.endPointChilds.Count <= 0)
                    {
                        toDelete = container;
                    }
                }
            }
            if (toDelete != null)
            {
                dialogueEndPointContainer.Remove(toDelete);
            }
        }

        protected override void resetNode()
        {
            if(container != null)
            {
                nPCDialogueAttacher.RemoveDialogue(container);
            }
        }
    }

    public delegate void DialogueHasFinished(string endPointId);

    [System.Serializable]
    public class DialogueEndPointContainer
    {
        public string id;
        public List<string> endPointChilds = new List<string>();

        public DialogueEndPointContainer(string id, string endPointChild = null)
        {
            this.id = id;
            if(endPointChilds!= null)
                this.endPointChilds.Add(endPointChild);
        }
    }

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