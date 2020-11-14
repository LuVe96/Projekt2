using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BowHandler : MonoBehaviour, IAttackEnemyInterface
{
    private ArrayList enemys = new ArrayList();
    public float attackPause = 1f;
    private float periodeTimeSum;
    public GameObject arrowPrefab;


    // Start is called before the first frame update
    void Start()
    {
        //then he attacks instantly
        periodeTimeSum = attackPause;
    }

    // Update is called once per frame
    void Update()
    {
        enemys = transform.parent.parent.GetComponentInChildren<EnemyDetector>().enemys;

        // return when there are no enemys
        if (enemys.Count == 0){
            return;
        }

        GameObject nearestEnemy = null;
        float? nearestDistance = null;
        foreach (GameObject enemy in enemys)
        {
            enemy.transform.Find("focus_marker").gameObject.SetActive(false);

            float distance = new Vector2(transform.parent.transform.position.x / transform.parent.transform.position.y,
                enemy.transform.position.x / enemy.transform.position.y).magnitude;
            if (!nearestDistance.HasValue)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
            else if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        MarkFocusedEnemy(nearestEnemy);



        // move on when there are enemys
        periodeTimeSum += Time.deltaTime;
        if (periodeTimeSum >= attackPause && !PlayerMovement.playerIsMoving)
        {
            periodeTimeSum = 0;        

            //AttackEnemy(enemyToAtack);
            ///let player turn to enemy then atack gets triggered
            transform.parent.parent.GetComponent<PlayerMovement>().LookAt(nearestEnemy, this);
        }
    }

    public void MarkFocusedEnemy(GameObject enemy)
    {
        enemy.transform.Find("focus_marker").gameObject.SetActive(true);
    }

    void AttackEnemy(GameObject enemy)
    {
        if (enemy == null) { return; }

        GameObject arrow = Instantiate(arrowPrefab);
        arrow.transform.position = transform.position;

        arrow.GetComponent<ProjectileHandler>().ShotAt(enemy.transform.position + new Vector3(0,Random.Range(0.5f, 1f), 0));
        //transform.parent.parent.GetComponent<PlayerMovement>().LookAt(enemy.transform);
    }

    public void PlayerHasTurnedToEnemy(GameObject enemy)
    {
        AttackEnemy(enemy);

    }
}

public interface IAttackEnemyInterface
{
    void PlayerHasTurnedToEnemy(GameObject enemy);
}