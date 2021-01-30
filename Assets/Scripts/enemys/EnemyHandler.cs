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
    public float detectDistance = 20;
    public float attackDistance = 15;
    public float attackPause = 1.5f;
    private float attackPeriodeSum = 0;

    public OnHitEffectType weakness = OnHitEffectType.None;
    public float weaknessMultiplier = 1.5f;

    protected Transform player;
    private EnemyIndicator enemyIndicator;
    private Vector3 startPosition;
    private Image uiLifeBarFront;

    protected NavMeshAgent navMeshAgent;
    protected bool freezed = false;
    public Transform projectileSpawnPos;

    public ParticleSystem burnParticle;
    public ParticleSystem freezeParticle;
    public ParticleSystem poisonParticle;

    public Animator animator;
    private GameObject currentMagicWave;

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
        if (distance <= detectDistance)
        {

            if (!HitsAntiAttackCollider(distance))
            {
                //turn on enemy indicator
                enemyIndicator.setIndicator(true);

                // rotate and move towards player
                navMeshAgent.SetDestination(player.transform.position);

                // walkanimation when moving
                if (navMeshAgent.speed != 0) animator.SetBool("isWalking", true);

                if (distance <= attackDistance)
                {
                    attackPeriodeSum += Time.deltaTime;
                    if (attackPeriodeSum >= attackPause && !freezed /*&& OnSameLevel(player.transform, transform)*/)
                    {
                        attackPeriodeSum = 0;

                        AttackPlayer();

                    }
                }


            }
         
        }
        else
        {
            attackPeriodeSum = 0;
            navMeshAgent.SetDestination(startPosition);

            //turn off enemy indicator
            enemyIndicator.setIndicator(false);


            if(EqualizePositions(transform.position, startPosition, 0.2f) || (transform.position == startPosition))
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

    private bool OnSameLevel(Transform player, Transform enemy, float toleranz = 0.5f)
    {
        return (player.position.y + toleranz / 2 >= enemy.position.y) && (player.position.y - toleranz / 2 <= enemy.position.y);
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

    protected virtual void OnTriggerEnter(Collider other)
    {

        if(other.tag == "MagicWave" && currentMagicWave != other.transform.parent.gameObject)
        {
            currentMagicWave = other.transform.parent.gameObject;       // save currentMagicWave to avoid multiple trigger -> one wave can only trigger once
            float currentWeaknessMultiplier = 1;
            var wave = currentMagicWave.GetComponent<WaveMagicHandler>().magicWaveItem;

            if (weakness == wave.onHitEffectType)
            {
                currentWeaknessMultiplier = weaknessMultiplier;
            }

            switch (wave.onHitEffectType)
            {
                case OnHitEffectType.Burn:
                    StartCoroutine(EnableEffect(new OnHitEffect(wave.onHitEffectType, wave.effectTime, wave.damageOverTime)
                        , burnParticle, currentWeaknessMultiplier));
                    break;
                case OnHitEffectType.Freeze:
                    StartCoroutine(EnableEffect(new OnHitEffect(wave.onHitEffectType, wave.effectTime, wave.damageOverTime)
                        , freezeParticle, currentWeaknessMultiplier));
                    break;
                case OnHitEffectType.Poison:
                    StartCoroutine(EnableEffect(new OnHitEffect(wave.onHitEffectType, wave.effectTime, wave.damageOverTime)
                        , poisonParticle, currentWeaknessMultiplier));
                    break;
                default: break;

            }

        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {

        if (collision.transform.tag == "Arrow" && !collision.gameObject.GetComponent<ProjectileHandler>().disabledDamage)
        {
            // reset weakness multiplier on every new arrow hit
            float currentWeaknessMultiplier = 1;

            foreach (var effect in HitEffectManager.Instance.currentHitEffects)
            {
                if(weakness == effect.onHitEffectType)
                {
                    currentWeaknessMultiplier = weaknessMultiplier;
                }

                switch (effect.onHitEffectType)
                {
                    case OnHitEffectType.Burn:
                        StartCoroutine( EnableEffect(effect, burnParticle, currentWeaknessMultiplier));
                        break;
                    case OnHitEffectType.Freeze:
                        StartCoroutine(EnableEffect(effect, freezeParticle, currentWeaknessMultiplier));
                        break;
                    case OnHitEffectType.Poison:
                        StartCoroutine(EnableEffect(effect, poisonParticle, currentWeaknessMultiplier));
                        break;
                    default: break;
                }
            }

            collision.gameObject.GetComponent<ProjectileHandler>().disabledDamage = true;
            DamageEnemy(collision.gameObject.GetComponent<ProjectileHandler>().damage, currentWeaknessMultiplier);
        }
    }

    private void DamageEnemy(float amount, float currentWeaknessMultiplier, bool calledByEffect = false)
    {
        lifeAmount -= amount * currentWeaknessMultiplier;
        uiLifeBarFront.GetComponent<Image>().fillAmount = lifeAmount / MaxLifeAmount;
        if (!calledByEffect)
        {
            Instantiate(bloodParticles, transform.position, transform.rotation);
            hitSound.Play();
        }
    }

    IEnumerator EnableEffect(OnHitEffect effect, ParticleSystem particle, float currentWeaknessMultiplier)
    {
        particle.gameObject.SetActive(true);
        animator.SetFloat("WalkingSpeedMultiplier", currentWeaknessMultiplier);
        if(effect.onHitEffectType == OnHitEffectType.Freeze)
        {
            freezed = true;
        }
        float timeSum = 0;
        while(timeSum < effect.effectTime){
            timeSum += Time.deltaTime;
           
            float damage = effect.damageOverTime / effect.effectTime * Time.deltaTime;
            DamageEnemy(damage, currentWeaknessMultiplier, true);
            yield return null;
        }
        //yield return new WaitForSeconds(effect.effectTime);
        animator.SetFloat("WalkingSpeedMultiplier", 1f);
        freezed = false;
        particle.gameObject.SetActive(false);
    }

    bool EqualizePositions(Vector3 pos1, Vector3 pos2, float toleranz)
    {
        var tolHa = toleranz / 2;
        bool _x = pos2.x - tolHa <= pos1.x && pos1.x <= pos2.x + tolHa;
        //bool _y = pos2.y - tolHa <= pos1.y && pos1.y <= pos2.x + tolHa; // no Height
        bool _z = pos2.z - tolHa <= pos1.z && pos1.z <= pos2.z + tolHa;

        return _x && _z;
    }
}

public enum EnemyType
{
    Dog,
    Magician
}
