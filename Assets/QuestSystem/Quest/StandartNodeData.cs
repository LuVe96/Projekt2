using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem.Quest
{
    //[CreateAssetMenu(fileName = "New QuestDialogueNode", menuName = "QuestSystem/QuestDialogueNode", order = 0)]
    [System.Serializable]
    public class StandartNodeData : MainNodeData
    {


        public StandartNodeData(string id, NodeHasFinished nodeHasFinished, GetNodeByID getNodeByID) : base(id, nodeHasFinished, getNodeByID)
        {
        }

        protected override void executeNode()
        {
            Debug.Log("Execute StandartNode");
            FinishNode(0);
        }

    }

}
