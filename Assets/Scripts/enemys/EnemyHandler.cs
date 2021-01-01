using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class EnemyHandler : MonoBehaviour
{
    public float lifeAmount = 100;
    protected float MaxLifeAmount;
    public GameObject bloodParticles;
    public AudioSource hitSound;
    public float attackDistance = 15;
    public float attackPause = 1.5f;
    private float attackPeriodeSum = 0;

    protected Transform player;
    private EnemyIndicator enemyIndicator;
    private Vector3 startPosition;
    private Image uiLifeBarFront;

    public ParticleSystem burnParticle;
    protected NavMeshAgent navMeshAgent;

    public Animator animator;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.Find("Player").transform;
        MaxLifeAmount = lifeAmount;
        startPosition = transform.position;
        navMeshAgent = GetComponent<NavMeshAgent>();

        enemyIndicator = GetComponent<EnemyIndicator>();
        uiLifeBarFront = transform.Find("Canvas/lifebar/front").gameObject.GetComponent<Image>();

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackDistance)
        {

            if (!HitsAntiAttackCollider(distance))
            {
                //turn on enemy indicator
                enemyIndicator.setIndicator(true);

                // rotate and move towards player
                navMeshAgent.SetDestination(player.transform.position);

                // walkanimation when moving
                if (navMeshAgent.speed != 0) animator.SetBool("isWalking", true);

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
            navMeshAgent.SetDestination(startPosition);

            //turn off enemy indicator
            enemyIndicator.setIndicator(false);


            if(transform.position == startPosition)
            {
                animator.SetBool("isWalking", false);
            }
        }

        //On Death
        if (lifeAmount <= 0)
        {
            //GameObject.Find("EnemyDetection").GetComponent<EnemyDetector>().RemoveFromEnemyList(transform.Find("Charakter").gameObject);
            enemyIndicator.setIndicator(false);
            GameObject.Find("EnemyDetection").GetComponent<EnemyDetector>().RemoveFromEnemyList(gameObject);
            GetComponent<LootDropper>().DropLoot();
            Destroy(gameObject);
        }
    }


    private bool HitsAntiAttackCollider(float distance)
    {
        Ray ray = new Ray(transform.position, player.position - transform.position);
        RaycastHit[] hits = Physics.RaycastAll(ray, distance);

        foreach(RaycastHit hit in hits) {
            if(hit.collider.tag == "AntiAttackCollider")
            {
                return true;
            }
        }

        return false;
    }

    protected abstract void AttackPlayer();

    protected virtual void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.tag == "Arrow" && !collision.gameObject.GetComponent<ProjectileHandler>().disabledDamage)
        {
            collision.gameObject.GetComponent<ProjectileHandler>().disabledDamage = true;
            DamageEnemy(collision.gameObject.GetComponent<ProjectileHandler>().damage);
            Instantiate(bloodParticles, transform.position, transform.rotation);
            hitSound.Play();

            foreach (var effect in HitEffectManager.Instance.currentHitEffects)
            {
                switch (effect.onHitEffectType)
                {
                    case OnHitEffectType.Burn:
                        StartCoroutine( EnableEffect(effect, burnParticle));
                        break;
                    default: break;
                }
            }
        }
    }

    private void DamageEnemy(float amount, bool calledByEffect = false)
    {
        lifeAmount -= amount;
        uiLifeBarFront.GetComponent<Image>().fillAmount = lifeAmount / MaxLifeAmount;
        if (!calledByEffect)
        {
            Instantiate(bloodParticles, transform.position, transform.rotation);
            hitSound.Play();
        }

    }

    IEnumerator EnableEffect(OnHitEffect effect, ParticleSystem particle)
    {
        particle.gameObject.SetActive(true);
        float timeSum = 0;
        while(timeSum < effect.effectTime){
            timeSum += Time.deltaTime;
           
            float damage = effect.damageOverTime / effect.effectTime * Time.deltaTime;
            DamageEnemy(damage, true);
            yield return null;
        }
        //yield return new WaitForSeconds(effect.effectTime);
        particle.gameObject.SetActive(false);
    }
}

public enum EnemyType
{
    Dog,
    Magician
}
