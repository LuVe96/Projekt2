using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{

    public enum QuestNodeType 
    {
        StartNode, DialogueNode,
    }

    public enum ConnectionPointType {
        MainIn,
        MainOut,
        ReqIn,
        ReqOut
    }

    public enum SegmentType
    {
        MainSegment,
        RequirementSegment
    }

}