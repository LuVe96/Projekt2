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
            Quest quest = Selection.activeGameObject.GetComponent<Quest>() as Quest;
            if (quest != null)
            {
                SetupEditor(quest);
                Repaint();
            }
        }

        //FOR_NEW: 02 Setup with loaded Data
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

        private void SetupNodes()
        {
            foreach (var node in currentQuest.Nodes) // Add nodes with nodeData
            {
                //if (node is QuestStartNodeData)
                //{
                //    nodes.Add(new StartNode(OnClickNodePort, node));
                //}
                //else if (node is RequirementNodeData)
                //{
                //    nodes.Add(new RequirementNode(OnClickNodePort, node));
                //}
                //else if (node is QuestDialogueNodeData)
                //{
                //    nodes.Add(new DialogueNode(OnClickNodePort, node));
                //}

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

            }
        }

        private void CreateMatchingConnections(Node node, List<string> idList, SegmentType segmentType)
        {
            foreach (var childId in idList)
            {
                if (nodeLookUp.ContainsKey(childId))
                {
                    foreach (var segment in node.Segments.Where(s => s.Key == segmentType))
                    {
                        foreach (var childSegment in nodeLookUp[childId].Segments.Where(s => s.Key == segmentType))
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
                EditorGUILayout.LabelField("Name: " + currentQuest.questName);
                currentQuest.questName = EditorGUILayout.TextField("Name: ", currentQuest.questName);

                DrawNodes();
                DrawConnections();

                DrawConnectionLine(Event.current);

                ProcessNodeEvents(Event.current);
                ProccessEvents(Event.current);

            }
  

            if (GUI.changed)
            {
                Repaint();
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene()); // Make Scene Dirty, show that it has to be saved
            }
        }

       

        private void ProcessNodeEvents(Event current)
        {
            if (nodes != null)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = nodes[i].ProcessEvents(current);

                    if (guiChanged)
                    {
                        GUI.changed = true;
                    }
                }
            }
        }

        private void ProccessEvents(Event current)
        {
            switch (current.type)
            {
                case EventType.MouseDown:
                    if (current.button == 1)
                    {
                        ProcessContextMenu(current.mousePosition);
                    }
                    break;
            }
        }

        //FOR_NEW: 01 Add new Node by ContextMenu
        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add start node"), false, () => OnClickAddNode(mousePosition, QuestNodeType.StartNode));
            genericMenu.AddItem(new GUIContent("Add Dialogue node"), false, () => OnClickAddNode(mousePosition, QuestNodeType.DialogueNode));
            genericMenu.AddItem(new GUIContent("Add Invnetory Requirement node"), false, () => OnClickAddNode(mousePosition, QuestNodeType.InventoryRequirementNode));
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
                    CreateConnection(selectedInPoint.Segment.Type);
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
                    CreateConnection(selectedOutPoint.Segment.Type);
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

        private void CreateConnection( SegmentType segmentType)
        {

            connections.Add(new NodeConnection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));

            selectedInPoint.Segment.Node.AddChildsToData(selectedOutPoint.Segment.Node, segmentType);
        }

        private void ClearConnectionSelection()
        {
            selectedInPoint = null;
            selectedOutPoint = null;
        }

        #endregion

    }
}
