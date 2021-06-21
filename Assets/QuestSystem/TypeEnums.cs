using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{

    public enum QuestNodeType 
    {
        StartNode, EndNode, DialogueNode, StandartNode,
        InventoryRequirementNode, VariableRequirementNode,
        InventoryActionNode, EnableActionNode, VariableActionNode,
    }

    public enum ConnectionPointType {
        MainIn,
        MainOut,
        ReqIn,
        ReqOut,
        ActIn,
        ActOut,
    }

    public enum PortPosition
    {
       Left,Right
    }

    public enum SegmentType
    {
        undefined,
        MainSegment,
        RequirementSegment,
        ActionSegment,
        DialogueEndPointSegment
    }

    public struct PortProps
    {
        public ConnectionPointType connectionPointType;
        public PortPosition portPosition;

        public PortProps(ConnectionPointType connectionPointType, PortPosition portPosition)
        {
            this.connectionPointType = connectionPointType;
            this.portPosition = portPosition;
        }
    }
}