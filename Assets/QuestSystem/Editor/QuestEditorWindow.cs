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

        private NodePort selectedInPoint = null;
        private NodePort selectedOutPoint;
        Vector2 scrollPosition;
        [NonSerialized]
        bool draggingCanvas = false;
        [NonSerialized]
        Vector2 draggingCanvasOffset;

        private Vector2 canvasSize = new Vector2(4000, 2000);
        private QuestVariableTemplate newQuestVaraible = null;
        private Vector2 variableAreaOffset = new Vector2(0,0);

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
            try
            {
                if (Selection.activeGameObject.GetComponent<Quest>() == null) return;
            }
            catch (Exception)
            {
                return;
            }
    
            Quest quest = Selection.activeGameObject.GetComponent<Quest>() as Quest;
            if (quest != null)
            {
                SetupEditor(quest);
                Repaint();
                Repaint();
            }
        }

        private void RepaintEditor(bool resetup)
        {
            if (resetup)
            {
                SetupEditor(currentQuest);
            }
            Repaint();
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
                        nodes.Add(new StartNode(OnClickNodePort, n, RepaintEditor));
                        break;
                    case QuestDialogueNodeData n:
                        nodes.Add(new DialogueNode(OnClickNodePort, n, RepaintEditor));
                        break;
                    case InventoryRequireData n:
                        nodes.Add(new InventoryRequirementNode(OnClickNodePort, n, RepaintEditor)); break;
                    case InventoryActionData n:
                        nodes.Add(new InventoryActionNode(OnClickNodePort, n, RepaintEditor)); break;
                    case EnableActionData n:
                        nodes.Add(new EnableActionNode(OnClickNodePort, n, RepaintEditor)); break;
                    case StandartNodeData n:
                        nodes.Add(new StandartNode(OnClickNodePort, n, RepaintEditor)); break;
                    case QuestEndNodeData n:
                        nodes.Add(new QuestEndNode(OnClickNodePort, n, RepaintEditor)); break;
                    case VariableActionData n:
                        nodes.Add(new VariableActionNode(OnClickNodePort, n, RepaintEditor)); break;
                    case VariableRequireData n:
                        nodes.Add(new VariableRequireNode(OnClickNodePort, n, RepaintEditor)); break;
                    case NoteNodeData n:
                        nodes.Add(new NoteNode(OnClickNodePort, n, RepaintEditor)); break;
                    case BranchNodeData n:
                        nodes.Add(new BranchNode(OnClickNodePort, n, RepaintEditor)); break;
                    case TriggerRequirementNodeData n:
                        nodes.Add(new TriggerRequirementNode(OnClickNodePort, n, RepaintEditor)); break;
                    case EventActionData n:
                        nodes.Add(new EventActionNode(OnClickNodePort, n, RepaintEditor)); break;
                    case PositionActionData n:
                        nodes.Add(new PositionActionNode(OnClickNodePort, n, RepaintEditor)); break;
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
                    foreach (EndPointContainer container in (node.Questdata as QuestDialogueNodeData).EndPointContainer)
                    {
                        CreateMatchingConnections(node, container.endPointChilds, SegmentType.DialogueEndPointSegment, SegmentType.MainSegment, container.id);
                    }
                }
                if (node.Questdata is BranchNodeData)
                {
                    BranchNodeData data = node.Questdata as BranchNodeData;
                    CreateMatchingConnections(node, data.TrueEndPoint.endPointChilds, SegmentType.DialogueEndPointSegment, SegmentType.MainSegment, data.TrueEndPoint.id);
                    CreateMatchingConnections(node, data.FalseEndPoint.endPointChilds, SegmentType.DialogueEndPointSegment, SegmentType.MainSegment, data.FalseEndPoint.id);
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
            DrawVariableArea();
            if (currentQuest == null)
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

        private void DrawVariableArea()
        {
            float height = EditorGUILayout.BeginVertical().height;
            GUILayout.BeginHorizontal(GUILayout.Width(100));
            EditorGUILayout.LabelField("Quest Variables:");
            if(newQuestVaraible == null)
            {
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    newQuestVaraible = new QuestVariableTemplate(); ;
                }
            }
            else
            {
                if (GUILayout.Button("x", GUILayout.Width(30)))
                {
                    newQuestVaraible = null;
                }
            }
            GUILayout.EndHorizontal();

            if (newQuestVaraible != null)
            {
                GUILayout.BeginHorizontal(GUILayout.Width(300));
                newQuestVaraible.Title = EditorGUILayout.TextField(newQuestVaraible.Title);
                newQuestVaraible.Type = (QuestVariableType)EditorGUILayout.EnumPopup(newQuestVaraible.Type);
                if (GUILayout.Button("✓", GUILayout.Width(30)))
                {
                    QuestVariableObject qvo = Resources.Load("QuestVariables") as QuestVariableObject;
                    qvo.AddQuestVarialbe(newQuestVaraible);
                    EditorUtility.SetDirty(qvo);
                    newQuestVaraible = null;
                }
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            if(height != 0)
            {
                variableAreaOffset.y = height * -1;
            }

        }

        private Node GetNodeAtPoint(Vector2 point)
        {
            foreach (Node node in nodes)
            {
                if (node.Rect.Contains(point))
                {
                    return node;
                }
            }
            return null;
        }

        private void ProcessNodeEvents(Event current)
        {
            if (nodes != null)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = nodes[i].ProcessEvents(current, scrollPosition + variableAreaOffset);

                    if (guiChanged)
                    {
                        GUI.changed = true;
                    }
                }
            }
        }

        Vector2 curPos = new Vector2(100,100);
        float zoomScale = 1;


        private void ProccessEvents(Event current)
        {

            if (current.type == EventType.MouseDown)
            {
                // Open PopupMenu
                if (current.button == 1)
                {
                    Node node = GetNodeAtPoint(current.mousePosition + scrollPosition + variableAreaOffset);
                    if (node)
                    {
                        GenericMenu genericMenu = new GenericMenu();
                        genericMenu.AddItem(new GUIContent("Delete node"), false, () =>
                        {
                            currentQuest.DeleteNode(node.Questdata);
                            RepaintEditor(true);

                        });
                        genericMenu.ShowAsContext();
                    }
                    else
                    {
                        ProcessContextMenu(current.mousePosition + scrollPosition);
                    }

                }
               
                if (current.button == 0 && (GetNodeAtPoint(current.mousePosition + scrollPosition + variableAreaOffset) == null))
                {
                    ClearConnectionSelection();
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition + variableAreaOffset;
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
            genericMenu.AddItem(new GUIContent("Main Nodes / Standart"), false, () => OnClickAddNode(mousePosition, QuestNodeType.StandartNode));
            genericMenu.AddItem(new GUIContent("Main Nodes / End"), false, () => OnClickAddNode(mousePosition, QuestNodeType.EndNode));
            genericMenu.AddItem(new GUIContent("Main Nodes / Dialogue"), false, () => OnClickAddNode(mousePosition, QuestNodeType.DialogueNode));

            genericMenu.AddItem(new GUIContent("Requirement Nodes / Invnetory Requirement"), false, () => OnClickAddNode(mousePosition, QuestNodeType.InventoryRequirementNode));
            genericMenu.AddItem(new GUIContent("Requirement Nodes / Varaible Requirement"), false, () => OnClickAddNode(mousePosition, QuestNodeType.VariableRequirementNode));
            genericMenu.AddItem(new GUIContent("Requirement Nodes / Trigger Requirement"), false, () => OnClickAddNode(mousePosition, QuestNodeType.TriggerRequirementNode));

            genericMenu.AddItem(new GUIContent("Action Nodes / Invnetory Action"), false, () => OnClickAddNode(mousePosition, QuestNodeType.InventoryActionNode));
            genericMenu.AddItem(new GUIContent("Action Nodes / Enable Action"), false, () => OnClickAddNode(mousePosition, QuestNodeType.EnableActionNode));
            genericMenu.AddItem(new GUIContent("Action Nodes / Varaible Action"), false, () => OnClickAddNode(mousePosition, QuestNodeType.VariableActionNode));
            genericMenu.AddItem(new GUIContent("Action Nodes / Event Action"), false, () => OnClickAddNode(mousePosition, QuestNodeType.EventActionNode));
            genericMenu.AddItem(new GUIContent("Action Nodes / Postion Action"), false, () => OnClickAddNode(mousePosition, QuestNodeType.PostionActionNode));

            genericMenu.AddItem(new GUIContent("Other / Note"), false, () => OnClickAddNode(mousePosition, QuestNodeType.NoteNode));
            genericMenu.AddItem(new GUIContent("Other / Branch"), false, () => OnClickAddNode(mousePosition, QuestNodeType.BranchNode));
            genericMenu.ShowAsContext();
        }

     
 
        private void OnClickAddNode(Vector2 mousePosition, QuestNodeType type)
        {
            QuestNodeData questdate = currentQuest.CreateNewNode(type);
            switch (type)
            {
                case QuestNodeType.StartNode:
                    nodes.Add(new StartNode(mousePosition, 200, 100, OnClickNodePort, questdate, RepaintEditor));
                    break;
                case QuestNodeType.DialogueNode:
                    nodes.Add(new DialogueNode(mousePosition, 200, 100, OnClickNodePort, questdate, RepaintEditor));
                    break;
                case QuestNodeType.InventoryRequirementNode:
                    nodes.Add(new InventoryRequirementNode(mousePosition, 200, 100, OnClickNodePort, questdate, RepaintEditor));
                    break;
                case QuestNodeType.InventoryActionNode:
                    nodes.Add(new InventoryActionNode(mousePosition, 200, 100, OnClickNodePort, questdate, RepaintEditor));
                    break;
                case QuestNodeType.EnableActionNode:
                    nodes.Add(new EnableActionNode(mousePosition, 200, 100, OnClickNodePort, questdate, RepaintEditor));
                    break;
                case QuestNodeType.StandartNode:
                    nodes.Add(new StandartNode(mousePosition, 200, 100, OnClickNodePort, questdate, RepaintEditor));
                    break;
                case QuestNodeType.EndNode:
                    nodes.Add(new QuestEndNode(mousePosition, 200, 100, OnClickNodePort, questdate, RepaintEditor));
                    break; ;
                case QuestNodeType.VariableRequirementNode:
                    nodes.Add(new VariableRequireNode(mousePosition, 200, 100, OnClickNodePort, questdate, RepaintEditor));
                    break;
                case QuestNodeType.VariableActionNode:
                    nodes.Add(new VariableActionNode(mousePosition, 200, 100, OnClickNodePort, questdate, RepaintEditor));
                    break;
                case QuestNodeType.NoteNode:
                    nodes.Add(new NoteNode(mousePosition, 200, 100, OnClickNodePort, questdate, RepaintEditor));
                    break;
                case QuestNodeType.BranchNode:
                    nodes.Add(new BranchNode(mousePosition, 200, 100, OnClickNodePort, questdate, RepaintEditor));
                    break;
                case QuestNodeType.TriggerRequirementNode:
                    nodes.Add(new TriggerRequirementNode(mousePosition, 200, 100, OnClickNodePort, questdate, RepaintEditor));
                    break;
                case QuestNodeType.EventActionNode:
                    nodes.Add(new EventActionNode(mousePosition, 200, 100, OnClickNodePort, questdate, RepaintEditor));
                    break;
                case QuestNodeType.PostionActionNode:
                    nodes.Add(new PositionActionNode(mousePosition, 200, 100, OnClickNodePort, questdate, RepaintEditor));
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
            Color color = selectedPort.Segment.LineColor;

            Handles.DrawBezier(
                selectedPort.Rect.center,
                current.mousePosition,
                selectedPort.Rect.center + Vector2.left * 50f * multiplier,
                current.mousePosition - Vector2.left * 50f * multiplier,
                color,
                null,
                3f
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

    struct ContextMenuItem
    {
        public string text;
        public QuestNodeType type;

        public ContextMenuItem(string text, QuestNodeType type)
        {
            this.text = text;
            this.type = type;
        }
    }
}
