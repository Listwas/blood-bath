using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int bulletDamage = 10;
    private Transform attacker;
    
    public void SetAttacker(Transform enemyTransform)
    {
        attacker = enemyTransform;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            CombatScript playerCombat = other.GetComponent<CombatScript>();
            if (playerCombat != null) {
                if (playerCombat.IsParrying()) {
                    Debug.Log("Bullet parried! No damage taken.");
                    Destroy(gameObject);
                    return;
                }
                playerCombat.TakeDamage(bulletDamage, attacker); 
            }
            Destroy(gameObject);
        } else if (other.gameObject.CompareTag("Obstacle")) {
            Destroy(gameObject);
        }
    }
}