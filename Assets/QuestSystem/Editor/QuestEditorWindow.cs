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

        private void SetupEditor(Quest quest)
        {
            nodes.Clear();
            connections.Clear();
            selectedInPoint = null;
            selectedOutPoint = null;

            currentQuest = quest;

            foreach (var node in currentQuest.Nodes) // Add nodes with nodeData
            {

                if(node is QuestStartNodeData)
                {
                    nodes.Add(new StartNode( OnClickNodePort, node));
                }    
            }

            foreach (Node _node in nodes)
            {
                nodeLookUp[_node.Questdata.UID] = _node;
            }


            foreach (var node in nodes) // Add connections 
            {

                foreach (var childId in node.Questdata.ChildrenIDs)
                { 
                    if (nodeLookUp.ContainsKey(childId))
                    {
                        foreach (var segment in node.Segments.Where(s => s.Key == SegmentType.MainSegment))
                        {
                            foreach (var childSegment in nodeLookUp[childId].Segments.Where(s => s.Key == SegmentType.MainSegment))
                            {
                                NodePort outPort = segment.Value.NodePortsDict[ConnectionPointType.Out];
                                NodePort inPort = childSegment.Value.NodePortsDict[ConnectionPointType.In];
                                connections.Add(new NodeConnection(inPort, outPort, OnClickRemoveConnection));
                            }

                        }

                    }

                }
            }
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

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add start node"), false, () => OnClickAddNode(mousePosition, QuestNodeType.StartNode));
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
                Handles.DrawBezier(
                    selectedInPoint.Rect.center,
                    current.mousePosition,
                    selectedInPoint.Rect.center + Vector2.left * 50f,
                    current.mousePosition - Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }

            if (selectedOutPoint != null && selectedInPoint == null)
            {
                Handles.DrawBezier(
                    selectedOutPoint.Rect.center,
                    current.mousePosition,
                    selectedOutPoint.Rect.center - Vector2.left * 50f,
                    current.mousePosition + Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }
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

        private void OnClickNodePort(NodePort port, ConnectionPointType type)
        {
            switch (type)
            {
                case ConnectionPointType.In:
                    OnClickInPoint(port);
                    break;
                case ConnectionPointType.Out:
                    OnClickOutPoint(port);
                    break;
                default:
                    break;
            }
        }

        private void OnClickInPoint(NodePort inPoint)
        {
            selectedInPoint = inPoint;

            if (selectedOutPoint != null)
            {
                if (selectedOutPoint.Segment != selectedInPoint.Segment)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickOutPoint(NodePort outPoint)
        {
            selectedOutPoint = outPoint;

            if (selectedInPoint != null)
            {
                if (selectedOutPoint.Segment != selectedInPoint.Segment)
                {
                    CreateConnection();
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

        private void CreateConnection()
        {
            if (connections == null)
            {
                connections = new List<NodeConnection>();
            }

            connections.Add(new NodeConnection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
            selectedOutPoint.Segment.Node.Questdata.ChildrenIDs.Add(selectedInPoint.Segment.Node.Questdata.UID);//ToDo: Vershönern
        }

        private void ClearConnectionSelection()
        {
            selectedInPoint = null;
            selectedOutPoint = null;
        }

        #endregion

    }
}
