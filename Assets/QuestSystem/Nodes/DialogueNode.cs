using QuestSystem.Dialogue;
using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace QuestSystem
{

    public class DialogueNode : Node
    {

        public DialogueNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate) 
            : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
            this.OnClickNodePort = OnClickNodePort;
            DrawDialogueEndPorts(DialogueNodeData.Dialogue);
        }

        public DialogueNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
            this.OnClickNodePort = OnClickNodePort;
        }

        OnClickNodePortDelegate OnClickNodePort;
        PortSegment mainSegment;
        PortSegment requirementSegment;
        PortSegment actionSegment;
        List<KeyValuePair<SegmentType, PortSegment>> dialogueEndPointSegments = new List<KeyValuePair<SegmentType, PortSegment>>();

        Dialogue.Dialogue currentDialogue = null;

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

            //PortProps[] endPointTypes = { new PortProps(ConnectionPointType.MainIn, PortPosition.Right) };
            //dialogueEndPointSegment = new PortSegment(SegmentType.DialogueEndPointSegment, endPointTypes, OnClickNodePort, this);
            //segments.Add(new KeyValuePair<SegmentType, PortSegment>(dialogueEndPointSegment.Type, dialogueEndPointSegment));
        }


        protected override GUIStyle UseNodeStyle()
        {
            return base.UseNodeStyle();
        }

        protected override void DrawContent()
        {
            DrawHeader("Dialogue Node");
            mainSegment.Begin();
            mainSegment.End();
            GUILayout.Space(20);

            GUILayout.Label("Dialogue:");
            DialogueNodeData.Dialogue = (Dialogue.Dialogue)EditorGUILayout.ObjectField(DialogueNodeData.Dialogue, typeof(Dialogue.Dialogue), false);
            GUILayout.Space(10);
            GUILayout.Label("NPC:");
            DialogueNodeData.NPCDialogueAttacher = (NPCDialogueAttacher)EditorGUILayout.ObjectField(DialogueNodeData.NPCDialogueAttacher, typeof(NPCDialogueAttacher), true);
            GUILayout.Space(10);
            GUILayout.Label("EndPoints:");
            DrawDialogueEndPorts(DialogueNodeData.Dialogue);

            GUILayout.Space(20);
            requirementSegment.Begin();
            EditorGUILayout.LabelField("Requirements", leftPortTextStyle);
            requirementSegment.End();

            actionSegment.Begin();
            EditorGUILayout.LabelField("Actions", rightPortTextStyle);
            actionSegment.End();
        }

        private void DrawDialogueEndPorts(Dialogue.Dialogue dialogue)
        {
            foreach (KeyValuePair<SegmentType, PortSegment> item in dialogueEndPointSegments)
            {
                item.Value.Begin();
                EditorGUILayout.LabelField((item.Value as EndPortSegment).EndPortDescription, rightPortTextStyle);
                item.Value.End();
            }

            if (currentDialogue == dialogue) return;

            bool removed = false;
            foreach (KeyValuePair<SegmentType, PortSegment> item in dialogueEndPointSegments)
            {
                RemoveChildsInData(null, item.Value);
                Segments.Remove(item);
                removed = true;
            }
            if (removed)
            {
                RepaintEditor(true);
            }
  
            dialogueEndPointSegments.Clear();

            if (dialogue == null)
            {
                currentDialogue = null;
                return;
            }

            PortProps[] endPointTypes = { new PortProps(ConnectionPointType.MainIn, PortPosition.Right) };
            foreach (DialogueEndPoint item in dialogue.GetEndPoints())
            {
                EndPortSegment segment = new EndPortSegment(item.id, item.description, endPointTypes, OnClickNodePort, this);
                dialogueEndPointSegments.Add(new KeyValuePair<SegmentType, PortSegment>(segment.Type, segment));
            }

            foreach (KeyValuePair<SegmentType, PortSegment> item in dialogueEndPointSegments)
            {
                Segments.Add(item);
            }

            currentDialogue = dialogue;

        }
    }

}