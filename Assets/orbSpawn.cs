using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbSpawn : MonoBehaviour
{
    public GameObject orb1Prefab;
    public GameObject orb2Prefab;
    public int orbDropChance = 100;
    

    public void SpawnOrbAt(Vector3 enemyPosition)
    {
        int randomNum = Random.Range(1, 101);
        Debug.Log($"You have {orbDropChance}% chance of spawning orb.");
        int randomOrb = Random.Range(1, 3);

        if (randomNum <= orbDropChance)
        {
            if(randomOrb == 1){
                Instantiate(orb1Prefab, enemyPosition + new Vector3(0, 1, 0), transform.rotation);
            }
            if(randomOrb == 2){
                Instantiate(orb2Prefab, enemyPosition + new Vector3(0, 1, 0), transform.rotation);
            }
            
        }

    }
}
