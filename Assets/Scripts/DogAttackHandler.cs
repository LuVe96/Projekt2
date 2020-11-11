﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DogAttackHandler : MonoBehaviour
{
   
    public float attackTime = 1f;
    public float attackSpeed = 1f;
    public float attackAcceleration = 8f;
    public float damage = 30f;

    private float attackTimeSum = 0;
    private bool isAttacking = false;

    private NavMeshAgent navAgent;
    private float stdSpeed;
    private float stdAcceleration;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        stdSpeed = navAgent.speed;
        stdAcceleration = navAgent.acceleration;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            navAgent.speed = attackSpeed;
            navAgent.acceleration = attackAcceleration;

            attackTimeSum += Time.deltaTime;
            if( attackTimeSum >= attackTime)
            {
                navAgent.speed = stdSpeed;
                navAgent.acceleration = stdAcceleration;
                attackTimeSum = 0;
                isAttacking = false;
            }

        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            attackTimeSum = 0;
            isAttacking = false;

        }
    }

    public void Attack()
    {
        isAttacking = true;
    }
}
