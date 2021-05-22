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


        public StartNode(GUIStyle nodeStyle, Action<NodePort> OnClickInPoint, Action<NodePort> OnClickOutPoint, QuestNodeData _questdata/*, NodeChanged _nodeChanged*/) : base(nodeStyle, OnClickInPoint, OnClickOutPoint, _questdata/*, _nodeChanged*/)
        {
        }

        public StartNode(Vector2 position, float width, float height, GUIStyle nodeStyle, Action<NodePort> OnClickInPoint, Action<NodePort> OnClickOutPoint, QuestNodeData _questdata/*, NodeChanged _nodeChanged*/) : base(position, width, height, nodeStyle, OnClickInPoint, OnClickOutPoint, _questdata/*, _nodeChanged*/)
        {
        }

        public QuestStartNodeData StartNodeData { get => (QuestStartNodeData) questdata; set { } }

        public override void Draw()
        {
            inPort.Draw();
            outPort.Draw();
            GUILayout.BeginArea(Rect, style);

            GUILayout.Label("StartNodeData");
            StartNodeData.testString = EditorGUILayout.TextField(StartNodeData.testString);
            StartNodeData.testStartString = EditorGUILayout.TextField(StartNodeData.testStartString);

            GUILayout.EndArea();

            if (GUI.changed)
            {
               
            }
        }
    }

} 