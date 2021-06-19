using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "New QuestStateObject", menuName = "QuestSystem/QuestStateObject", order = 0)]
    public class QuestStateObject : ScriptableObject
    {
        [SerializeField] List<string> questNames = new List<string>();

        public void AddQuestName(string newName)
        {
            questNames.Add(newName);
        }

        public void RemoveQuestName(string newName)
        {
            questNames.Remove(newName);
        }

        public List<string> GetAllQuestNames() 
        {
            return questNames;
        }

        private void OnEnable()
        {
            if(questNames.Count <= 0)
            {
                questNames.Add("None");
            }
        }

    }

    //public class QuestState
    //{
    //    public string name;

    //}

}