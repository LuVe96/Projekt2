using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MagicanEnemyHandler : EnemyHandler
{
    private float stdMoveSpeed;
    public GameObject projectilePrfab;


    protected override void Start()
    {
        base.Start();
        stdMoveSpeed = navMeshAgent.speed;
    }

    protected override void AttackPlayer()
    {
        StartCoroutine(ProjectilAttack());  
    }

    protected override void isDieing()
    {

        //StopAllCoroutines();
        Destroy(transform.Find("focus_marker").gameObject);
        StartCoroutine(Dieing());
    }

    IEnumerator ProjectilAttack()
    {
        
        this.navMeshAgent.speed = 0;
        animator.SetBool("isWalking", false);
        animator.SetBool("isShooting", true);

        yield return new WaitForSeconds(0.5f);

        GameObject projectile = Instantiate(projectilePrfab);
        projectile.transform.position = projectileSpawnPos.position; /*transform.forward + transform.localPosition;*/
        projectile.GetComponent<ProjectileHandler>().ShotAt(player.transform.position + new Vector3(0,projectileSpawnPos.localPosition.y, 0));

        yield return new WaitForSeconds(0.5f);

        navMeshAgent.speed = stdMoveSpeed;
        animator.SetBool("isShooting", false);
        animator.SetBool("isWalking", true);
    }

    IEnumerator Dieing()
    {
        animator.SetBool("isDieing", true);
        isAtDieing = true;
        GetComponent<NavMeshAgent>().enabled = false;

        foreach (var col in GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        yield return new WaitForSeconds(4f);

        base.isDieing();
    }

}
