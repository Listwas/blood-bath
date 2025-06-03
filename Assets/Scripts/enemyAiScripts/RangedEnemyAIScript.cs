using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyAIScript : BaseAIScript
{
    public float repositionRadius = 5f;
    public int repositionSampleCount = 12;

    protected override void HandleStates()
    { 
        distanceToPlayer = (player.position - transform.position).sqrMagnitude;

        if (distanceToPlayer < ((attackRange * attackRange) / 2))
        {
            //if player is too close, run away from him
            if(HasLineOfSight())
            {
                Flee();
            }
            
        }
        else if (distanceToPlayer < attackRange * attackRange)
        {
            //AttackPlayer();
            
            if (HasLineOfSight())
            {
                AttackPlayer();
            }
            else
            {
                WalkAroundObstacle();
            }
            
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
            Debug.Log("enemy flees");
            agent.SetDestination(hit.position);
        }
    }

    
    //checks for obstacles between enemy and player while trying to shoot
    protected bool HasLineOfSight()
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
        Vector3 center = player.position;
        float angleStep = 360f / repositionSampleCount;

        for (int i = 0; i < repositionSampleCount; i++)
        {
            float angle = i * angleStep;
            Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
            Vector3 samplePos = center + dir * repositionRadius;



            NavMeshHit navHit;


            if (NavMesh.SamplePosition(samplePos, out navHit, 1.0f, NavMesh.AllAreas))
            {
                // Simulate shot from sample position
                Vector3 shootDir = (player.position - navHit.position).normalized;
                float dist = Vector3.Distance(navHit.position, player.position);

                if (!Physics.SphereCast(navHit.position, bulletPrefab.GetComponent<SphereCollider>().radius * 0.8f, shootDir, out RaycastHit hit, dist, obstacleMask)
                    || hit.transform == player)
                {
                    agent.SetDestination(navHit.position);
                    return;
                }
            }
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

    protected override void OnDrawGizmosSelected()
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
