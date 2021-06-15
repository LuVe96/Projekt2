using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace QuestSystem
{
    public class EndPortSegment : PortSegment
    {
        string endPortId;
        string endPortDescription;

        public EndPortSegment(string id, string discription, PortProps[] nodePorts, OnClickNodePortDelegate onClickNodePort, Node _node) : base(SegmentType.DialogueEndPointSegment, nodePorts, onClickNodePort, _node)
        {
            endPortId = id;
            EndPortDescription = discription;
        }


        public string EndPortId { get => endPortId; set => endPortId = value; }
        public string EndPortDescription { get => endPortDescription; set => endPortDescription = value; }
    }

}