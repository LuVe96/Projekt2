using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BowHandler : MonoBehaviour, IAttackEnemyInterface
{
    private ArrayList enemys = new ArrayList();
    public float attackPause = 1f;
    private float periodeTimeSum;
    public GameObject arrowPrefab;
    public Animator charakterAnimator;
    public Animator bowAnimator;
    public bool freezed = false;


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

            float distance = new Vector2(transform.parent.transform.position.x - enemy.transform.position.x,
                transform.parent.transform.position.y - enemy.transform.position.y).magnitude;
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

        if (!HitsAntiAttackCollider(nearestEnemy))
        {
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
       
    }

    public void MarkFocusedEnemy(GameObject enemy)
    {
        enemy.transform.Find("focus_marker").gameObject.SetActive(true);
        enemy.GetComponent<EnemyIndicator>().setFocused(true);

        //disable others
        foreach (GameObject e in enemys)
        {
            if (e != enemy)
            {
                e.transform.Find("focus_marker").gameObject.SetActive(false);
                enemy.GetComponent<EnemyIndicator>().setFocused(false);
            }
        }
    }

    private bool HitsAntiAttackCollider(GameObject enemy)
    {
        Ray ray = new Ray(transform.position, enemy.transform.position - transform.position);
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        RaycastHit[] hits = Physics.RaycastAll(ray, distance);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.tag == "AntiAttackCollider")
            {
                return true;
            }
        }

        return false;
    }


    public void EnableEffect(OnHitEffectType effect, float time)
    {

    }


    IEnumerator AttackEnemy(GameObject enemy)
    {
        if (enemy == null) { yield return null; }

        charakterAnimator.SetBool("isShooting", true);

        yield return new WaitForSeconds(0.1f);
        bowAnimator.SetBool("isShooting", true);


        yield return new WaitForSeconds(0.2f);

        GameObject arrow = Instantiate(arrowPrefab);
        arrow.transform.position = transform.position;
        arrow.GetComponent<ProjectileHandler>().ShotAt(enemy.transform.position + new Vector3(0,0.7f, 0));
        //transform.parent.parent.GetComponent<PlayerMovement>().LookAt(enemy.transform);
        charakterAnimator.SetBool("isShooting", false);
        bowAnimator.SetBool("isShooting", false);
        yield return null;
    }

    public void PlayerHasTurnedToEnemy(GameObject enemy)
    {
        //AttackEnemy(enemy);
        if(!freezed)
            StartCoroutine(AttackEnemy(enemy));
    }
}

public interface IAttackEnemyInterface
{
    void PlayerHasTurnedToEnemy(GameObject enemy);
}