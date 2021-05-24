﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace QuestSystem
{
    [System.Serializable] 
    public class NodeConnection
    {
        NodePort inPoint;
        NodePort outPoint;
        Action<NodeConnection> OnClickRemoveConnection;

        public NodeConnection(NodePort inPoint, NodePort outPoint, Action<NodeConnection> OnClickRemoveConnection)
        {
            this.inPoint = inPoint;
            this.outPoint = outPoint;
            this.OnClickRemoveConnection = OnClickRemoveConnection;
        }

        public void Draw()
        {
            Handles.DrawBezier(
                inPoint.Rect.center, 
                outPoint.Rect.center,
                inPoint.Rect.center + Vector2.left * 50f,
                outPoint.Rect.center - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );


            if(GUI.Button(new Rect((inPoint.Rect.center + outPoint.Rect.center) * 0.5f, new Vector2(15,15)), "x"))
            {
                Debug.Log("Remove");
                outPoint.Segment.Node.Questdata.ChildrenIDs.Remove(inPoint.Segment.Node.Questdata.UID); // TODO: schönner!!!
                if (OnClickRemoveConnection != null)
                {
                    OnClickRemoveConnection(this);
                }
            }

            //if (Handles.Button((inPoint.Rect.center + outPoint.Rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.CircleHandleCap))
            //{
            //    Debug.Log("Remove");
            //    inPoint.Segment.Node.Questdata.ChildrenIDs.Remove(outPoint.Segment.Node.Questdata.UID); // TODO: schönner!!!
            //    if (OnClickRemoveConnection != null)
            //    {
            //        OnClickRemoveConnection(this);
            //    }
            //}
        }
    }

}