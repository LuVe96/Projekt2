using System.Collections;
using UnityEngine;

namespace QuestSystem.Quest
{
    [System.Serializable]
    public class NoteNodeData : QuestNodeData
    {
        [SerializeField] string description;

        public NoteNodeData(string id)
        {
            this.UID = id;
        }

        public string Description { get => description; set => description = value; }
    }
}