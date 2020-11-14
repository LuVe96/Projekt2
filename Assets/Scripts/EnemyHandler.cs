using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyHandler : MonoBehaviour
{
    public EnemyType enemyType = EnemyType.Magician;
    public float lifeAmount = 100;
    private float FullLifeAmount;
    public GameObject projectilePrfab;
    public GameObject bloodParticles;
    public AudioSource hitSound;
    public float attackDistance = 15;
    public float attackPause = 1.5f;
    private float attackPeriodeSum = 0; 

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

        if(Vector3.Distance(transform.position, player.position) <= attackDistance)
        {
            // rotate and move towards player
            GetComponent<NavMeshAgent>().SetDestination(player.transform.position);

            attackPeriodeSum += Time.deltaTime;
            if (attackPeriodeSum >= attackPause)
            {
                attackPeriodeSum = 0;

                AttackPlayer();

            }     
        }
        else
        {
            attackPeriodeSum = 0;
        }

        //On Death
        if (lifeAmount <= 0)
        {
            //GameObject.Find("EnemyDetection").GetComponent<EnemyDetector>().RemoveFromEnemyList(transform.Find("Charakter").gameObject);
            GameObject.Find("EnemyDetection").GetComponent<EnemyDetector>().RemoveFromEnemyList(gameObject);
            Destroy(gameObject);
        }
    }

    private bool LookAtPlayer()
    {
        //var rotation = Quaternion.LookRotation( transform.position - player.transform.position);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, 1f);

        // when rotation ended
        //if (rotation == transform.rotation)
        //{
        //    return true;
        //}
        return true;
    }

    private void AttackPlayer()
    {
        if (enemyType == EnemyType.Magician)
        {
            GameObject projectile = Instantiate(projectilePrfab);
            projectile.transform.position = transform.forward + transform.localPosition;
            projectile.GetComponent<ProjectileHandler>().ShotAt(player.transform.position + new Vector3(0, UnityEngine.Random.Range(-0.5f, 0.5f), 0));
        } else if (enemyType == EnemyType.Dog)
        {
            GetComponent<DogAttackHandler>().Attack();
        }
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Arrow" && !collision.gameObject.GetComponent<ProjectileHandler>().disabledDamage)
        {
            collision.gameObject.GetComponent<ProjectileHandler>().disabledDamage = true;
            lifeAmount -= collision.gameObject.GetComponent<ProjectileHandler>().damage;
            transform.Find("Canvas/lifebar/front").gameObject.GetComponent<Image>().fillAmount = lifeAmount / FullLifeAmount;
            Instantiate(bloodParticles, transform.position, transform.rotation);
            hitSound.Play();
        }
    }
}

public enum EnemyType
{
    Dog,
    Magician
}
