using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem.Quest
{
    [System.Serializable]
    public class QuestStartNodeData : MainNodeData
    {
        [SerializeField] public string[] requieredStates;

        [SerializeField] public string testStartString = "";

        public QuestStartNodeData(string id, NodeHasFinished nodeHasFinished, GetNodeByID getNodeByID) : base(id, nodeHasFinished, getNodeByID)
        {
        }

        public override void execute()
        {
            Debug.Log("Execute QuestStartNode");
            NodeHasFinished(0);
        }
    }


}
 