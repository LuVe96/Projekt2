using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{

    public class StandartNode : Node
    {

        public StandartNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate) 
            : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public StandartNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        PortSegment mainSegment;
        PortSegment requirementSegment;
        PortSegment actionSegment;

        public StandartNodeData StandartNodeData { get => (StandartNodeData)Questdata; set { } }


        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {

            PortProps[] mainTypes = { new PortProps(ConnectionPointType.MainIn, PortPosition.Right), new PortProps(ConnectionPointType.MainOut, PortPosition.Left) };
            mainSegment = new PortSegment(SegmentType.MainSegment, mainTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(mainSegment.Type, mainSegment));

            PortProps[] reqTypes = { new PortProps(ConnectionPointType.ReqIn, PortPosition.Left) };
            requirementSegment = new PortSegment(SegmentType.RequirementSegment, reqTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(requirementSegment.Type, requirementSegment));

            PortProps[] actionTypes = { new PortProps(ConnectionPointType.ActIn, PortPosition.Right) };
            actionSegment = new PortSegment(SegmentType.ActionSegment, actionTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(actionSegment.Type, actionSegment));
        }


        protected override GUIStyle UseNodeStyle()
        {
            return base.UseNodeStyle();
        }

        protected override void DrawContent()
        {
            DrawHeader("Standart Node");
            mainSegment.Begin();
            mainSegment.End();
            GUILayout.Space(20);

            requirementSegment.Begin();
            EditorGUILayout.LabelField("Requirements", leftPortTextStyle);
            requirementSegment.End();

            actionSegment.Begin();
            EditorGUILayout.LabelField("Actions", rightPortTextStyle);
            actionSegment.End();
        }


    }

}
