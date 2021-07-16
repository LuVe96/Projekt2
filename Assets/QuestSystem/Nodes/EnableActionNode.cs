using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace QuestSystem
{
    public class EnableActionNode : Node
    {
        public EnableActionNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate) 
            : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public EnableActionNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate) 
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        private EnableActionData EnableActionData { get => (EnableActionData)Questdata; set { } }

        PortSegment actionSegment;

        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {
            PortProps[] actionTypes = { new PortProps(ConnectionPointType.ActOut, PortPosition.Left) };
            actionSegment = new PortSegment(SegmentType.ActionSegment, actionTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(actionSegment.Type, actionSegment));
        }

        protected override GUIStyle UseNodeStyle()
        {
            style = new GUIStyle();
            style.normal.background = Resources.Load("node_green") as Texture2D;
            style.border = new RectOffset(20, 20, 20, 20);
            style.padding = new RectOffset(24, 24, 16, 16);
            return style;
        }
#if UNITY_EDITOR
        protected override void DrawNodeHeader()
        {
            GUILayout.Label("Enable Action", headerTextStyle);
            actionSegment.Begin();
            actionSegment.End();
        }

        protected override void DrawContent()
        {
           

            GUILayout.Label("GameObject:", textStyle);
            EnableActionData.GObject = (GameObject)EditorGUILayout.ObjectField(EnableActionData.GObject, typeof(GameObject), true);

            EditorGUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Disable:", textStyle);
            EnableActionData.Disable = GUILayout.Toggle(EnableActionData.Disable, "");
            GUILayout.EndHorizontal();
        }
#endif
    }

}