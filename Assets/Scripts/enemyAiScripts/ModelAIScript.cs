using System.Collections;
using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;
using UnityEngine.AI;

public class ModelAIScript : BaseAIScript
{


    private void Awake()
    {
        /*
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        blood = FindObjectOfType<SpawningBlood>();
        orbs = FindObjectOfType<orbSpawn>();
        healthBar.DoHealthBar(currentHealth, maxHealth);
        */
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        blood = FindObjectOfType<SpawningBlood>();
        orbs = FindObjectOfType<orbSpawn>();
        healthBar.DoHealthBar(currentHealth, maxHealth);
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerMask);

        if(!playerInSightRange && !playerInAttackRange)
        {
            RandomPatrol();
        }
        if(playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        if(playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
    }

    private void RandomPatrol()
    {
        //searches for a random point in range to walk to
        if(!isWalkPointSet)
        {
            SearchWalkPoint();
        }

        // walks towards the point
        if(isWalkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        //resets the point upon reaching it
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if(distanceToWalkPoint.magnitude < 1f)
        {
            isWalkPointSet = false;
        }
    }

    //search for a random point in range to walk to
    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        Debug.Log("walk point selected:" + walkPoint);

        //check if its in boundaries of NavMeshSurface (i think)
        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
        {
            Debug.Log("walk point selected");
            isWalkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        //starts chasing the player
        Debug.Log("Enemy AI chases!");
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //stop moving
        agent.SetDestination(transform.position);

        //TODO: Bug: When player is too close, enemy looks at him and angles itself to look up, becoming a ramp. Player can climb it to permanently fly above, well, EVERYTHING
        // Caused by this line
        transform.LookAt(player);

        //attacks, with interval of time in between
        if(!hasAlreadyAttacked)
        {
            // this is where the attack code goes
            ShootOrbs();
            Debug.Log("Enemy AI attacks!");

            hasAlreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ShootOrbs()
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
    private void ResetAttack()
    {
        hasAlreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        // Debug.Log("enemy took " + damage + " damage. Current health: " +
        // currentHealth);
        healthBar.DoHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead)
            return;
        isDead = true;
        Vector3 enemyPosition =
          new Vector3(transform.position.x, 0, transform.position.z);
        if (blood != null && orbs != null)
        {
            blood.SpawnBloodAt(enemyPosition);
            orbs.SpawnOrbAt(enemyPosition);
        }
        Debug.Log("enemy died!");
        Destroy(gameObject);
        Debug.Log("Die() called for " + gameObject.name);
    }
}
