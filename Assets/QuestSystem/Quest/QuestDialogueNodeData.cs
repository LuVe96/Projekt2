using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem.Quest
{
    //[CreateAssetMenu(fileName = "New QuestDialogueNode", menuName = "QuestSystem/QuestDialogueNode", order = 0)]
    [System.Serializable]
    public class QuestDialogueNodeData : EndpointMainNodeData
    {
        [SerializeField] Dialogue.Dialogue dialogue;
        [SerializeField] NPCDialogueAttacher nPCDialogueAttacher;
        [SerializeField] bool startInstant = false;
        DialogueContainer container;
       

        public QuestDialogueNodeData(string id) : base(id)
        {
        }

        public Dialogue.Dialogue Dialogue { get => dialogue; set => dialogue = value; }
        public NPCDialogueAttacher NPCDialogueAttacher { get => nPCDialogueAttacher; set => nPCDialogueAttacher = value; }
        public bool StartInstant { get => startInstant; set => startInstant = value; }

        protected override void executeNode()
        {
            Debug.Log("Execute QuestDialogueNode");
            container = new DialogueContainer(dialogue, DialogueHasFinished, startInstant);
            nPCDialogueAttacher.AddDialogue(container);
        }

        private void DialogueHasFinished(string endPointId)
        {
            Debug.Log("Dialoge Has finished: ");
            nPCDialogueAttacher.RemoveDialogue(container);

            EndPointContainer endPointContainer = null;
            foreach (EndPointContainer endPoint in EndPointContainer)
            {
                if(endPoint.id == endPointId)
                {
                    endPointContainer = endPoint;
                }
            }
            FinishNode(endPointContainer);

        }

        public override void AddChildToEndPoint(string endPointId, string childId)
        {
            bool endPointExists = false;
            foreach (EndPointContainer container in EndPointContainer)
            {
                if (container.id == endPointId)
                {
                    container.endPointChilds.Add(childId);
                    endPointExists = true;
                }
            }
            if (!endPointExists)
            {
                EndPointContainer.Add(new EndPointContainer(endPointId, childId));
            }
        }

        public override void RemoveChildToFromPoint(string endPointId, string childId)
        {
            EndPointContainer toDelete = null;
            foreach (EndPointContainer container in EndPointContainer)
            {
                if (container.id == endPointId)
                {
                    if (childId != null)
                    {
                        container.endPointChilds.Remove(childId);
                    }
                    else
                    {
                        container.endPointChilds.Clear();
                    }

                    if (container.endPointChilds.Count <= 0)
                    {
                        toDelete = container;
                    }
                }
            }
            if (toDelete != null)
            {
                EndPointContainer.Remove(toDelete);
            }
        }

        public override void resetNode()
        {
            base.resetNode();
            if(container != null)
            {
                nPCDialogueAttacher.RemoveDialogue(container);
            }
        }

      
    }

    public delegate void DialogueHasFinished(string endPointId);

    [System.Serializable]
    public class EndPointContainer
    {
        public string id;
        public List<string> endPointChilds = new List<string>();

        public EndPointContainer(string id, string endPointChild = null)
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
        public bool startInstant;

        public DialogueContainer(Dialogue.Dialogue dialogue, DialogueHasFinished dialogueHasFinished, bool startInstant)
        {
            this.dialogue = dialogue;
            this.dialogueHasFinished = dialogueHasFinished;
            this.startInstant = startInstant;
        }
    }
}