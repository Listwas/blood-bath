using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialEventTest : MonoBehaviour
{
    public string testName;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision is BoxCollider && collision.CompareTag("Player"))
            {
                DialogueManager.DialogueEvent(testName);
            }
            
        
    }
}
