using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{
    public class OtherQuestEndActionNode : Node
    {
        public OtherQuestEndActionNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public OtherQuestEndActionNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public OtherQuestEndActionData Data { get => (OtherQuestEndActionData)Questdata; set { } }

        PortSegment actionSegment;
        QuestVariableTemplate selectedVariable = null;
        int selectedOptionIndex = 0;

        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {
            PortProps[] actionTypes = { new PortProps(ConnectionPointType.ActOut, PortPosition.Left) };
            actionSegment = new PortSegment(SegmentType.ActionSegment, actionTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(actionSegment.Type, actionSegment));
        }
        protected override GUIStyle UseNodeStyle()
        {
            style = new GUIStyle();
            //style.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node3.png") as Texture2D;
            style.normal.background = Resources.Load("node_green") as Texture2D;
            style.border = new RectOffset(20, 20, 20, 20);
            style.padding = new RectOffset(24, 24, 16, 16);
            return style;
        }

        protected override void DrawNodeHeader()
        {
            GUILayout.Label("Quest End Action", headerTextStyle);
            actionSegment.Begin();
            actionSegment.End();
        }

        protected override void DrawContent()
        {
            GUILayout.Label("Quest: ", textStyle);
            Data.OtherQuest = (Quest.Quest)EditorGUILayout.ObjectField(Data.OtherQuest, typeof(Quest.Quest), true);
            GUILayout.Label("End: ", textStyle);
            Data.QuestEndType = (QuestEndType)EditorGUILayout.EnumPopup(Data.QuestEndType);
        }
    }

}