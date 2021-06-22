using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace QuestSystem.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        static Dialogue selectedDialogue = null;
        [NonSerialized] // not saved in Unity
        GUIStyle nodeStyle = new GUIStyle();
        [NonSerialized]
        GUIStyle playerNodeStyle = new GUIStyle();
        [NonSerialized]
        DialogueNode draggingNode = null;
        [NonSerialized]
        Vector2 draggingOffset;
        [NonSerialized]
        DialogueNode creatingNode = null;
        [NonSerialized]
        DialogueNode deletingNode = null;
        [NonSerialized]
        DialogueNode linkingParentNode = null;
        Vector2 scrollPosition;
        [NonSerialized]
        bool draggingCanvas = false;
        [NonSerialized]
        Vector2 draggingCanvasOffset;

        public Vector2 canvasSize = new Vector2(4000, 4000);

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");           
        }

        [OnOpenAsset(1)]
        public static bool OnOpenDialogueAsset(int instanceId, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceId) as Dialogue;
            if( dialogue != null)
            {
                selectedDialogue = dialogue;
                ShowEditorWindow();
                return true;
            }
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            SetupNodeStyle(nodeStyle, "node0");
            SetupNodeStyle(playerNodeStyle, "node1");
        }

        private void SetupNodeStyle(GUIStyle gUIStyle, string bg)
        {
            //gUIStyle = new GUIStyle();
            gUIStyle.normal.background = EditorGUIUtility.Load(bg) as Texture2D;
            gUIStyle.padding = new RectOffset(20, 20, 20, 20);
            gUIStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnSelectionChanged()
        {
            Dialogue dialogue = Selection.activeObject as Dialogue;
            if( dialogue != null)
            {
                selectedDialogue = dialogue;
                Repaint();
            }
        }
        string txyt = "";
        private void OnGUI()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue is selected");
            }
            else
            {
                EditorGUILayout.LabelField(selectedDialogue.name);              

                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                Rect canvas = GUILayoutUtility.GetRect(canvasSize.x, canvasSize.y);
                Texture2D texture = Resources.Load("background") as Texture2D;
                Rect textCoords = new Rect(0, 0, canvasSize.x / texture.width, canvasSize.y / texture.height);
                GUI.DrawTextureWithTexCoords(canvas, texture, textCoords );


                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    OnGuiNode(node);
                }
                EditorGUILayout.EndScrollView();

                if (creatingNode != null)
                {
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }
                if (deletingNode != null)
                {
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
                ProcessEvent();

            }   
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.Rect.xMax, node.Rect.center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(childNode.Rect.xMin, childNode.Rect.center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(startPosition, endPosition, startPosition + controlPointOffset, endPosition - controlPointOffset, Color.white, null, 4f);
            }
        }

        private void ProcessEvent()
        {
            if(Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if( draggingNode != null)
                {
                    draggingOffset = draggingNode.Rect.position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;  // to select Node and show in Inspector
                } else
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {

                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;

            }
            else if( Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }

        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode returnNode = null;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.Rect.Contains(point))
                {
                    if (!node.GetTextFieldRectWithOffset().Contains(point))
                    {
                        returnNode = node;
                    }
            
                }
            }
            return returnNode;
        }

        private void OnGuiNode(DialogueNode node)
        {
            GUIStyle style = node.IsPlayerSpeaking ? playerNodeStyle : nodeStyle;
            GUILayout.BeginArea(node.Rect, style);
            //EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("Node:", EditorStyles.whiteLabel);
            GUIStyle textFieldStyle = EditorStyles.textArea;
            textFieldStyle.wordWrap = true;

            node.Text = EditorGUILayout.TextArea(node.Text, textFieldStyle,  GUILayout.Height(50));

            //node.TextFieldRect = GUILayoutUtility.GetLastRect();

            //if (EditorGUI.EndChangeCheck())  // is true, when there are changes
            //{
            //}
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Delete"))
            {
                deletingNode = node;
            }
            if (!node.IsEndPoint)
            {
                DrawLinkButton(node);

                if (GUILayout.Button("Add"))
                {
                    creatingNode = node;
                }
            }
          
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawLinkButton(DialogueNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    linkingParentNode = node;
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("Cancel"))
                {
                    linkingParentNode = null;
                }
            }
            else if (linkingParentNode.Children.Contains(node.UniqueID))
            {
                if (GUILayout.Button("Unlink"))
                {

                    linkingParentNode.RemoveChild(node.UniqueID);
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("Child"))
                {
                    linkingParentNode.AddChild(node.UniqueID);
                    linkingParentNode = null;
                }
            }
        }
    }
}

