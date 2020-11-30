using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootObjectHandler : MonoBehaviour
{

    public float rotationSpeed = 5;
    public LootItem item;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0), rotationSpeed * Time.deltaTime,  Space.World);
    }
}
