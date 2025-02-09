using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int bulletDamage = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CombatScript playerCombat = other.GetComponent<CombatScript>();
            if (playerCombat != null)
            {
                if (playerCombat.is_parrying)
                {
                    Debug.Log("Bullet parried! No damage taken.");
                    Destroy(gameObject);
                    return;
                }
                playerCombat.TakeDamage(bulletDamage);
            }

            Destroy(gameObject);  
        }
        else if (other.gameObject.tag == "Obstacle")
        {
            Destroy(gameObject);  
        }
    }
}
