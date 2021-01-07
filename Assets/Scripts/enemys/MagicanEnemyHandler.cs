using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    IEnumerator ProjectilAttack()
    {
        
        this.navMeshAgent.speed = 0;
        animator.SetBool("isWalking", false);
        animator.SetBool("isShooting", true);

        yield return new WaitForSeconds(0.5f);

        GameObject projectile = Instantiate(projectilePrfab);
        projectile.transform.position = projectileSpawnPos.position; /*transform.forward + transform.localPosition;*/
        projectile.GetComponent<ProjectileHandler>().ShotAt(player.transform.position + new Vector3(0,projectileSpawnPos.position.y, 0));

        yield return new WaitForSeconds(0.5f);

        navMeshAgent.speed = stdMoveSpeed;
        animator.SetBool("isShooting", false);
        animator.SetBool("isWalking", true);
    }

}
