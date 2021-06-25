using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace QuestSystem.Quest
{
    [Serializable]
    public class EventActionData : ActionNodeData
    {
        [SerializeField] MonoBehaviour monoBehaviour;
        [SerializeField] string methodeName;

        public EventActionData(string id)
        {
            this.uID = id;
        }

        public MonoBehaviour MonoBehaviour { get => monoBehaviour; set => monoBehaviour = value; }
        public string MethodeName { get => methodeName; set => methodeName = value; }

        public override void executeAction()
        {

            if (monoBehaviour != null)
            {
                MethodInfo methodInfo = MonoBehaviour.GetType().GetMethod(MethodeName);
                var t = typeof(Action);
                Action method = (Action)Delegate.CreateDelegate(t, MonoBehaviour, methodInfo);
                method.Invoke();
            }
            else
            {
                Debug.LogWarning("No Monobehavior set on Event Action");
            }
        }
    }
}