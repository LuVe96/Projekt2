using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;

namespace QuestSystem.Quest
{
    public class QuestEditorWindow : EditorWindow
    {
        static Quest currentQuest = null;

        private List<Node> nodes = new List<Node>();
        private List<NodeConnection> connections = new List<NodeConnection>();
        Dictionary<string, Node> nodeLookUp = new Dictionary<string, Node>();

        private GUIStyle nodeStyle;

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

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
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
                    nodes.Add(new StartNode(nodeStyle, OnClickInPoint, OnClickOutPoint, node));
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
                        connections.Add(new NodeConnection(node.InPort, nodeLookUp[childId].OutPort, OnClickRemoveConnection));
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
            }

            DrawNodes();
            DrawConnections();

            DrawConnectionLine(Event.current);

            ProcessNodeEvents(Event.current);
            ProccessEvents(Event.current);

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
                    nodes.Add(new StartNode(mousePosition, 200, 100, nodeStyle, OnClickInPoint, OnClickOutPoint, questdate));
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

        private void OnClickInPoint(NodePort inPoint)
        {
            selectedInPoint = inPoint;

            if (selectedOutPoint != null)
            {
                if (selectedOutPoint.Node != selectedInPoint.Node)
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
                if (selectedOutPoint.Node != selectedInPoint.Node)
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
            selectedInPoint.Node.Questdata.ChildrenIDs.Add(selectedOutPoint.Node.Questdata.UID);//ToDo: Vershönern
        }

        private void ClearConnectionSelection()
        {
            selectedInPoint = null;
            selectedOutPoint = null;
        }

        #endregion

    }
}
