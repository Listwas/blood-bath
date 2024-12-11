using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCombat : MonoBehaviour {
    public Animator animator;
    public float fastAttackRange = 2f;
    public float midSlowAttackRange = 3f;
    public int fastAttackDamage = 5;
    public int midSlowAttackDamage = 10;
    public LayerMask enemyLayers;
    public float evadeDistance = 2f;
    public float evadeDuration = 0.2f; 
    public float evadeCooldown = 1f;

    private CharControler charControler;
    private Rigidbody rb;
    private int attackCount = 0;
    private float lastAttackTime = 0f;
    private float attackCooldown = 0.5f;
    private bool isEvading = false; 
    private Vector3 evadeVelocity;

    private void Start() {
        charControler = GetComponent<CharControler>();
        rb = GetComponent<Rigidbody>(); 
    }

    void Update() {
        HandleInput();
    }

    void HandleInput() {
        if (Input.GetButtonDown("Fire1")) {
            PerformFastAttack();
        } else if (Input.GetButtonDown("Fire2")) {
            PerformMidSlowAttack();
        } else if (Input.GetButtonDown("Evade") && !isEvading) {
            StartCoroutine(PerformEvade());
        }
    }

    void PerformFastAttack() {
        if (Time.time - lastAttackTime < attackCooldown) {
            return;
        }

        attackCount++;
        if (attackCount > 3) {
            attackCount = 1; 
        }

        animator.SetTrigger("FastAttack" + attackCount);
        Debug.Log("fast attack" + attackCount);

        lastAttackTime = Time.time;
    }

    void PerformMidSlowAttack() {
        animator.SetTrigger("MidSlowAttack");
        Debug.Log("mid slow attack");
    }

    IEnumerator PerformEvade() {
        isEvading = true;
        Debug.Log("evading");

        Vector3 isoInputDirection = charControler.GetIsoInputDirection();
        if (isoInputDirection != Vector3.zero) {
            evadeVelocity = isoInputDirection * (evadeDistance / evadeDuration);
            float elapsedTime = 0f;

            while (elapsedTime < evadeDuration) {
                rb.velocity = evadeVelocity;
                elapsedTime += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            rb.velocity = Vector3.zero; 
        }

        yield return new WaitForSeconds(evadeCooldown);
        isEvading = false;
    }

    void ApplyFastAttackDamage() {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward, fastAttackRange, enemyLayers);
        foreach (Collider enemy in hitEnemies) {
            enemy.GetComponent<EnemyScript>().TakeDamage(fastAttackDamage);
        }
    }

    void ApplyMidSlowAttackDamage() {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward, midSlowAttackRange, enemyLayers);
        foreach (Collider enemy in hitEnemies) {
            enemy.GetComponent<EnemyScript>().TakeDamage(midSlowAttackDamage);
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * fastAttackRange, fastAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + transform.forward * midSlowAttackRange, midSlowAttackRange);
    }
}
