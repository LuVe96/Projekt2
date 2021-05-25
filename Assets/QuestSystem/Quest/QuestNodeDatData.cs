using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem.Quest
{
    [System.Serializable]
    public abstract class QuestNodeData
    {
        [SerializeField] string uID;
        [SerializeField] List<string> childrenIDs = new List<string>();
        [SerializeField] List<string> requirementIDs = new List<string>();
        [SerializeField] Rect rect;

        [SerializeField] public string testString = "";

        public Rect Rect { get => rect; set => rect = value; }
        public string UID { get => uID; set => uID = value; }
        public List<string> ChildrenIDs { get => childrenIDs; set => childrenIDs = value; }
        public List<string> RequirementIDs { get => requirementIDs; set => requirementIDs = value; }

        public abstract void execute();
        //public virtual void execute() { }
    }
}
