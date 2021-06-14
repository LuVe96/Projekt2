using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestSystem
{

    public class InventoryRequirementNode : Node
    {

        public InventoryRequirementNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata) : base(OnClickNodePort, _questdata)
        {
        }

        public InventoryRequirementNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata) : base(position, width, height, OnClickNodePort, _questdata)
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


        protected override GUIStyle UseStyle()
        {
            return base.UseStyle();
        }

        protected override void DrawContent()
        {
            GUILayout.Label("Invnetory Requirement");
            requirementSegment.Begin();
            requirementSegment.End();

            GUILayout.Space(20);


            GUILayout.Label("Item:");
            GUILayout.BeginHorizontal();
            InventoryRequirementData.LootItem = (LootItem)EditorGUILayout.ObjectField(InventoryRequirementData.LootItem, typeof(LootItem), false);
            EditorGUILayout.Space(10);
            InventoryRequirementData.Count = EditorGUILayout.IntField(InventoryRequirementData.Count, GUILayout.Width(25));
            GUILayout.EndHorizontal();

        }


    }

}
