using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHandler : MonoBehaviour
{
    private bool isShooting = false;
    private Vector3 direction;
    private float velocity = 0;
    public float aliveTime = 7;
    private float aliveTimeSum = 0;

    public float damage { get; private set; } = 25;

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
            transform.position += direction * velocity * Time.deltaTime;   
        }

        aliveTimeSum += Time.deltaTime;
        if(aliveTimeSum >= aliveTime)
        {
            Destroy(gameObject);
        }
    }

    public void ShotAt(Transform goalTransform, float arrowVelocity)
    {
        transform.LookAt(goalTransform.position);
        direction = goalTransform.position - this.transform.position;
        velocity = arrowVelocity;
        isShooting = true;

    }
}
