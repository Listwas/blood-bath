using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class healForTime : MonoBehaviour
{
     public void OnTriggerEnter(Collider collision){
        if( collision.tag == "Player"){
            if (collision is BoxCollider){
                StartCoroutine(healingforTime(collision));
            }
            
        }
    }
    IEnumerator healingforTime(Collider collision){
        var healthComponent = collision.GetComponent<CombatScript>();
         Collider collider = GetComponent<Collider>();
         MeshRenderer mesh = GetComponent<MeshRenderer>();
            if (healthComponent != null && collider != null && mesh != null){
                StartCoroutine(healthComponent.timedHealing(2)); 
                collider.enabled = false;
                mesh.enabled = false;
                yield return new WaitForSeconds(10);
                Destroy(gameObject);
            }
    }
}
