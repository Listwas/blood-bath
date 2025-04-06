using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseAIScript : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;

    protected Transform player;

    [SerializeField] protected LayerMask groundMask;
    [SerializeField] protected LayerMask playerMask;

    //patrolling
    protected Vector3 walkPoint;
    protected bool isWalkPointSet = false;
    [SerializeField] protected float walkPointRange = 5f;

    //attacking
    [SerializeField] protected float timeBetweenAttacks = 2f;
    protected bool hasAlreadyAttacked;

    //States
    [SerializeField] protected float sightRange = 10f;
    [SerializeField] protected float attackRange = 2f;
    protected bool playerInSightRange;
    protected bool playerInAttackRange;
    protected float distanceToPlayer;

    [Header("Moved from EnemyScript.cs")]
    [SerializeField] protected int maxHealth = 50;
    protected int currentHealth;
    [SerializeField] protected float bulletSpeed = 10f;
    [SerializeField] protected float bulletLifetime = 2f;

    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected LayerMask obstacleMask;

    public SpawningBlood blood;
    public orbSpawn orbs;
    public FloatingHealthBar healthBar;
    protected bool isDead = false;

    protected virtual void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        blood = FindObjectOfType<SpawningBlood>();
        orbs = FindObjectOfType<orbSpawn>();
        healthBar.DoHealthBar(currentHealth, maxHealth);
    }

    protected virtual void Update()
    {
        HandleStates();
    }

    protected virtual void HandleStates()
    {
        distanceToPlayer = (player.position - transform.position).sqrMagnitude;

        if (distanceToPlayer < attackRange * attackRange)
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

    protected abstract void AttackPlayer();

    protected virtual void RandomPatrol()
    {
        //searches for a random point in range to walk to
        if (!isWalkPointSet)
        {
            SearchWalkPoint();
        }

        // walks towards the point
        if (isWalkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        //resets the point upon reaching it
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            isWalkPointSet = false;
        }
    }

    //search for a random point in range to walk to
    protected virtual void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        //Debug.Log("walk point selected:" + walkPoint);

        //check if its in boundaries of NavMeshSurface (i think)
        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundMask))
        {
            //Debug.Log("walk point selected");
            isWalkPointSet = true;
        }
    }

    protected virtual void ChasePlayer()
    {
        //starts chasing the player
        Debug.Log("Enemy AI chases!");
        agent.SetDestination(player.position);
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

