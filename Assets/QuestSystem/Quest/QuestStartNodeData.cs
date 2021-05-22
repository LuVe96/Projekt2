using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem.Quest
{
    [System.Serializable]
    public class QuestStartNodeData : QuestNodeData
    {
        [SerializeField] public string[] requieredStates;


        [SerializeField] public string testStartString = "";

        public override void execute()
        {
            Debug.Log("Execute QuestStartNode");
        }
    }
}
 