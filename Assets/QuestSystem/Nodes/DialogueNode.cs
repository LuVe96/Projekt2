using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace QuestSystem
{

    public class DialogueNode : Node
    {

        public DialogueNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata) : base(OnClickNodePort, _questdata)
        {
        }

        public DialogueNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata) : base(position, width, height, OnClickNodePort, _questdata)
        {
        }

        PortSegment mainSegment;
        PortSegment requirementSegment;
        PortSegment actionSegment;

        public QuestDialogueNodeData DialogueNodeData { get => (QuestDialogueNodeData)Questdata; set { } }


        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {

            PortProps[] mainTypes = { new PortProps(ConnectionPointType.MainIn, PortPosition.Right), new PortProps(ConnectionPointType.MainOut, PortPosition.Left) };
            mainSegment = new PortSegment(SegmentType.MainSegment, mainTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(mainSegment.Type, mainSegment));

            PortProps[] reqTypes = { new PortProps(ConnectionPointType.ReqIn, PortPosition.Left) };
            requirementSegment = new PortSegment(SegmentType.RequirementSegment, reqTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(requirementSegment.Type, requirementSegment));

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
            GUILayout.Label("Dialogue Node");
            GUILayout.Space(20);
            GUILayout.Label("Dialogue:");
            DialogueNodeData.Dialogue = (Dialogue.Dialogue)EditorGUILayout.ObjectField(DialogueNodeData.Dialogue, typeof(Dialogue.Dialogue),false);
            GUILayout.Space(20);
            GUILayout.Label("NPC:");
            DialogueNodeData.NPCDialogueAttacher = (NPCDialogueAttacher)EditorGUILayout.ObjectField(DialogueNodeData.NPCDialogueAttacher, typeof(NPCDialogueAttacher), true);
            mainSegment.End();

            requirementSegment.Begin(); 
            EditorGUILayout.LabelField("Requirements");
            requirementSegment.End();

            actionSegment.Begin();
            EditorGUILayout.LabelField("Actions");
            actionSegment.End();
        }


    }

}