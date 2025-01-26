using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapCollision : MonoBehaviour
{

    private CombatScript combat;
    public int trapDMG = 5;
    public int howQuickDMG = 2;
    // Start is called before the first frame update
    void Start()
    {
        combat = FindObjectOfType<CombatScript>();
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            //sa 2 collidery u playera
            if (collision is BoxCollider)
            {
                Debug.Log("Player in trap");
                combat.current_health -= trapDMG;
            }

        }
            
    }
}
