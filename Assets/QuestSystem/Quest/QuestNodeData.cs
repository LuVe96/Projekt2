using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem.Quest
{

    [System.Serializable]
    public abstract class QuestNodeData
    {
        [SerializeField] protected string uID;
        [SerializeField] Rect rect;


        public Rect Rect { get => rect; set => rect = value; }
        public string UID { get => uID; set => uID = value; }

        //public abstract void finished(int nextChildIndex);
        //public virtual void execute() { }
    }
}
