using System.Collections;
using UnityEngine;


public class spawningBlood : MonoBehaviour
{
    [SerializeField]private GameObject bloodPrefab;
    [SerializeField]private int bloodDropChance = 100;
    [SerializeField]private int bloodDestructionTime;
    private PlayerEvade ev;
    private GameEvents events;
    void Awake(){
        ev = FindObjectOfType<PlayerEvade>();
        events = FindObjectOfType<GameEvents>();
    }
    private void OnEnable()
    {
        events.OnEnemyDeath += SpawnBloodAt;
    }

    private void OnDisable()
    {
        events.OnEnemyDeath -= SpawnBloodAt;
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
