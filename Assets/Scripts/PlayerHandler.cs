using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    public float lifeAmount = 100;
    private float FullLifeAmount;
    public GameObject bloodParticles;
    public AudioSource hitSound;

    // Start is called before the first frame update
    void Start()
    {
        FullLifeAmount = lifeAmount;
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
            lifeAmount -= collision.gameObject.GetComponent<ProjectileHandler>().damage;
            GameObject.Find("IngameUICanvas").transform.Find("lifeBar/front").gameObject.GetComponent<Image>().fillAmount = lifeAmount / FullLifeAmount;
            Instantiate(bloodParticles, transform.position, transform.rotation);
            hitSound.Play();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyProjectile")
        {
            if( other.transform.parent.GetComponent<EnemyHandler>().enemyType == EnemyType.Dog)
            {
                lifeAmount -= other.transform.parent.GetComponent<DogAttackHandler>().damage;
                GameObject.Find("IngameUICanvas").transform.Find("lifeBar/front").gameObject.GetComponent<Image>().fillAmount = lifeAmount / FullLifeAmount;
                Instantiate(bloodParticles, transform.position, transform.rotation);
                hitSound.Play();
                
            }

        }
    }
}
