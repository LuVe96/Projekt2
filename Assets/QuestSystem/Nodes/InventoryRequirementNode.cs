using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{

    public class InventoryRequirementNode : Node
    {

        public InventoryRequirementNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate)
            : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public InventoryRequirementNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate) 
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        PortSegment requirementSegment;

        public InventoryRequireData InventoryRequirementData { get => (InventoryRequireData)Questdata; set { } }


        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {

            PortProps[] reqTypes = { new PortProps(ConnectionPointType.ReqOut, PortPosition.Right) };
            requirementSegment = new PortSegment(SegmentType.RequirementSegment, reqTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(requirementSegment.Type, requirementSegment));
        }


        protected override GUIStyle UseNodeStyle()
        {
            style = new GUIStyle();
            style.normal.background = Resources.Load("node_red") as Texture2D;
            style.border = new RectOffset(20, 20, 20, 20);
            style.padding = new RectOffset(24, 24, 16, 16);
            return style;
        }
        protected override void DrawNodeHeader()
        {
            GUILayout.Label("Invnetory Requirement", headerTextStyle);
            GUILayout.Space(10);
            requirementSegment.Begin();
            requirementSegment.End();

        }

        protected override void DrawContent()
        {
          


            GUILayout.Label("Item:");
            GUILayout.BeginHorizontal();
            InventoryRequirementData.LootItem = (LootItem)EditorGUILayout.ObjectField(InventoryRequirementData.LootItem, typeof(LootItem), false);
            EditorGUILayout.Space(10);
            InventoryRequirementData.Count = EditorGUILayout.IntField(InventoryRequirementData.Count, GUILayout.Width(25));
            GUILayout.EndHorizontal();

        }


    }

}
