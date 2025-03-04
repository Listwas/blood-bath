using System.Collections;
using UnityEngine;

public class HealForTime : MonoBehaviour
{
    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && collision is BoxCollider) {
            StartCoroutine(HealingOverTime(collision));
        }
    }

    IEnumerator HealingOverTime(Collider collision)
    {
        var healthComponent = collision.GetComponent<CombatScript>();
        Collider collider = GetComponent<Collider>();
        MeshRenderer mesh = GetComponent<MeshRenderer>();

        if (healthComponent != null && collider != null && mesh != null) {
            StartCoroutine(healthComponent.HealOverTime(2, 5, 3));
            collider.enabled = false;
            mesh.enabled = false;
            yield return new WaitForSeconds(10);
            Destroy(gameObject);
        }
    }
}