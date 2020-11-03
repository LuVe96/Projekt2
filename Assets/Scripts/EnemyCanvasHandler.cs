using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvasHandler : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.rotation = new Quaternion(50, 0, 0, transform.rotation.w);
    }
}
