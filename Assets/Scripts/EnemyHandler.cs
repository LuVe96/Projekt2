using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHandler : MonoBehaviour
{
    public EnemyType enemyType = EnemyType.Magician;
    public float lifeAmount = 100;
    private float FullLifeAmount;
    public GameObject projectilePrfab;
    public float attackDistance = 15;
    public float attackPause = 1.5f;
    public float attackPeriodeSum = 0; 

    private Transform player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        FullLifeAmount = lifeAmount;
    }

    // Update is called once per frame
    void Update()
    {

        if(lifeAmount <= 0)
        {
            //GameObject.Find("EnemyDetection").GetComponent<EnemyDetector>().RemoveFromEnemyList(transform.Find("Charakter").gameObject);
            GameObject.Find("EnemyDetection").GetComponent<EnemyDetector>().RemoveFromEnemyList(gameObject);
            Destroy(gameObject);
        }

        if(Vector3.Distance(transform.position, player.position) <= attackDistance)
        {
            if (LookAtPlayer())
            {
                attackPeriodeSum += Time.deltaTime;
                if (attackPeriodeSum >= attackPause)
                {
                    attackPeriodeSum = 0;

                    AttackPlayer();

                }
            }

               
        }
        else
        {
            attackPeriodeSum = 0;
        }
    }

    private bool LookAtPlayer()
    {
        var rotation = Quaternion.LookRotation( transform.position - player.transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 1f);

        // when rotation ended
        //if (rotation == transform.rotation)
        //{
        //    return true;
        //}
        return true;
    }

    private void AttackPlayer()
    {
        GameObject projectile = Instantiate(projectilePrfab);
        projectile.transform.position = transform.position;
        projectile.GetComponent<ProjectileHandler>().ShotAt(player.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Arrow")
        {
            lifeAmount -= collision.gameObject.GetComponent<ProjectileHandler>().damage;

            transform.Find("Canvas/lifebar/front").gameObject.GetComponent<Image>().fillAmount = lifeAmount / FullLifeAmount;
        }
    }
}

public enum EnemyType
{
    Dog,
    Magician
}
