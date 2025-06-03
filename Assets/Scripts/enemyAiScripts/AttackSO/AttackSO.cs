using UnityEngine;

public abstract class AttackSO : ScriptableObject
{
    public string attackName;
    public int damage;
    public float cooldown;
    public bool isRanged = false;

    public virtual float attackRange => 5f;

    public abstract void ExecuteAttack(Transform attacker, Transform target, Transform shootOrigin, LayerMask targetMask, MonoBehaviour context);

    public virtual void DealDamage(Collider hit, Transform attacker)
    {
        if (hit.gameObject.CompareTag("Player"))
        {
            CombatScript playerCombat = hit.GetComponent<CombatScript>();
            if (playerCombat != null)
            {
                if (playerCombat.IsParrying())
                {
                    Debug.Log("Hit parried! No damage taken.");
                    return;
                }
                else
                {
                    playerCombat.TakeDamage(damage, attacker);
                }
            }
        }
    }

    public virtual void GetAttackRange()
    {

    }
}
