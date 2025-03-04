using System.Collections;
using UnityEngine;

public class CollisionWithPlayer : MonoBehaviour
{
    private StopPlayerMove stopScript;
    private BloodCount countScript;

    void Start()
    {
        stopScript = FindObjectOfType<StopPlayerMove>();
        countScript = FindObjectOfType<BloodCount>();
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Player"))
        {
            //sa 2 collidery u playera
            if (collision is BoxCollider)
            {
                Debug.Log("Player in blood");
                StartCoroutine(stopScript.moveBlock());
                countScript.bloodLogic();
            }

        }
            
    }

}

