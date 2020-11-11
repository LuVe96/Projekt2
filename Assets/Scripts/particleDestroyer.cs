using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleDestroyer : MonoBehaviour
{
    public float lifeTime = 5;

    public float timeSum = 0;
    // Update is called once per frame
    void Update()
    {
        timeSum += Time.deltaTime;
        if(lifeTime <= timeSum)
        {
            Destroy(gameObject);
        }
    }
}
