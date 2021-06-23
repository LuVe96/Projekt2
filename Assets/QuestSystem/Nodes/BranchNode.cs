using QuestSystem.Dialogue;
using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{
    public class BranchNode : Node
    {
        public BranchNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
                  : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
            this.OnClickNodePort = OnClickNodePort;
        }

        public BranchNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
            this.OnClickNodePort = OnClickNodePort;
        }

        OnClickNodePortDelegate OnClickNodePort;

        PortSegment mainSegment;
        PortSegment requirementSegment;
        EndPortSegment trueEndPointSegment;
        EndPortSegment falseEndPointSegment;

        public QuestDialogueNodeData DialogueNodeData { get => (QuestDialogueNodeData)Questdata; set { } }


        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {
            PortProps[] mainTypes = { new PortProps(ConnectionPointType.MainOut, PortPosition.Left) };
            mainSegment = new PortSegment(SegmentType.MainSegment, mainTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(mainSegment.Type, mainSegment));

            PortProps[] reqTypes = { new PortProps(ConnectionPointType.ReqIn, PortPosition.Left) };
            requirementSegment = new PortSegment(SegmentType.RequirementSegment, reqTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(requirementSegment.Type, requirementSegment));

            PortProps[] endPointTypes = { new PortProps(ConnectionPointType.MainIn, PortPosition.Right) };
            trueEndPointSegment = new EndPortSegment("true_endpoint", "True", endPointTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(trueEndPointSegment.Type, trueEndPointSegment));

            falseEndPointSegment = new EndPortSegment("false_endpoint", "False", endPointTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(falseEndPointSegment.Type, falseEndPointSegment));
        }


        protected override GUIStyle UseNodeStyle()
        {
            return base.UseNodeStyle(); 
        }

        protected override void DrawNodeHeader()
        {
            DrawEditabelHeader("Branch Node");
            mainSegment.Begin();
            mainSegment.End();
        }

        protected override void DrawContent()
        {

            trueEndPointSegment.Begin();
            EditorGUILayout.LabelField(trueEndPointSegment.EndPortDescription, rightPortTextStyle);
            trueEndPointSegment.End();
            GUILayout.Space(10);
            falseEndPointSegment.Begin();
            EditorGUILayout.LabelField(falseEndPointSegment.EndPortDescription, rightPortTextStyle);
            falseEndPointSegment.End();

            GUILayout.Space(20);
            requirementSegment.Begin();
            EditorGUILayout.LabelField("Requirements", leftPortTextStyle);
            requirementSegment.End();

        }

    }
}
