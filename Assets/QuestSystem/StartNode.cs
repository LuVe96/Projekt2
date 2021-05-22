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


        public StartNode( Action<NodePort> OnClickInPoint, Action<NodePort> OnClickOutPoint, QuestNodeData _questdata) : base( OnClickInPoint, OnClickOutPoint, _questdata)
        {
        }

        public StartNode(Vector2 position, float width, float height, Action<NodePort> OnClickInPoint, Action<NodePort> OnClickOutPoint, QuestNodeData _questdata) : base(position, width, height, OnClickInPoint, OnClickOutPoint, _questdata)
        {
        }

        public QuestStartNodeData StartNodeData { get => (QuestStartNodeData) Questdata; set { } }


        protected override GUIStyle UseStyle()
        {
            return base.UseStyle();
        }

        public override void Draw()
        {
            InPort.Draw();
            OutPort.Draw();
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