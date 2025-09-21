using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAIScript : BaseAIScript
{
    public AttackSO[] meleeAttacks;

    private float attackCooldownTimer = 0f;
    private int attackIndex = 0;
    [SerializeField] private AttackingPatters attackingPatters; 
    

    protected override void HandleStates()
    {
        base.HandleStates();

        attackCooldownTimer -= Time.deltaTime;
    }

    protected override void AttackPlayer()
    {
        agent.SetDestination(transform.position); // Stop
        //transform.LookAt(player);

        if (attackCooldownTimer <= 0f)
        {
            attackCooldownTimer = meleeAttacks[attackIndex].cooldown;
    
            if(attackingPatters == AttackingPatters.RoundRobin)
            {
                Debug.Log("AttackPlayer called!");
                DoAttack(attackIndex);

                attackIndex++;

                if (attackIndex >= meleeAttacks.Length)
                {
                    attackIndex = 0;
                }
            }
        }
    }

    private void DoAttack(int attackIndex)
    {
        meleeAttacks[attackIndex].ExecuteAttack(transform, player, firePoint, playerMask, this);
    }

    protected override float GetAttackRange()
    {
        if (meleeAttacks.Length != 0)
        {
            return meleeAttacks[attackIndex].attackRange;
        }
        else
        {
            return 5f;
        }
    }

    private enum AttackingPatters
    {
        Random,
        RoundRobin,
    }
}
