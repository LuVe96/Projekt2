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

        public StartNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata) : base(OnClickNodePort, _questdata)
        {
        }

        public StartNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata) : base(position, width, height, OnClickNodePort, _questdata)
        {
        }

        PortSegment mainSegment;
        PortSegment requirementSegment;

        public QuestStartNodeData StartNodeData { get => (QuestStartNodeData)Questdata; set { } }


        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {

            ConnectionPointType[] mainTypes = { ConnectionPointType.MainIn, ConnectionPointType.MainOut };
            mainSegment = new PortSegment(SegmentType.MainSegment, mainTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>( mainSegment.Type, mainSegment));

            ConnectionPointType[] reqTypes = { ConnectionPointType.ReqIn };
            requirementSegment = new PortSegment(SegmentType.RequirementSegment, reqTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(requirementSegment.Type, requirementSegment));
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

            requirementSegment.Begin();
            EditorGUILayout.LabelField("Requirements");
            requirementSegment.End();

            requirementSegment.Begin();
            EditorGUILayout.LabelField("Requirements");
            EditorGUILayout.LabelField("Requirements");
            EditorGUILayout.LabelField("Requirements");
            EditorGUILayout.LabelField("Requirements");
            requirementSegment.End();
        }


    }

} 