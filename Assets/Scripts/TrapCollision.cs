using System.Collections;
using UnityEngine;

public class TrapCollision : MonoBehaviour
{
    public int trapDMG = 5;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && collision is BoxCollider) {
            Debug.Log("Player in trap");
            var combat = collision.GetComponent<CombatScript>();
            if (combat != null) {
                combat.TakeDamage(trapDMG);
            }
        }
    }
}