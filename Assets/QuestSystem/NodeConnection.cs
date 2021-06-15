using System.Collections;
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
            Rect leftPointRect = (outPoint.PortPosition == PortPosition.Left) ? outPoint.Rect : inPoint.Rect;
            Rect rightPointRect = (outPoint.PortPosition == PortPosition.Right) ? outPoint.Rect : inPoint.Rect;
            Handles.DrawBezier(
                leftPointRect.center,
                rightPointRect.center,
                leftPointRect.center + Vector2.left * 50f,
                rightPointRect.center - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );


            if(GUI.Button(new Rect((inPoint.Rect.center + outPoint.Rect.center) * 0.5f, new Vector2(15,15)), "x"))
            {

                inPoint.Segment.Node.RemoveChildsInData(outPoint.Segment.Node, inPoint.Segment);
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