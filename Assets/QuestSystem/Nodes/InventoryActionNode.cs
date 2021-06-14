using QuestSystem;
using QuestSystem.Quest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace QuestSystem
{
    public class InventoryActionNode : Node
    {
        public InventoryActionNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata) : base(OnClickNodePort, _questdata)
        {
        }

        public InventoryActionNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata) : base(position, width, height, OnClickNodePort, _questdata)
        {
        }

        public InventoryActionData InventoryActionData { get => (InventoryActionData)Questdata; set { } }

        PortSegment actionSegment;

        protected override void SetupSegments(OnClickNodePortDelegate OnClickNodePort, List<KeyValuePair<SegmentType, PortSegment>> segments)
        {
            PortProps[] actionTypes = { new PortProps(ConnectionPointType.ActOut, PortPosition.Left) };
            actionSegment = new PortSegment(SegmentType.ActionSegment, actionTypes, OnClickNodePort, this);
            segments.Add(new KeyValuePair<SegmentType, PortSegment>(actionSegment.Type, actionSegment));
        }

        protected override void DrawContent()
        {
            EditorGUILayout.LabelField("Invnetory Action");
            actionSegment.Begin();
            actionSegment.End();

            GUILayout.Label("Action:");
            InventoryActionData.InventoryAction = (InventorySelectionType)EditorGUILayout.EnumPopup(InventoryActionData.InventoryAction);

            GUILayout.Label("Item:");
            GUILayout.BeginHorizontal();
            InventoryActionData.LootItem = (LootItem)EditorGUILayout.ObjectField(InventoryActionData.LootItem, typeof(LootItem), false);
            EditorGUILayout.Space(10);
            InventoryActionData.Count = EditorGUILayout.IntField(InventoryActionData.Count, GUILayout.Width(25));
            GUILayout.EndHorizontal();
        }

    }
    
}
