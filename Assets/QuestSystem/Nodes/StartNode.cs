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


        protected override GUIStyle UseNodeStyle()
        {
            return base.UseNodeStyle();
        }

        protected override void DrawNodeHeader()
        {
            DrawEditabelHeader("StartNodeData");
            GUILayout.Space(10);
            mainSegment.Begin();
            mainSegment.End();
        }

        protected override void DrawContent()
        {
         

            QuestStateObject qso = Resources.Load("QuestStateData") as QuestStateObject;
            string[] choices = qso.GetAllQuestNames().ToArray();
            int selectedIndex = qso.GetAllQuestNames().IndexOf(StartNodeData.ActiveAfter);
            GUILayout.Label("Start After Quest: ");
            selectedIndex = EditorGUILayout.Popup(selectedIndex != -1 ? selectedIndex : 0, choices);
            StartNodeData.ActiveAfter = choices[selectedIndex];

            GUILayout.Space(20);

            actionSegment.Begin();
            EditorGUILayout.LabelField("Actions", rightPortTextStyle);
            actionSegment.End();
        }

       
    }

} 