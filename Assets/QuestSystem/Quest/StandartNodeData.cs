using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem.Quest
{
    //[CreateAssetMenu(fileName = "New QuestDialogueNode", menuName = "QuestSystem/QuestDialogueNode", order = 0)]
    [System.Serializable]
    public class StandartNodeData : MainNodeData
    {


        public StandartNodeData(string id) : base(id)
        {
        }

        protected override void executeNode()
        {

            FinishNode();
        }

    }

}
