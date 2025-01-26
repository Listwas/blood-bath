using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heal : MonoBehaviour
{
    public void OnTriggerEnter(Collider collision){
        if( collision.tag == "Player"){
            if (collision is BoxCollider){
                healOnTrigger(collision);
            }
            
        }
    }
    void healOnTrigger(Collider collision){
        var healthComponent = collision.GetComponent<CombatScript>();
            if (healthComponent != null){
                healthComponent.HealingOneTime(10);
                Destroy(gameObject);
            }
    }
}
