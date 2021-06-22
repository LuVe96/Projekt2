using System.Collections;
using UnityEngine;

namespace QuestSystem.Quest
{
    [System.Serializable]
    public class BranchNodeData : EndpointMainNodeData
    {

        [SerializeField] EndPointContainer trueEndPoint = new EndPointContainer("true_endpoint");
        [SerializeField] EndPointContainer falseEndPoint = new EndPointContainer("false_endpoint");

        public EndPointContainer TrueEndPoint { get => trueEndPoint; set => trueEndPoint = value; }
        public EndPointContainer FalseEndPoint { get => falseEndPoint; set => falseEndPoint = value; }

        public BranchNodeData(string id) : base(id)
        {
            EndPointContainer.Add(trueEndPoint); 
            EndPointContainer.Add(falseEndPoint);
        }

        protected override void executeNode()
        {
        }

        protected override void execute()
        {
            if (CheckRequirements())
            {
                FinishNode(TrueEndPoint);
            } else
            {
                FinishNode(FalseEndPoint);
            }
        }

       

        public override void AddChildToEndPoint(string endPointId, string childId)
        {

            if (trueEndPoint.id == endPointId)
            {
                TrueEndPoint.endPointChilds.Add(childId);
            }
            else
            {
                FalseEndPoint.endPointChilds.Add(childId);
            }
        }

        public override void RemoveChildToFromPoint(string endPointId, string childId)
        {
            if (trueEndPoint.id == endPointId)
            {
                TrueEndPoint.endPointChilds.Remove(childId);
            }
            else
            {
                FalseEndPoint.endPointChilds.Remove(childId);
            }
        }
    }


}