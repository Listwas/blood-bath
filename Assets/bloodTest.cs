using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodTest : MonoBehaviour
{
    private GameEvents events;
    void Start()
    {
        events = FindObjectOfType<GameEvents>();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision is BoxCollider && collision.CompareTag("Player"))
            {
                events.EnemyHit();
            }
            
        
    }
}
