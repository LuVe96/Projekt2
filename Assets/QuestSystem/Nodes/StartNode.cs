using QuestSystem;
using QuestSystem.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{

    public class StartNode : Node
    {

        public StartNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate) 
            : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public StartNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        PortSegment mainSegment;
        PortSegment actionSegment;

        public QuestStartNodeData StartNodeData { get => (QuestStartNodeData)Questdata; set { } }


        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {

            PortProps[] mainTypes = { new PortProps(ConnectionPointType.MainIn, PortPosition.Right)};
            mainSegment = new PortSegment(SegmentType.MainSegment, mainTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>( mainSegment.Type, mainSegment));

            PortProps[] actionTypes = { new PortProps(ConnectionPointType.ActIn, PortPosition.Right) };
            actionSegment = new PortSegment(SegmentType.ActionSegment, actionTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(actionSegment.Type, actionSegment));
        }


        protected override GUIStyle UseStyle()
        {
            return base.UseStyle();
        }

        protected override void DrawContent()
        {
            mainSegment.Begin();
            GUILayout.Label("StartNodeData");
            StartNodeData.testString = EditorGUILayout.TextField(StartNodeData.testString);
            StartNodeData.testStartString = EditorGUILayout.TextField(StartNodeData.testStartString);
            mainSegment.End();

            actionSegment.Begin();
            EditorGUILayout.LabelField("Actions");
            actionSegment.End();
        }


    }

} 