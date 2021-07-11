using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1;
    [SerializeField] bool reversedRotation = false;
    [SerializeField] RotationAxis axis = RotationAxis.X;

    void Update()
    {
        float dir = reversedRotation ? -1 : 1;
        transform.Rotate(axis.GetAxisVector(), rotationSpeed * Time.deltaTime * dir);
    }
}

public enum RotationAxis
{
    X,Y,Z
}

static class RotationAxisMethods
{

    public static Vector4 GetAxisVector(this RotationAxis s1)
    {
        switch (s1)
        {
            case RotationAxis.X:
                return new Vector4(1, 0, 0);
            case RotationAxis.Y:
                return new Vector4(0, 1, 0);
            case RotationAxis.Z:
                return new Vector4(0, 0, 1);
            default:
                return new Vector4(1, 0, 0);
        }
    }
}