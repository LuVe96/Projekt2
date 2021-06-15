using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;
using System.Linq;

namespace QuestSystem.Quest
{
    public class QuestEditorWindow : EditorWindow
    {
        static Quest currentQuest = null;

        private List<Node> nodes = new List<Node>();
        private List<NodeConnection> connections = new List<NodeConnection>();
        Dictionary<string, Node> nodeLookUp = new Dictionary<string, Node>();

        private NodePort selectedInPoint;
        private NodePort selectedOutPoint;
        Vector2 scrollPosition;
        [NonSerialized]
        bool draggingCanvas = false;
        [NonSerialized]
        Vector2 draggingCanvasOffset;

        public Vector2 canvasSize = new Vector2(4000, 2000);

        [MenuItem("Tools/QuestWindow")]
        public static void Init()
        {
            EditorWindow.GetWindow(typeof(QuestEditorWindow), false, "Editor Window");
        }

        internal void Init(QuestEditor questEditor, UnityEngine.Object target)
        {
            Init();
            SetupEditor((Quest)target);
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged; 
        }

        private void OnSelectionChanged()
        {
            if (Selection.activeGameObject.GetComponent<Quest>() == null) return;

            Quest quest = Selection.activeGameObject.GetComponent<Quest>() as Quest;
            if (quest != null)
            {
                SetupEditor(quest);
                Repaint();
                Repaint();
            }
        }

        private void SetupEditor(Quest quest)
        {
            nodes.Clear();
            connections.Clear();
            selectedInPoint = null;
            selectedOutPoint = null;

            currentQuest = quest;

            SetupNodes();
            SetupConnections();
        }

        //FOR_NEW: 02 Setup with loaded Data
        private void SetupNodes()
        {
            foreach (var node in currentQuest.Nodes) // Add nodes with nodeData
            {
                switch (node)
                {
                    case QuestStartNodeData n:
                        nodes.Add(new StartNode(OnClickNodePort, n));
                        break;
                    case QuestDialogueNodeData n:
                        nodes.Add(new DialogueNode(OnClickNodePort, n));
                        break;
                    case InventoryRequireData n:
                        nodes.Add(new InventoryRequirementNode(OnClickNodePort, n)); break;
                    case InventoryActionData n:
                        nodes.Add(new InventoryActionNode(OnClickNodePort, n)); break;
                    case EnableActionData n:
                        nodes.Add(new EnableActionNode(OnClickNodePort, n)); break;
                    case StandartNodeData n:
                        nodes.Add(new StandartNode(OnClickNodePort, n)); break;
                    default:
                        break;
                }
            }

            // fill Lookup to get nodes over ID in futher steps
            foreach (Node _node in nodes)
            {
                nodeLookUp[_node.Questdata.UID] = _node;
            }
        }

        private void SetupConnections()
        {
            foreach (var node in nodes) // Adds connections 
            {
                if(node.Questdata is MainNodeData)
                {
                    CreateMatchingConnections(node, (node.Questdata as MainNodeData).ChildrenIDs, SegmentType.MainSegment);
                    CreateMatchingConnections(node, (node.Questdata as MainNodeData).RequirementIDs, SegmentType.RequirementSegment);
                    CreateMatchingConnections(node, (node.Questdata as MainNodeData).ActionIDs, SegmentType.ActionSegment);
                }
                if(node.Questdata is QuestDialogueNodeData)
                {
                    foreach (DialogueEndPointContainer container in (node.Questdata as QuestDialogueNodeData).DialogueEndPointContainer)
                    {
                        CreateMatchingConnections(node, container.endPointChilds, SegmentType.DialogueEndPointSegment, SegmentType.MainSegment, container.id);
                    }
                }

            }
        }

