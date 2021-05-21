using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem.Quest
{
    [System.Serializable]
    public abstract class QuestNode 
    {
        [SerializeField] string uID;
        [SerializeField] List<string> childrenIDs = new List<string>();

        public abstract void execute();
    }
}
