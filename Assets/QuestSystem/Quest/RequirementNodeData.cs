using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem.Quest
{
    public delegate void OnRequirementCheckPassed();
    [System.Serializable]
    public abstract class RequirementNodeData : QuestNodeData
    {
         public string Requirement = "";

        protected OnRequirementCheckPassed OnRequirementCheckPassed;

        //public RequirementNodeData(string id, NodeHasFinished nodeHasFinished) : base(id, nodeHasFinished)
        //{
        //}
        public bool CheckRequirement(OnRequirementCheckPassed onRequirementCheckPassed)
        {
            OnRequirementCheckPassed = onRequirementCheckPassed;
            return CheckRequirementNode();
        }
        public abstract bool CheckRequirementNode();

    }
}