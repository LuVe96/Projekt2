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

    ///Efects
    public ParticleSystem burnEffect;

    // Start is called before the first frame update
    void Start()
    {
        MaxLifeAmount = lifeAmount;
        //uiLifeBar = GameObject.Find("IngameUICanvas").transform.Find("lifeBar/front").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(lifeAmount <= 0)
        {
            GameManager.Instance.playerIsDead = true;
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

    private void DamagePlayer(float amount, OnHitEffectType effect = OnHitEffectType.None, float effectTime = 0)
    {
        lifeAmount -= amount;
        uiLifeBarFront.GetComponent<Image>().fillAmount = lifeAmount / MaxLifeAmount;
        Instantiate(bloodParticles, transform.position, transform.rotation);
        hitSound.Play();

        switch (effect)
        {
            case OnHitEffectType.Burn:
                StartCoroutine(EnableEffect(burnEffect, effectTime));
                break;
            default: break;
        }
       
    }

    IEnumerator EnableEffect(ParticleSystem particle, float time)
    {
        particle.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        particle.gameObject.SetActive(false);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "EnemyProjectile" && !collision.gameObject.GetComponent<ProjectileHandler>().disabledDamage)
        {
            var projHander = collision.gameObject.GetComponent<ProjectileHandler>();
            projHander.disabledDamage = true;
            DamagePlayer(projHander.damage, projHander.onHitEffect, projHander.onHitEffectTime);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyProjectile")
        {
            if (!other.transform.parent.GetComponent<DogEnemyHandler>().hasHitten)
            {
                other.transform.parent.GetComponent<DogEnemyHandler>().hasHitten = true;
                DamagePlayer(other.transform.parent.GetComponent<DogEnemyHandler>().damage);
            }

        }
    }
}
