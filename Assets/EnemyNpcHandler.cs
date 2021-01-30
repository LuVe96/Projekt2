using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNpcHandler : NpcHandler
{


    override protected void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if (other.tag == "Player")
        {
            GetComponent<MagicanEnemyHandler>().enabled = true;
            gameObject.tag = "Enemy";
        }
    }
}
