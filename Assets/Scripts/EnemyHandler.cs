using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHandler : MonoBehaviour
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
            GameObject.Find("EnemyDetection").GetComponent<EnemyDetector>().removeFromEnemyList(transform.Find("Charakter").gameObject);
            Destroy(gameObject);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Arrow")
        {
            lifeAmount -= collision.gameObject.GetComponent<ArrowHandler>().damage;

            transform.Find("Canvas/lifebar/front").gameObject.GetComponent<Image>().fillAmount = lifeAmount / FullLifeAmount;
        }
    }
}
