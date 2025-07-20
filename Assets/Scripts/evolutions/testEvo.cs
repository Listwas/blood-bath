
using UnityEngine;

public class testEvo : MonoBehaviour
{
    /*[SerializeField] private GameEvents events;
    [SerializeField] private GameObject player;
    [SerializeField] private int evoIndex;
    private GameObject oldPlayerPref = null;

    void Start()
    {
        events = FindObjectOfType<GameEvents>();
        FindParent();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision is BoxCollider && collision.CompareTag("Player"))
        {
            events.EvolutionChange(evoIndex, oldPlayerPref.transform.position);

        }


    }
    private void FindParent()
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        //looking for parent obj
        foreach (GameObject obj in allPlayers)
        {
            Transform parent = obj.transform.parent;
            bool hasPlayerParent = false;

            while (parent != null)
            {
                if (parent.CompareTag("Player"))
                {
                    hasPlayerParent = true;
                    break;
                }
                parent = parent.parent;
            }

            if (!hasPlayerParent)
            {
                oldPlayerPref = obj;
                Debug.Log("Old Player found" + oldPlayerPref.name);
                break;
            }
        }

        if (oldPlayerPref == null)
        {
            Debug.LogError("Nie znaleziono głównego obiektu gracza!");
            return;
        }
    }*/
}
