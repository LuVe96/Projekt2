using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
    public float lifeAmount = 100;
    private float FullLifeAmount;


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

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.transform.tag == "EnemyProjectile")
    //    {
    //        lifeAmount -= collision.gameObject.GetComponent<ProjectileHandler>().damage;
    //        GameObject.Find("IngameUICanvas").transform.Find("lifeBar/front").gameObject.GetComponent<Image>().fillAmount = lifeAmount / FullLifeAmount;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyProjectile")
        {
            Debug.Log("ht trigger ");
            lifeAmount -= other.gameObject.GetComponent<ProjectileHandler>().damage;
            GameObject.Find("IngameUICanvas").transform.Find("lifeBar/front").gameObject.GetComponent<Image>().fillAmount = lifeAmount / FullLifeAmount;
        }
    }
}
