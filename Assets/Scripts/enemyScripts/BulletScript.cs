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
