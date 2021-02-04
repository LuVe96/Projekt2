using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatHandler : MonoBehaviour, IAttackEnemyInterface
{
    public WeaponHandler weaponHandler;

    //SpellParticle
    public GameObject fireWave;
    public GameObject iceWave;
    public GameObject poisonWave;

    [HideInInspector]
    public bool freezed = false;

    private ArrayList enemys = new ArrayList();
    private float attackPause = 1f;
    private float periodeTimeSum;


    // Start is called before the first frame update
    void Start()
    {
        //then he attacks instantly
        attackPause = weaponHandler.attackPause;
        periodeTimeSum = attackPause;
    }

    // Update is called once per frame
    void Update()
    {
        enemys = GetComponentInChildren<EnemyDetector>().enemys;

        // return when there are no enemys
        if (enemys.Count == 0)
        {
            return;
        }

        GameObject nearestEnemy = null;
        float? nearestDistance = null;
        foreach (GameObject enemy in enemys)
        {

            float distance = new Vector2(transform.position.x - enemy.transform.position.x,
                transform.position.y - enemy.transform.position.y).magnitude;
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

        if (!HitsAntiAttackCollider(nearestEnemy)/* && OnSameLevel(transform, nearestEnemy.transform)*/)
        {
            MarkFocusedEnemy(nearestEnemy);

            // move on when there are enemys
            periodeTimeSum += Time.deltaTime;
            if (periodeTimeSum >= attackPause && !PlayerMovement.playerIsMoving)
            {
                periodeTimeSum = 0;

                //AttackEnemy(enemyToAtack);
                ///let player turn to enemy then atack gets triggered
                GetComponent<PlayerMovement>().LookAt(nearestEnemy, this);
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

    private bool OnSameLevel(Transform player, Transform enemy, float toleranz = 0.5f)
    {
        return (player.position.y + toleranz / 2 >= enemy.position.y) && (player.position.y - toleranz / 2 <= enemy.position.y);
    }

    private bool HitsAntiAttackCollider(GameObject enemy, float height = 1.2f) // height = shot height
    {
        Vector3 heightVector = new Vector3(0, height, 0);
        Ray ray = new Ray(transform.position + heightVector, enemy.transform.position + heightVector - transform.position + heightVector);
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

    public void PlayerHasTurnedToEnemy(GameObject enemy)
    {
        //AttackEnemy(enemy);
        if (!freezed)
            weaponHandler.Attack(enemy);
           
    }

    public void CastSpell(OnHitEffectType onHitEffectType)
    {

        switch (onHitEffectType)
        {
            case OnHitEffectType.Burn:
                var fireW = Instantiate(fireWave);
                fireW.transform.position = transform.position;
                break;
            case OnHitEffectType.Freeze:
                var iceW = Instantiate(iceWave);
                iceW.transform.position = transform.position;
                break;
            case OnHitEffectType.Poison:
                var poisonW = Instantiate(poisonWave);
                poisonW.transform.position = transform.position;
                break;
            default: break;
        }
    }
}

public interface IAttackEnemyInterface
{
    void PlayerHasTurnedToEnemy(GameObject enemy);
}

