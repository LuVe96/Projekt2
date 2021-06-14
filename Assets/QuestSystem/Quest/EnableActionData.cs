using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem.Quest
{
    [System.Serializable]
    public class EnableActionData : ActionNodeData
    {
        [SerializeField] GameObject gObject;
        [SerializeField] bool disable = false;

        public EnableActionData(string id)
        {
            this.uID = id;
        }

        public GameObject GObject { get => gObject; set => gObject = value; }
        public bool Disable { get => disable; set => disable = value; }

        public override void executeAction()
        {
            Debug.Log("Execute Enable Action");

            if(GObject != null)
            {
                GObject.SetActive(!Disable);
            }
            else
            {
                Debug.LogWarning("No GameObject set on Enable Action");
            }
        }
    }

}