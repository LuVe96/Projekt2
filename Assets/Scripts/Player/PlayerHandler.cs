using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    public float lifeAmount = 100;
    private float MaxLifeAmount;
    public GameObject bloodParticles;
    public AudioSource hitSound;
    public GameObject uiLifeBarFront;
    private PlayerCombatHandler combatHandler;
    private Animator animator;

    [HideInInspector]
    public bool isDieing;

    ///Efects
    public ParticleSystem burnEffect;
    public ParticleSystem freezeEffect;
    public ParticleSystem poisonEffect;


    // Start is called before the first frame update
    void Start()
    {
        MaxLifeAmount = lifeAmount;
        combatHandler = GetComponent<PlayerCombatHandler>();
        //uiLifeBar = GameObject.Find("IngameUICanvas").transform.Find("lifeBar/front").gameObject;
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (lifeAmount <= 0)
        {
            //GameManager.Instance.playerIsDead = true;
            StartCoroutine(Dieing());
        }

    }

    public void HealPlayer(float amount)
    {
        lifeAmount += amount;
        if(lifeAmount >= MaxLifeAmount)
        {
            lifeAmount = MaxLifeAmount;
        }
        uiLifeBarFront.GetComponent<Image>().fillAmount = lifeAmount / MaxLifeAmount;
    }

    private void PlayerAttacked(float amount, OnHitEffectType effect = OnHitEffectType.None, 
        float effectTime = 0, float damageOverTime = 0, float moveSpeedMultiplier = 1)
    {
        DamagePlayer(amount);

        switch (effect)
        {
            case OnHitEffectType.Burn:
                StartCoroutine(EnableEffect(burnEffect, effectTime, damageOverTime, moveSpeedMultiplier));
                break;
            case OnHitEffectType.Freeze:
                StartCoroutine(EnableEffect(freezeEffect, effectTime, damageOverTime, moveSpeedMultiplier, true));
                break;
            case OnHitEffectType.Poison:
                StartCoroutine(EnableEffect(poisonEffect, effectTime, damageOverTime, moveSpeedMultiplier));
                break;
            default: break;
        }
       
    }

    private void DamagePlayer(float amount, bool calledByEffect = false)
    {
        lifeAmount -= amount;
        uiLifeBarFront.GetComponent<Image>().fillAmount = lifeAmount / MaxLifeAmount;
        if (!calledByEffect)
        {
            Instantiate(bloodParticles, transform.position, transform.rotation);
            hitSound.Play();
        }

    }

    IEnumerator Dieing()
    {
        animator.SetBool("isDieing", true);
        isDieing = true;
        GetComponent<PlayerCombatHandler>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;

        yield return new WaitForSeconds(6f);

        GameManager.Instance.playerIsDead = true;


    }

    IEnumerator EnableEffect(ParticleSystem particle, float time, float damageOverTime , float moveSpeedMultipliyer, bool freeze = false)
    {
        particle.gameObject.SetActive(true);
        GetComponent<PlayerMovement>().setMovementSpeed(moveSpeedMultipliyer);
        if (freeze)
        {
            combatHandler.freezed = true;
        }


        float timeSum = 0;

        while(timeSum <= time)
        {
            timeSum += Time.deltaTime;
            float damage = damageOverTime / time * Time.deltaTime;
            DamagePlayer(damage, true);
            yield return null;

        }

        GetComponent<PlayerMovement>().setMovementSpeed(1);
        if (freeze)
        {
            combatHandler.freezed = false;
        }

        particle.gameObject.SetActive(false);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "EnemyProjectile" && !collision.gameObject.GetComponent<ProjectileHandler>().disabledDamage)
        {
            var projHander = collision.gameObject.GetComponent<ProjectileHandler>();
            projHander.disabledDamage = true;
            PlayerAttacked(projHander.damage, projHander.onHitEffect, projHander.onHitEffectTime, projHander.damageOverTime, projHander.walkSpeedMultiplier);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.name == "DogAttackCollider")
        {
            if (!other.transform.parent.GetComponent<DogEnemyHandler>().hasHitten)
            {
                other.transform.parent.GetComponent<DogEnemyHandler>().hasHitten = true;
                PlayerAttacked(other.transform.parent.GetComponent<DogEnemyHandler>().damage);
            }
        }
    }
}
