using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "New QuestStateObject", menuName = "QuestSystem/QuestStateObject", order = 0)]
    public class QuestStateObject : ScriptableObject
    {
        [SerializeField] List<QuestName> questStates = new List<QuestName>();

        public void AddQuestName(string newName, string logName)
        {
            questStates.Add(new QuestName(newName, logName));
        }

        public void RemoveQuestName(string newName)
        {
            questStates.Remove(questStates.Find(qS => qS.Name == newName));
        }

        public List<QuestName> GetAllQuestNames() 
        {
            //return questStates.Select( qs => qs.Name).ToList();
            return questStates;
        }

        private void OnEnable()
        {
            if(questStates.Count <= 0)
            {
                questStates.Add(new QuestName("None", "none"));
            }
        }

    }

    [System.Serializable]
    public class QuestName
    {
        [SerializeField] string name;
        [SerializeField] string logName;

        public QuestName(string name, string logName)
        {
            this.Name = name;
            this.LogName = logName;
        }

        public string Name { get => name; set => name = value; }
        public string LogName { get => logName; set => logName = value; }
    }

}