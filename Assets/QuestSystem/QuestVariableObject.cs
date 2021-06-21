using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem
{
    [CreateAssetMenu(fileName = "New QuestVariableObject", menuName = "QuestSystem/QuestVariableObject", order = 0)]
    public class QuestVariableObject : ScriptableObject
    {
        [SerializeField] List<QuestVariableTemplate> questVariableTemplates = new List<QuestVariableTemplate>();

        public void AddQuestVarialbe(QuestVariableTemplate variable)
        {
            questVariableTemplates.Add(variable);
        }

        //public void RemoveQuestName(string newName)
        //{
        //    questNames.Remove(newName);
        //}

        public List<QuestVariableTemplate> GetAllQuestVariableTemplates()
        {
            return questVariableTemplates;
        }
    }

    [System.Serializable]
    public class QuestVariableTemplate
    {
        [SerializeField] string title = "";
        [SerializeField] QuestVariableType type = QuestVariableType.Bool;
        [SerializeField] List<string> datas = new List<string>();

        public QuestVariableTemplate()
        {
        }

        public string Title { get => title; set => title = value; }
        public List<string> Datas { get => datas; set => datas = value; }
        public QuestVariableType Type { get => type; set
            {
                switch (value)
                {
                    case QuestVariableType.Bool:
                        this.Datas.Clear();
                        this.datas.Add("True");
                        this.datas.Add("False");
                        break;
                    case QuestVariableType.Enum:
                        break;
                    default:
                        break;
                }

                type = value;
            }
        }

        //public QuestVariableTemplate(string title, QuestVariableType type, List<string> datas = null)
        //{
        //    this.title = title;
        //    this.type = type;

        //    if(datas == null)
        //    {
        //        if( type == QuestVariableType.Bool)
        //        {
        //            this.datas.Add("True");
        //            this.datas.Add("False");
        //        }
        //    }
        //    else
        //    {
        //        this.datas = datas;
        //    }
        //}


    }

    public enum QuestVariableType
    {
        Bool, Enum
    }

}