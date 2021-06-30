using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{

    public class QuestEndNode : Node
    {

        public QuestEndNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public QuestEndNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        PortSegment mainSegment;
        PortSegment actionSegment;

        public QuestEndNodeData EndNodeData { get => (QuestEndNodeData)Questdata; set { } }


        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {

            PortProps[] mainTypes = { new PortProps(ConnectionPointType.MainOut, PortPosition.Left) };
            mainSegment = new PortSegment(SegmentType.MainSegment, mainTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(mainSegment.Type, mainSegment));

            PortProps[] actionTypes = { new PortProps(ConnectionPointType.ActIn, PortPosition.Right) };
            actionSegment = new PortSegment(SegmentType.ActionSegment, actionTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(actionSegment.Type, actionSegment));
        }


        protected override GUIStyle UseNodeStyle()
        {
            return base.UseNodeStyle();
        }
#if UNITY_EDITOR
        protected override void DrawNodeHeader()
        {
            DrawEditabelHeader("End Node");
            mainSegment.Begin();
            mainSegment.End();
        }

        protected override void DrawContent()
        {
         

            GUILayout.Label("End Node Type:", textStyle);
            EndNodeData.EndType = (QuestEndType) EditorGUILayout.EnumPopup(EndNodeData.EndType);

            GUILayout.Space(20);

            actionSegment.Begin();
            EditorGUILayout.LabelField("Actions", rightPortTextStyle);
            actionSegment.End();
        }
#endif
    }
}