using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class camerarotator : MonoBehaviour
{

    public float rot_x;
    public float rot_z;
    public int  devider;

    private Vector3 startPos;
    private float TimeSum = 0;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {


            transform.position += new Vector3(rot_x/ devider, 0, 0); 

            if(transform.position.x >= startPos.x + Mathf.Abs(rot_x) || transform.position.x <= startPos.x - Mathf.Abs(rot_x))
            {
                rot_x = -rot_x;
            }

            //if (transform.position.z >= startPos.z + Mathf.Abs(rot_z) || transform.position.z <= startPos.z - Mathf.Abs(rot_z))
            //{
            //    rot_z = -rot_z;
            //}
        
    }
}
