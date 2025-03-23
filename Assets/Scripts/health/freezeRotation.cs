using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freezeRotation : MonoBehaviour
{
   private Quaternion fixedRotation;

    void Start()
    {
        fixedRotation = Quaternion.identity; 
    }

    void LateUpdate()
    {
        transform.rotation = fixedRotation;
    }
}
