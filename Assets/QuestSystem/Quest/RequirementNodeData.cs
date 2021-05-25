using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem.Quest
{
    [System.Serializable]
    public class RequirementNodeData : QuestNodeData
    {
         public string Requirement = ""; 
        public override void execute()
        {
            throw new System.NotImplementedException();
        }
    }
}