using QuestSystem;
using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace QuestSystem
{
    public class RequirementNode : Node
    {
        public RequirementNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata) : base(OnClickNodePort, _questdata)
        {
        }

        public RequirementNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata) : base(position, width, height, OnClickNodePort, _questdata)
        {
        }

        PortSegment requirementSegment;

        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {
            PortProps[] reqTypes = { new PortProps(ConnectionPointType.ReqOut, PortPosition.Right) };
            requirementSegment = new PortSegment(SegmentType.RequirementSegment, reqTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(requirementSegment.Type, requirementSegment));
        }

        protected override void DrawContent()
        {
            requirementSegment.Begin();
            EditorGUILayout.LabelField("Req ");
            requirementSegment.End();
        }

    }
    
}
