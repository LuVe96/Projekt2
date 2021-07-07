using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTransformater : MonoBehaviour
{

    [SerializeField] MonoBehaviour[] monoBehaviours;

    public void TransormToEnemy()
    {
        foreach (MonoBehaviour mono in monoBehaviours)
        {
            mono.enabled = true;
        }

        gameObject.tag = "Enemy";
        GameObject.Find("Player").GetComponentInChildren<EnemyDetector>().AddEnemyToList(gameObject);
    }
}
