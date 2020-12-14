using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    private bool isShooting = false;
    public bool disabledDamage = false;
    private Vector3 direction;
    public float velocity = 1;
    public float aliveTime = 7;
    private float aliveTimeSum = 0;
    public float damage = 25;
    public OnHitEffect onHitEffect = OnHitEffect.None;
    public float onHitEffectTime = 1f;

    // Update is called once per frame
    void Update()
    {
        if (isShooting)
        {
            transform.position += direction.normalized * velocity * Time.deltaTime;   
        }

        aliveTimeSum += Time.deltaTime;
        if(aliveTimeSum >= aliveTime)
        {
            Destroy(gameObject);
        }
    }

    public void ShotAt(Vector3 position, float? projectilVelocity = null)
    {
        transform.LookAt(position);
        direction = position - this.transform.position;
        if (projectilVelocity.HasValue)
        {
            velocity = (float)projectilVelocity;
        }  
        isShooting = true;

    }

    public void StopShoot()
    {
        isShooting = false;
    }

  
}

public enum OnHitEffect
{
    None, Burn
}