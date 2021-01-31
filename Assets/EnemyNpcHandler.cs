using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNpcHandler : NpcHandler
{


    override protected void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.tag == "Player")
        {
            GetComponent<MagicanEnemyHandler>().enabled = true;
            GetComponent<NavMeshAgent>().enabled = true;
            gameObject.tag = "Enemy";
            GameObject.Find("Player").GetComponentInChildren<EnemyDetector>().AddEnemyToList(gameObject);
            GetComponent<NPCDialogHandler>().enabled = false;
            base.enabled = false;
            enabled = false;

        }
    }
}
