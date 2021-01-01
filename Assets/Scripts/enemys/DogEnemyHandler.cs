using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogEnemyHandler : EnemyHandler
{
   
    public float attackTime = 1f;
    public float attackSpeed = 1f;
    public float attackAcceleration = 8f;
    public float damage = 30f;
    public float stopTimeAfterHit = 10f;

    [HideInInspector]
    public bool hasHitten = false;

    private float attackTimeSum = 0;
    private float stopTimeAfterHitSum = 0;
    private bool isAttacking = false;

    private float stdSpeed;
    private float stdAcceleration;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        stdSpeed = navMeshAgent.speed;
        stdAcceleration = navMeshAgent.acceleration;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (isAttacking && !hasHitten)
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("WalkingSpeedMultiplier", 2f);
            navMeshAgent.speed = attackSpeed;
            navMeshAgent.acceleration = attackAcceleration;

            attackTimeSum += Time.deltaTime;
            if( attackTimeSum >= attackTime)
            {
                animator.SetFloat("WalkingSpeedMultiplier", 1f);
                navMeshAgent.speed = stdSpeed;
                navMeshAgent.acceleration = stdAcceleration;
                attackTimeSum = 0;
                isAttacking = false;
            }
        }

        if (hasHitten)
        {
            navMeshAgent.speed = 0;
            transform.Find("AttackCollider").gameObject.SetActive(false);
            animator.SetBool("isWalking", false);

            stopTimeAfterHitSum += Time.deltaTime;

            if (stopTimeAfterHitSum >= stopTimeAfterHit)
            {
                navMeshAgent.speed = stdSpeed;
                navMeshAgent.acceleration = stdAcceleration;
                stopTimeAfterHitSum = 0;
                hasHitten = false;
                attackTimeSum = 0;
                isAttacking = false;
                transform.Find("AttackCollider").gameObject.SetActive(true);
            }

        }
    }

    protected override void AttackPlayer()
    {
        isAttacking = true;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if(collision.gameObject.tag == "Player")
        {
            attackTimeSum = 0;
            isAttacking = false;
        }
    }



}
