using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseAIScript : MonoBehaviour
{
    [SerializeField] protected NavMeshAgent agent;

    [SerializeField] protected Transform player;

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



}
