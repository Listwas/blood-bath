using System.Collections;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public ComboSystem comboSystem;
    public Animator animator;

    [Header("Combat Settings")]
    [Header("Light Attack")]
    public int lightAttackDamage = 5;
    public float lightAttackRange = 2f;
    public float lightAttackCooldown = 0.5f;

    [Header("Heavy Attack")]
    public int heavyAttackDamage = 10;
    public float heavyAttackRange = 3f;
    public float heavyAttackCooldown = 1.0f;

    public LayerMask enemyLayers;

    [Header("Player Stats")]
    private int currentHealth = 100;
    public int maxHealth = 100;
    public bool takenDamageDebug;
    private bool hasDied = false;

    [Header("Parry Settings")]
    private bool isParrying = false;
    public float parryDuration = 0.5f;
    private float parryEndTime;
    public float parryRange = 1.5f;

    private FloatingHealthBar healthBar;

    [Header("Debug Log Enabler")]
    public bool lightAttackDebug;
    public bool heavyAttackDebug;
    public bool comboExecutedDebug;
    public bool showAttackRange;
    public bool showParryRange;

    private float lastLightAttackTime;
    private float lastHeavyAttackTime;
    private BloodCount blood;

    private void Start()
    {
        comboSystem = GetComponent<ComboSystem>();
        comboSystem.OnComboExecuted += ExecuteComboEffect;
        healthBar = GetComponentInChildren<FloatingHealthBar>();
        blood = FindObjectOfType<BloodCount>();
        healthBar.DoHealthBar(currentHealth, maxHealth);
    }

   private void Update()
    {
        healthBar.DoHealthBar(currentHealth, maxHealth);

        if (isParrying && Time.time >= parryEndTime)
        {
            StopParry();
        }
    }

    public void ProcessPlayerInput()
    {
        if (Input.GetButtonDown("LightAttack") || Input.GetKeyDown(KeyCode.Q))
        {
            if (Time.time >= lastLightAttackTime + lightAttackCooldown)
            {
                ExecuteLightAttack();
                lastLightAttackTime = Time.time;
            }
        }
        else if (Input.GetButtonDown("HeavyAttack") || Input.GetKeyDown(KeyCode.E))
        {
            if (Time.time >= lastHeavyAttackTime + heavyAttackCooldown)
            {
                ExecuteHeavyAttack();
                lastHeavyAttackTime = Time.time;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            StartParry();
        }
        else if (Input.GetKeyUp(KeyCode.F)) 
        {
            StopParry();
        }
    }

    private void StartParry()
    {
        isParrying = true;
        parryEndTime = Time.time + parryDuration; // Max duration
        animator.SetTrigger("parry");
        Debug.Log("Player started parrying!");
    }

    private void StopParry()
    {
        isParrying = false;
        Debug.Log("Player stopped parrying!");
    }
    
    public bool IsParrying()
    {
        return isParrying;
    }

    private void ExecuteComboEffect(ComboSystem.DamageType damageType, int totalDamage)
    {
        if (comboExecutedDebug)
        {
            Debug.Log($"Combo executed: {damageType}, total damage: {totalDamage}");
        }
        ApplyAttackDamage(totalDamage, damageType == ComboSystem.DamageType.Slash ? lightAttackRange : heavyAttackRange);
    }

    private void ExecuteLightAttack()
    {
        comboSystem.RegisterAttack(ComboSystem.AttackType.Light);
        bool hitEnemy = ApplyAttackDamage((int)(lightAttackDamage * blood.DMGMulti), lightAttackRange);
        animator.SetTrigger("fast attack");
        if (lightAttackDebug)
        {
            Debug.Log($"Light Attack: Damage={lightAttackDamage * blood.DMGMulti}, Cooldown={lightAttackCooldown}s, Hit={(hitEnemy ? "enemy" : "nothing")}");
        }
    }

    private void ExecuteHeavyAttack()
    {
        comboSystem.RegisterAttack(ComboSystem.AttackType.Heavy);
        bool hitEnemy = ApplyAttackDamage((int)(heavyAttackDamage * blood.DMGMulti), heavyAttackRange);
        animator.SetTrigger("heavy attack");
        if (heavyAttackDebug)
        {
            Debug.Log($"Heavy Attack: Damage={heavyAttackDamage * blood.DMGMulti}, Cooldown={heavyAttackCooldown}s, Hit={(hitEnemy ? "enemy" : "nothing")}");
        }
    }

    private bool ApplyAttackDamage(int damage, float range)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward, range, enemyLayers);
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
        if (isParrying)
        {
            Debug.Log("Parry successful! No damage taken.");
            return;
        }
        currentHealth -= damageAmount;
        if (currentHealth > 0)
        {
            if (takenDamageDebug)
            {
                Debug.Log($"Player took {damageAmount} damage. Current health: {currentHealth}");
            }
        }
        else
        {
            if (!hasDied)
            {
                hasDied = true;
                Debug.Log("Player died!");
            }
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public IEnumerator HealOverTime(int healAmount, int times, float interval)
    {
        for (int i = 0; i < times; i++)
        {
            currentHealth += healAmount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            yield return new WaitForSeconds(interval);
        }
    }

    private void OnDrawGizmos()
    {
        if (showAttackRange)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + transform.forward, lightAttackRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.forward, heavyAttackRange);
        }
        if (showParryRange)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, parryRange);
        }
    }
}
