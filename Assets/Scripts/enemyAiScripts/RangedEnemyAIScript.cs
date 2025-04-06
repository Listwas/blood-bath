using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyAIScript : BaseAIScript
{
    

    protected override void HandleStates()
    { 
        distanceToPlayer = (player.position - transform.position).sqrMagnitude;

        if (distanceToPlayer < ((attackRange * attackRange) / 2))
        {
            //if player is too close, run away from him
            Flee();
        }
        else if (distanceToPlayer < attackRange * attackRange)
        {
            AttackPlayer();
        }
        else if (distanceToPlayer < sightRange * sightRange)
        {
            ChasePlayer();
        }
        else
        {
            RandomPatrol();
        }
    }

    private void Flee()
    {
        //choose direction away from the player and choose a point in that direction to run to
        Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
        Vector3 fleeTarget = transform.position + directionAwayFromPlayer * 2;

        //if the point exists, run towards it
        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeTarget, out hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    protected override void AttackPlayer()
    {
        //stop moving
        agent.SetDestination(transform.position);

        //TODO: Fix Bug: When player is too close, enemy looks at him and angles itself to look up, becoming a ramp. Player can climb it to permanently fly above, well, EVERYTHING
        // Caused by this line
        //transform.LookAt(player);

        //attacks, with interval of time in between
        if (!hasAlreadyAttacked)
        {
            // this is where the attack code goes
            ShootOrbs();
            Debug.Log("Enemy AI attacks!");

            hasAlreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    protected void ShootOrbs()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - firePoint.position).normalized;
            direction.y = 0;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            BulletScript bulletScript = bullet.GetComponent<BulletScript>();
            if (bulletScript != null)
            {
                bulletScript.SetAttacker(this.transform);
            }
            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;

            Destroy(bullet, bulletLifetime);
        }
    }

    // resets attack cooldown
    protected void ResetAttack()
    {
        hasAlreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange/2);
    }
}
