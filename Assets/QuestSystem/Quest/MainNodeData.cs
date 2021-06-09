using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem.Quest
{
    public abstract class MainNodeData : QuestNodeData
    {

        [SerializeField] List<string> childrenIDs = new List<string>();
        [SerializeField] List<string> requirementIDs = new List<string>();

        public List<string> ChildrenIDs { get => childrenIDs; set => childrenIDs = value; }
        public List<string> RequirementIDs { get => requirementIDs; set => requirementIDs = value; }

        protected NodeHasFinished NodeHasFinished;

        public MainNodeData(string uID, NodeHasFinished nodeHasFinished)
        {
            this.uID = uID;
            NodeHasFinished = nodeHasFinished;
        }

        public abstract void execute();

    } 
}
