using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem.Quest
{
    public abstract class EndpointMainNodeData : MainNodeData
    {
        [SerializeField] List<EndPointContainer> endPointContainer = new List<EndPointContainer>();
        public List<EndPointContainer> EndPointContainer { get => endPointContainer; set => endPointContainer = value; }

        public EndpointMainNodeData(string id) : base(id)
        {

        }

        protected abstract override void executeNode();

        public abstract void AddChildToEndPoint(string endPointId, string childId);
        public abstract void RemoveChildToFromPoint(string endPointId, string childId);
    }
}