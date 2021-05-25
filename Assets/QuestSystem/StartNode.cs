using QuestSystem;
using QuestSystem.Quest;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace QuestSystem
{

    public class StartNode : Node
    {


        public StartNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata) : base( _questdata)
        {
            setupSegments(OnClickNodePort);
        }

        public StartNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata) : base(position, width, height, _questdata)
        {
            setupSegments(OnClickNodePort);
        }

        NodeSegment mainSegment;

        public QuestStartNodeData StartNodeData { get => (QuestStartNodeData)Questdata; set { } }


        private void setupSegments(OnClickNodePortDelegate OnClickNodePort)
        {
            ConnectionPointType[] types = { ConnectionPointType.MainIn, ConnectionPointType.MainOut };
            mainSegment = new NodeSegment(types, OnClickNodePort, this);
            Segments.Add(new KeyValuePair<SegmentType, NodeSegment>( SegmentType.MainSegment, mainSegment));
        }


        protected override GUIStyle UseStyle()
        {
            return base.UseStyle();
        }

        public override void Draw()
        {
            //InPort.Draw();
            //OutPort.Draw();
            GUILayout.BeginArea(Rect, style);

            GUILayout.Label("StartNodeData");

            mainSegment.Begin();
            StartNodeData.testString = EditorGUILayout.TextField(StartNodeData.testString);
            StartNodeData.testStartString = EditorGUILayout.TextField(StartNodeData.testStartString);
            mainSegment.End();

            GUILayout.EndArea();
            mainSegment.DrawPorts();

            if (GUI.changed)
            {
               
            }
        }

    }

} 