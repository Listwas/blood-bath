using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trapCollision : MonoBehaviour
{

    public CombatScript combat;
    // Start is called before the first frame update
    void Start()
    {
        combat = FindObjectOfType<CombatScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
