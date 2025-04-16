using System.Collections;
using System.Collections.Generic;
using TMPro;
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
            /*
            if (CanShootFreely())
            {
                AttackPlayer();
            }
            else
            {
                agent.SetDestination(player.position);
            }
            */
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

    /*
    //checks for obstacles between enemy and player while trying to shoot
    protected bool CanShootFreely()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = (player.position - firePoint.position).normalized;

        if (Physics.SphereCast(transform.position, 0.5f, directionToPlayer, out hit, attackRange, obstacleMask))
        {
            Debug.Log("obstacle between enemy and player");
            return false;
        }
        else
        {
            return true;
        }
        
    }

    protected void WalkAroundObstacle()
    {
        float searchRadius = 10f;
        int attempts = 20;

        for (int i = 0; i < attempts; i++)
        {
            Vector3 randomDir = Random.insideUnitSphere * searchRadius;
            randomDir.y = 0;
            Vector3 candidatePos = transform.position + randomDir;

            if (NavMesh.SamplePosition(candidatePos, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
            {
                Vector3 dirToPlayer = (player.position - navHit.position).normalized;
                float distance = Vector3.Distance(navHit.position, player.position);

                if (distance <= attackRange && !Physics.SphereCast(firePoint.position, 0.3f, dirToPlayer, out RaycastHit hit, distance, obstacleMask))
                {
                    agent.SetDestination(navHit.position);
                    break; // Found a good spot!
                }
            }
        }
    }
    */

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
        //sight range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        //attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        //flee range
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange/2);
    }
}
