using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{

    public ArrayList enemys { get; private set; } = new ArrayList();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemys.Add(other.gameObject);     
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemys.Remove(other.gameObject);
        }
    }

    public void RemoveFromEnemyList(GameObject enemy)
    {
        if (enemy.tag == "Enemy")
        {
            enemys.Remove(enemy);
        }
    }


}
