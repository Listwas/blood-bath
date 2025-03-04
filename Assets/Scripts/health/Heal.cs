using System.Collections;
using UnityEngine;

public class Heal : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && collision is BoxCollider) {
            HealOnTrigger(collision);
        }
    }

    void HealOnTrigger(Collider collision)
    {
        var healthComponent = collision.GetComponent<CombatScript>();
        if (healthComponent != null) {
            healthComponent.Heal(10);
            Destroy(gameObject);
        }
    }
}