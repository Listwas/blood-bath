using System.Collections;
using UnityEngine;


public class spawningBlood : MonoBehaviour
{
    public GameObject bloodPrefab;
    public int bloodDropChance = 100;
    public int bloodDestructionTime;
    private PlayerEvade ev;
    void Start(){
        ev = FindObjectOfType<PlayerEvade>();
    }

    public void SpawnBloodAt(Vector3 enemyPosition)
    {
        int randomNum = Random.Range(1, 101);
        Debug.Log($"You have {bloodDropChance}% chance of spawning blood.");

        if (randomNum <= bloodDropChance)
        {
            Debug.Log($"Blood spawned at position {enemyPosition} with {bloodDropChance}% chance.");
            GameObject spawnedBlood = Instantiate(bloodPrefab, enemyPosition, transform.rotation);

            StartCoroutine(bloodDestroy(spawnedBlood));
        }

    }

    IEnumerator bloodDestroy(GameObject spawnedBlood)
    {   
        Debug.Log("Wait for " + bloodDestructionTime + " seconds to destroy blood");
        yield return new WaitForSeconds(bloodDestructionTime);
        if(spawnedBlood != null && ev.enabled == true)
        {
            Destroy(spawnedBlood);
        }
    }

}
