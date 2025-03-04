using System.Collections;
using UnityEngine;

public class collisionWithPlayer : MonoBehaviour
{
    private stopPlayerMove stopScript;
    private BloodCount countScript;


    void Start()
    {
        stopScript = FindObjectOfType<stopPlayerMove>();
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

