using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{

    public class TriggerRequirementNode : Node
    {

        public TriggerRequirementNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public TriggerRequirementNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        PortSegment requirementSegment;

        public TriggerRequirementNodeData TriggerRequirementData { get => (TriggerRequirementNodeData)Questdata; set { } }


        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {

            PortProps[] reqTypes = { new PortProps(ConnectionPointType.ReqOut, PortPosition.Right) };
            requirementSegment = new PortSegment(SegmentType.RequirementSegment, reqTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(requirementSegment.Type, requirementSegment));
        }


        protected override GUIStyle UseNodeStyle()
        {
            style = new GUIStyle();
            style.normal.background = Resources.Load("node_red") as Texture2D;
            style.border = new RectOffset(20, 20, 20, 20);
            style.padding = new RectOffset(24, 24, 16, 16);
            return style;
        }

        protected override void DrawNodeHeader()
        {
            GUILayout.Label("Trigger Requirement", headerTextStyle);
            requirementSegment.Begin();
            requirementSegment.End();

        }

        protected override void DrawContent()
        {

            GUILayout.Label("Trigger:", textStyle);
            TriggerRequirementData.QuestTrigger = (QuestTrigger)EditorGUILayout.ObjectField(TriggerRequirementData.QuestTrigger, typeof(QuestTrigger), true);

        }


    }

}