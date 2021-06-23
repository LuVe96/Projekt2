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
        public InventoryActionNode(OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate) 
            : base(OnClickNodePort, _questdata, repaintEditorDelegate)
        {
        }

        public InventoryActionNode(Vector2 position, float width, float height, OnClickNodePortDelegate OnClickNodePort, QuestNodeData _questdata, RepaintEditorDelegate repaintEditorDelegate) 
            : base(position, width, height, OnClickNodePort, _questdata, repaintEditorDelegate)
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
        protected override GUIStyle UseNodeStyle()
        {
            style = new GUIStyle();
            style.normal.background = Resources.Load("node_green") as Texture2D;
            style.border = new RectOffset(20, 20, 20, 20);
            style.padding = new RectOffset(24, 24, 16, 16);
            return style;
        }

        protected override void DrawNodeHeader()
        {
            GUILayout.Label("Invnetory Action", headerTextStyle);
            actionSegment.Begin();
            actionSegment.End();
        }

        protected override void DrawContent()
        {

            GUILayout.Label("Action:", textStyle);
            InventoryActionData.InventoryAction = (InventorySelectionType)EditorGUILayout.EnumPopup(InventoryActionData.InventoryAction);

            GUILayout.Label("Item:", textStyle);
            GUILayout.BeginHorizontal();
            InventoryActionData.LootItem = (LootItem)EditorGUILayout.ObjectField(InventoryActionData.LootItem, typeof(LootItem), false);
            EditorGUILayout.Space(10);
            InventoryActionData.Count = EditorGUILayout.IntField(InventoryActionData.Count, GUILayout.Width(25));
            GUILayout.EndHorizontal();
        }

     
    }
    
}
