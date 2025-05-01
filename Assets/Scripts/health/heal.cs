using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heal : MonoBehaviour
{
    [SerializeField]private GameEvents events;
    [SerializeField]private string healType;
    void Start()
    {
        events = FindObjectOfType<GameEvents>();
    }
    public void OnTriggerEnter(Collider collision){
        if( collision.tag == "Player"){
            if (collision is BoxCollider){
                Debug.Log("wejscie w heal");
                events.HealEnter(healType);
                //Destroy(gameObject);
            }
            
        }
    }

}
