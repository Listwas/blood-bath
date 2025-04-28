using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbSpawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> healingOrbsList;
    [SerializeField] private int OrbDropChance = 100;
    [SerializeField] private float heightOffset;

    private int randomNum;
    [SerializeField]private GameEvents events;
    void Awake()
        {
            events = FindObjectOfType<GameEvents>();
        }
    private void OnEnable()
    {
        events.OnEnemyDeath += SpawnRandomOrbAtPosition;
    }

    private void OnDisable()
    {
        events.OnEnemyDeath -= SpawnRandomOrbAtPosition;
    }

    private void SpawnRandomOrbAtPosition(Vector3 enemyPosition){
        Debug.Log("odebrano sygnal o zgonie przeciwnika");
        randomNum = Random.Range(1, 101);
        if(randomNum <= OrbDropChance){
            int randomOrb = Random.Range(0, healingOrbsList.Count);
            Instantiate(healingOrbsList[randomOrb], new Vector3(enemyPosition.x, enemyPosition.y + heightOffset, enemyPosition.z), transform.rotation);
        }
    }    
}
