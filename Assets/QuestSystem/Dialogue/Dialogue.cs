using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuestSystem.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "QuestSystem/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
        [SerializeField] Vector2 newNodeOffset = new Vector2(250, 0);
        Dictionary<string, DialogueNode> nodeLookUp = new Dictionary<string, DialogueNode>();


        private void Awake()    //called on Creation or on Unity start
        {
            OnValidate();
        }

#if UNITY_EDITOR    // Awake is only called in Unity, not in Game

        public void CreateNode(DialogueNode parentNode)
        {

            DialogueNode newNode = CreateInstance<DialogueNode>();
            newNode.UniqueID = Guid.NewGuid().ToString();
            Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
            if (parentNode != null)
            {
                newNode.IsPlayerSpeaking = !parentNode.IsPlayerSpeaking;
                parentNode.AddChild(newNode.UniqueID);
                newNode.SetPosition(parentNode.Rect.position + newNodeOffset);
            }
            Undo.RecordObject(this, "Added Dialogue Node");
            nodes.Add(newNode);
            //AssetDatabase.AddObjectToAsset(newNode, this);
            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted Dialogue Node");
            nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private void CleanDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                node.RemoveChild(nodeToDelete.UniqueID);
            }
        }
#endif


        private void OnValidate() // called when changes in inspector
        {

            if (nodes.Count == 0)
            {
#if UNITY_EDITOR
                CreateNode(null);
#endif
            }
            nodeLookUp.Clear();
            foreach (DialogueNode node in GetAllNodes())
            {
                nodeLookUp[node.UniqueID] = node;
            }
        }
        

        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }

        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            List<DialogueNode> result = new List<DialogueNode>();

            foreach (string childId in parentNode.Children)
            {
                if(nodeLookUp.ContainsKey(childId))
                {
                    result.Add(nodeLookUp[childId]);
                } 
            }
            return result;
        }

        // Checked for Conditions/Variable
        public IEnumerable<DialogueNode> GetAllValideChildren(DialogueNode parentNode)
        {
            List<DialogueNode> result = new List<DialogueNode>();

            foreach (string childId in parentNode.Children)
            {
                if (nodeLookUp.ContainsKey(childId))
                {
                    DialogueNode n = nodeLookUp[childId];
                    if (n.IsUsingCondition)
                    {
                        // Check Condition
                        if (QuestSystemManager.Instance.getQuestVariableValue(n.SelectedVariable.Title) == n.RequiredVarialbeValue)
                        {
                            result.Add(n);
                        }
                    }
                    else
                    {
                        result.Add(n);
                    }


                }
            }
            return result;
        }

        public DialogueNode GetNodeById(string uID)
        {
            DialogueNode result = null;
            if (nodeLookUp.ContainsKey(uID))
            {
                result = nodeLookUp[uID];
            }        
            return result;
        }

        public DialogueEndPoint[] GetEndPoints()
        {
            List<DialogueEndPoint> result = new List<DialogueEndPoint>();
            foreach (DialogueNode node in nodes)
            {
                if (node.IsEndPoint)
                {
                    result.Add(new DialogueEndPoint(node.UniqueID, node.EndPointDescription));
                }
            }
            return result.ToArray();
        }


        public void OnBeforeSerialize() // called bevore saving
        {
#if UNITY_EDITOR
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach(DialogueNode node in GetAllNodes())
                {
                    if(AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize()
        { 
        }
    }
}
