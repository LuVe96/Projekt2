using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem.Quest
{

    public delegate void NodeHasFinished(int nextChildIndex);
    public delegate QuestNodeData GetNodeByID(string id);

    [System.Serializable]
    public abstract class MainNodeData : QuestNodeData
    {

        [SerializeField] List<string> childrenIDs = new List<string>();
        [SerializeField] List<string> requirementIDs = new List<string>();
        [SerializeField] List<string> actionIDs = new List<string>();

        public List<string> ChildrenIDs { get => childrenIDs; set => childrenIDs = value; }
        public List<string> RequirementIDs { get => requirementIDs; set => requirementIDs = value; }
        public List<string> ActionIDs { get => actionIDs; set => actionIDs = value; }

        protected NodeHasFinished NodeHasFinished;
        protected GetNodeByID GetNodeByID;

        public MainNodeData(string uID, NodeHasFinished nodeHasFinished, GetNodeByID getNodeByID)
        {
            this.uID = uID;
            NodeHasFinished = nodeHasFinished;
            GetNodeByID = getNodeByID;
        }

        public abstract void execute();

    } 
}
