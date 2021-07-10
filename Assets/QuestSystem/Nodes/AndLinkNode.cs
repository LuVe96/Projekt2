using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{
    public class AndLinkNode : Node
    {
        public AndLinkNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
                  : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
            this.OnClickNodePort = OnClickNodePort;
        }

        public AndLinkNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
            this.OnClickNodePort = OnClickNodePort;
        }

        OnClickNodePortDelegate OnClickNodePort;

        PortSegment mainSegment;


        public AndLinkNodeData Data { get => (AndLinkNodeData)Questdata; set { } }


        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {
            PortProps[] mainTypes = { new PortProps(ConnectionPointType.MainIn, PortPosition.Right) ,new PortProps(ConnectionPointType.MainOut, PortPosition.Left) };
            mainSegment = new PortSegment(SegmentType.MainSegment, mainTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(mainSegment.Type, mainSegment));

        }


        protected override GUIStyle UseNodeStyle()
        {
            return base.UseNodeStyle();
        }

#if UNITY_EDITOR
        protected override void DrawNodeHeader()
        {
            DrawEditabelHeader("And Link Node");
            mainSegment.Begin();
            mainSegment.End();
        }

        protected override void DrawContent()
        {
            GUILayout.Label("Inputs count:");
            Data.RequiredExecutes = EditorGUILayout.IntField(Data.RequiredExecutes);

        }
#endif
    }
}