using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform model;
    [SerializeField] private float speed = 5;
    [SerializeField] private float turnSpeed = 360;

    private Vector3 input;
    private bool isEvading = false;
    private float evadeEndTime = 0f;

    [Header("Combat Settings")]
    public Animator animator;
    public float fastAttackRange = 2f;
    public float midSlowAttackRange = 3f;
    public int fastAttackDamage = 5;
    public int midSlowAttackDamage = 10;
    public LayerMask enemyLayers;
    public float evadeDistance = 10f;
    public float evadeDuration = 0.2f;
    public float evadeCooldown = 1f;

    private int attackCount = 0;
    private float lastAttackTime = 0f;
    private float attackCooldown = 0.5f;

    private void Update() {
        HandleInput();
        GatherMovementInput();
        HandleRotation();
    }

    private void FixedUpdate() {
        if (!isEvading) {
            MovePlayer();
        }
    }

    private void GatherMovementInput() {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
    }

    private void HandleRotation() {
        if (input == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(input.ToIso(), Vector3.up);
        model.rotation = Quaternion.RotateTowards(model.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void MovePlayer() {
        Vector3 moveDirection = input.ToIso().normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDirection);
    }

    private void HandleInput() {
        if (Input.GetButtonDown("Fire1")) {
            PerformFastAttack();
        } else if (Input.GetButtonDown("Fire2")) {
            PerformMidSlowAttack();
        } else if (Input.GetButtonDown("Evade") && !isEvading) {
            StartCoroutine(PerformEvade());
        }
    }

    private void PerformFastAttack() {
        if (Time.time - lastAttackTime < attackCooldown) return;

        attackCount = (attackCount % 3) + 1; 

        animator.SetTrigger("FastAttack" + attackCount);
        Debug.Log("Performing fast attack " + attackCount);

        lastAttackTime = Time.time;
    }

    private void PerformMidSlowAttack() {
        animator.SetTrigger("MidSlowAttack");
        Debug.Log("Performing mid slow attack");
    }

    private IEnumerator PerformEvade() {
        isEvading = true;
        Debug.Log("Evading...");

        Vector3 inputDirection = GetIsoInputDirection();
        if (inputDirection != Vector3.zero) {
            Vector3 evadeVelocity = inputDirection * (evadeDistance / evadeDuration);
            float elapsedTime = 0f;

            while (elapsedTime < evadeDuration) {
                rb.velocity = evadeVelocity;
                elapsedTime += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            rb.velocity = Vector3.zero;
        }

        evadeEndTime = Time.time + evadeCooldown; 
        isEvading = false;
    }

    private void CheckEvadeCooldown() {
        if (isEvading && Time.time > evadeEndTime) {
            isEvading = false;
        }
    }

    public Vector3 GetIsoInputDirection() {
        return input.ToIso().normalized;
    }

    private void ApplyFastAttackDamage() {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward, fastAttackRange, enemyLayers);
        foreach (Collider enemy in hitEnemies) {
            enemy.GetComponent<EnemyScript>().TakeDamage(fastAttackDamage);
        }
    }

    private void ApplyMidSlowAttackDamage() {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward, midSlowAttackRange, enemyLayers);
        foreach (Collider enemy in hitEnemies) {
            enemy.GetComponent<EnemyScript>().TakeDamage(midSlowAttackDamage);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * fastAttackRange, fastAttackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + transform.forward * midSlowAttackRange, midSlowAttackRange);
    }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("soldier"))
            {
                Debug.Log("Player collided with a soldier!");
            }
        }   
}

