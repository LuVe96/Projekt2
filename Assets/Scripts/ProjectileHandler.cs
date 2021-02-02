using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    public bool disabledDamage = false;
    public float velocity = 1;
    public float aliveTime = 7;
    public float damage = 25;
    public OnHitEffectType onHitEffect = OnHitEffectType.None;
    public float onHitEffectTime = 1f;
    public float damageOverTime = 1f;
    public float walkSpeedMultiplier = 1f;

    private Vector3 direction;
    private float aliveTimeSum = 0;
    private bool isShooting = false;
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (isShooting)
        //{
        //    transform.position += direction.normalized * velocity * Time.deltaTime;   
        //}

        aliveTimeSum += Time.deltaTime;
        if(aliveTimeSum >= aliveTime)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (isShooting)
        {
            //transform.position += direction.normalized * velocity * Time.deltaTime;
            _rigidbody.velocity = direction.normalized * velocity;
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

public enum OnHitEffectType
{
    None, Burn, Freeze, Poison
}