        private void CreateMatchingConnections(Node node, List<string> idList, SegmentType segmentType, 
            SegmentType goalSegmentType = SegmentType.undefined, string endPortId = null)
        {

            if (goalSegmentType == SegmentType.undefined)
                goalSegmentType = segmentType;

            foreach (var childId in idList)
            {
                if (nodeLookUp.ContainsKey(childId))
                {
                    foreach (var segment in node.Segments.Where(s => s.Key == segmentType))
                    {
                        foreach (var childSegment in nodeLookUp[childId].Segments.Where(s => s.Key == goalSegmentType))
                        {
                            switch (segmentType)
                            {
                                case SegmentType.MainSegment:
                                    AddConnection(segment.Value, childSegment.Value, ConnectionPointType.MainIn, ConnectionPointType.MainOut);
                                    break;
                                case SegmentType.RequirementSegment:
                                    AddConnection(segment.Value, childSegment.Value, ConnectionPointType.ReqIn, ConnectionPointType.ReqOut);
                                    break;
                                case SegmentType.ActionSegment:
                                    AddConnection(segment.Value, childSegment.Value, ConnectionPointType.ActIn, ConnectionPointType.ActOut);
                                    break;
                                case SegmentType.DialogueEndPointSegment:
                                    if((segment.Value as EndPortSegment).EndPortId == endPortId)
                                        AddConnection(segment.Value, childSegment.Value, ConnectionPointType.MainIn, ConnectionPointType.MainOut);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void AddConnection( PortSegment segment, PortSegment childSegment, ConnectionPointType inType, ConnectionPointType outType)
        {
            NodePort inPort = segment.NodePortsDict[inType];
            NodePort outPort = childSegment.NodePortsDict[outType];
            connections.Add(new NodeConnection(inPort, outPort, OnClickRemoveConnection));

        }

        private void OnGUI()
        {
            if(currentQuest == null)
            {
                EditorGUILayout.LabelField("Select A Quest");
            }
            else
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                Rect canvas = GUILayoutUtility.GetRect(canvasSize.x, canvasSize.y);
                Texture2D texture = Resources.Load("background") as Texture2D;
                Rect textCoords = new Rect(0, 0, canvasSize.x / texture.width, canvasSize.y / texture.height);
                GUI.DrawTextureWithTexCoords(canvas, texture, textCoords);

                DrawNodes();
                DrawConnections();

                DrawConnectionLine(Event.current);

                EditorGUILayout.EndScrollView();

                ProcessNodeEvents(Event.current);
                ProccessEvents(Event.current);

            }
  

            if (GUI.changed)
            {
                Repaint();
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene()); // Make Scene Dirty, show that it has to be saved
            }
        }

   

        private bool IsNodeAtPoint(Vector2 point)
        {
            foreach (Node node in nodes)
            {
                if (node.Rect.Contains(point))
                {
                    return true;
                }
            }
            return false;
        }

        private void ProcessNodeEvents(Event current)
        {
            if (nodes != null)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = nodes[i].ProcessEvents(current, scrollPosition);

                    if (guiChanged)
                    {
                        GUI.changed = true;
                    }
                }
            }
        }

        private void ProccessEvents(Event current)
        {
           
            if (current.type == EventType.MouseDown)
            {
                // Open PopupMenu
                if (current.button == 1)
                {
                    ProcessContextMenu(current.mousePosition + scrollPosition);
                }
               
                if (current.button == 0 && !IsNodeAtPoint(current.mousePosition + scrollPosition))
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                }
            }
            else if (current.type == EventType.MouseDrag && draggingCanvas && current.button == 0)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;

            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }
        }

        //FOR_NEW: 01 Add new Node by ContextMenu
        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add Standart node"), false, () => OnClickAddNode(mousePosition, QuestNodeType.StandartNode));
            genericMenu.AddItem(new GUIContent("Add start node"), false, () => OnClickAddNode(mousePosition, QuestNodeType.StartNode));
            genericMenu.AddItem(new GUIContent("Add Dialogue node"), false, () => OnClickAddNode(mousePosition, QuestNodeType.DialogueNode));
            genericMenu.AddItem(new GUIContent("Add Invnetory Requirement node"), false, () => OnClickAddNode(mousePosition, QuestNodeType.InventoryRequirementNode));
            genericMenu.AddItem(new GUIContent("Add Invnetory Action node"), false, () => OnClickAddNode(mousePosition, QuestNodeType.InventoryActionNode));
            genericMenu.AddItem(new GUIContent("Add Enable Action node"), false, () => OnClickAddNode(mousePosition, QuestNodeType.EnableActionNode));;
            genericMenu.ShowAsContext();
        }
 
