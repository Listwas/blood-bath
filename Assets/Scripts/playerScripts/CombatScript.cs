using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public ComboSystem comboSystem;
    public Animator animator;

    [Header("Combat Settings")]
    [Header("Light Attack")]
    public int light_attack_damage = 5;
    public float light_attack_range = 2f;
    public float light_attack_cooldown = 0.5f;

    [Header("Heavy Attack")]
    public int heavy_attack_damage = 10;
    public float heavy_attack_range = 3f;
    public float heavy_attack_cooldown = 1.0f;

    public LayerMask enemy_layers;

    [Header("Player Stats")]
    public int current_health = 100; 
    public int max_health = 100;     
    public bool takenDamageDebug;    
    private bool has_died = false;   

    [Header("Debug Log Enabler")]
    public bool lightAttackDebug;
    public bool heavyAttackDebug;
    public bool comboExecutedDebug;
    public bool showAttackRange;

    private float last_light_attack_time;
    private float last_heavy_attack_time;

    private void Start()
    {
        comboSystem = GetComponent<ComboSystem>();
        comboSystem.OnComboExecuted += ExecuteComboEffect;
    }

    public void ProcessPlayerInput()
    {
        if (Input.GetButtonDown("LightAttack") || Input.GetKeyDown(KeyCode.Q))
        {
            if (Time.time >= last_light_attack_time + light_attack_cooldown)
            {
                ExecuteLightAttack();
                last_light_attack_time = Time.time;
            }
        }
        else if (Input.GetButtonDown("HeavyAttack") || Input.GetKeyDown(KeyCode.E))
        {
            if (Time.time >= last_heavy_attack_time + heavy_attack_cooldown)
            {
                ExecuteHeavyAttack();
                last_heavy_attack_time = Time.time;
            }
        }
    }

    private void ExecuteComboEffect(ComboSystem.DamageType damageType, int totalDamage)
    {
        if (comboExecutedDebug)
        {
            Debug.Log($"Combo executed: {damageType}, total damage: {totalDamage}");
        }

        ApplyAttackDamage(totalDamage, damageType == ComboSystem.DamageType.Slash ? light_attack_range : heavy_attack_range);
    }

    private void ExecuteLightAttack()
    {
        comboSystem.RegisterAttack(ComboSystem.AttackType.Light);
        bool hitEnemy = ApplyAttackDamage(light_attack_damage, light_attack_range);
        animator.SetTrigger("fast attack");

        if (lightAttackDebug)
        {
            Debug.Log($"Light Attack: Damage={light_attack_damage}, Cooldown={light_attack_cooldown}s, Hit={(hitEnemy ? "enemy" : "nothing")}");
        }
    }

    private void ExecuteHeavyAttack()
    {
        comboSystem.RegisterAttack(ComboSystem.AttackType.Heavy);
        bool hitEnemy = ApplyAttackDamage(heavy_attack_damage, heavy_attack_range);
        animator.SetTrigger("heavy attack");

        if (heavyAttackDebug)
        {
            Debug.Log($"Heavy Attack: Damage={heavy_attack_damage}, Cooldown={heavy_attack_cooldown}s, Hit={(hitEnemy ? "enemy" : "nothing")}");
        }
    }

    private bool ApplyAttackDamage(int damage, float range)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward, range, enemy_layers);

        if (hitEnemies.Length > 0)
        {
            foreach (Collider enemy in hitEnemies)
            {
                EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(damage);
                }
            }
            return true;
        }

        return false;
    }

    public void TakeDamage(int damageAmount)
    {
        current_health -= damageAmount;

        if (current_health > 0)
        {
            if (takenDamageDebug)
            {
                Debug.Log("Player took " + damageAmount + " damage. Current health: " + current_health);
            }
        }
        else
        {
            if (!has_died)
            {
                has_died = true;
                Debug.Log("Player died!");
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (showAttackRange)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + transform.forward, light_attack_range);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.forward, heavy_attack_range);
        }
    }
}
