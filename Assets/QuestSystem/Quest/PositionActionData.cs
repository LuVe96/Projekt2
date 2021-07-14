using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    [System.Serializable]
    public class PositionActionData : ActionNodeData
    {
        [SerializeField] Transform targetTransform;
        [SerializeField] Transform goalTransform;

        public PositionActionData(string id)
        {
            this.uID = id;
        }

        public Transform TargetTransform { get => targetTransform; set => targetTransform = value; }
        public Transform GoalTransform { get => goalTransform; set => goalTransform = value; }

        public override void executeAction()
        {

            if (targetTransform != null && goalTransform != null)
            {
                targetTransform.gameObject.SetActive(false);
                targetTransform.position += new Vector3(0, 100, 0);
                Vector3 t = goalTransform.position;
                t.y = 100;
                targetTransform.position = t;
                targetTransform.position = goalTransform.position;
                targetTransform.rotation = goalTransform.rotation;
                targetTransform.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("No Monobehavior set on Event Action");
            }
        }
    }
}
