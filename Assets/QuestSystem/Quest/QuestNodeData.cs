using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem.Quest
{
    public delegate void NodeHasFinished(int nextChildIndex);

    [System.Serializable]
    public abstract class QuestNodeData
    {
        [SerializeField] protected string uID;
        [SerializeField] Rect rect;

        [SerializeField] public string testString = "";

        public Rect Rect { get => rect; set => rect = value; }
        public string UID { get => uID; set => uID = value; }

        //public abstract void finished(int nextChildIndex);
        //public virtual void execute() { }
    }
}
