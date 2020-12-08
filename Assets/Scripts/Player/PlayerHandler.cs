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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "EnemyProjectile" && !collision.gameObject.GetComponent<ProjectileHandler>().disabledDamage)
        {
            collision.gameObject.GetComponent<ProjectileHandler>().disabledDamage = true;
            DamagePlayer(collision.gameObject.GetComponent<ProjectileHandler>().damage);
            Destroy(collision.gameObject);
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

    private void DamagePlayer(float amount)
    {
        lifeAmount -= amount;
        uiLifeBarFront.GetComponent<Image>().fillAmount = lifeAmount / MaxLifeAmount;
        Instantiate(bloodParticles, transform.position, transform.rotation);
        hitSound.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyProjectile")
        {
            if( other.transform.parent.GetComponent<EnemyHandler>().enemyType == EnemyType.Dog 
                && !other.transform.parent.GetComponent<DogAttackHandler>().hasHitten)
            {
                other.transform.parent.GetComponent<DogAttackHandler>().hasHitten = true;
                DamagePlayer(other.transform.parent.GetComponent<DogAttackHandler>().damage);
            }

        }
    }
}