        private void OnClickAddNode(Vector2 mousePosition, QuestNodeType type)
        {
            QuestNodeData questdate = currentQuest.CreateNewNode(type);
            switch (type)
            {
                case QuestNodeType.StartNode:
                    nodes.Add(new StartNode(mousePosition, 200, 100, OnClickNodePort, questdate));
                    break;
                case QuestNodeType.DialogueNode:
                    nodes.Add(new DialogueNode(mousePosition, 200, 100, OnClickNodePort, questdate));
                    break;
                case QuestNodeType.InventoryRequirementNode:
                    nodes.Add(new InventoryRequirementNode(mousePosition, 200, 100, OnClickNodePort, questdate));
                    break;
                case QuestNodeType.InventoryActionNode:
                    nodes.Add(new InventoryActionNode(mousePosition, 200, 100, OnClickNodePort, questdate));
                    break;
                case QuestNodeType.EnableActionNode:
                    nodes.Add(new EnableActionNode(mousePosition, 200, 100, OnClickNodePort, questdate));
                    break;
                case QuestNodeType.StandartNode:
                    nodes.Add(new StandartNode(mousePosition, 200, 100, OnClickNodePort, questdate));
                    break;
                default:
                    break;
            }

        }

        private void DrawNodes()
        {
            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Draw();
                }
            }
        }

        #region NodePorts / Connections

        private void DrawConnectionLine(Event current)
        {
            if (selectedInPoint != null && selectedOutPoint == null)
            {
                DrawHandel(current, selectedInPoint);
            }

            if (selectedOutPoint != null && selectedInPoint == null)
            {
                DrawHandel(current, selectedOutPoint);
            }
        }

        private void DrawHandel(Event current, NodePort selectedPort)
        {
            float multiplier = selectedPort.PortPosition == PortPosition.Left ? 1 : -1;

            Handles.DrawBezier(
                selectedPort.Rect.center,
                current.mousePosition,
                selectedPort.Rect.center + Vector2.left * 50f * multiplier,
                current.mousePosition - Vector2.left * 50f * multiplier,
                Color.white,
                null,
                2f
            );
            GUI.changed = true;
        }

        private void DrawConnections()
        {
            if (connections != null)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    connections[i].Draw();
                }
            }
        }

        //FOR_NEW: Define and Create Connections
        private void OnClickNodePort(NodePort port, ConnectionPointType type)
        {
            switch (type)
            {
                case ConnectionPointType.MainIn:
                    OnClickInPoint(port, ConnectionPointType.MainOut);
                    break;
                case ConnectionPointType.MainOut:
                    OnClickOutPoint(port, ConnectionPointType.MainIn);
                    break;
                case ConnectionPointType.ReqIn:
                    OnClickInPoint(port, ConnectionPointType.ReqOut);
                    break;
                case ConnectionPointType.ReqOut:
                    OnClickOutPoint(port, ConnectionPointType.ReqIn);
                    break;
                case ConnectionPointType.ActIn:
                    OnClickInPoint(port, ConnectionPointType.ActOut);
                    break;
                case ConnectionPointType.ActOut:
                    OnClickOutPoint(port, ConnectionPointType.ActIn);
                    break;
                default:
                    break;
            }
        }

        private void OnClickInPoint(NodePort inPoint, ConnectionPointType counterpartType)
        {
            selectedInPoint = inPoint;

            if (selectedOutPoint != null)
            {
                if (selectedOutPoint.Segment != selectedInPoint.Segment && selectedOutPoint.Type == counterpartType)
                {
                    CreateConnection(selectedInPoint.Segment);
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickOutPoint(NodePort outPoint, ConnectionPointType counterpartType)
        {
            selectedOutPoint = outPoint;

            if (selectedInPoint != null)
            {
                if (selectedOutPoint.Segment != selectedInPoint.Segment && selectedInPoint.Type == counterpartType)
                {
                    CreateConnection(selectedInPoint.Segment);
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickRemoveConnection(NodeConnection connection)
        {
            connections.Remove(connection);
        }

        private void CreateConnection( PortSegment segment)
        {

            connections.Add(new NodeConnection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));

            selectedInPoint.Segment.Node.AddChildsToData(selectedOutPoint.Segment.Node, segment);
        }

        private void ClearConnectionSelection()
        {
            selectedInPoint = null;
            selectedOutPoint = null;
        }

        #endregion

    }
}
