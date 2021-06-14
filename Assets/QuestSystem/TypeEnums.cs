using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{

    public enum QuestNodeType 
    {
        StartNode, DialogueNode, StandartNode,
        InventoryRequirementNode, 
        InventoryActionNode, EnableActionNode
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
        MainSegment,
        RequirementSegment,
        ActionSegment
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