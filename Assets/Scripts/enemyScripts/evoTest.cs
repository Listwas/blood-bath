using UnityEngine;

public class evoTest : MonoBehaviour
{
    private GameEvents events;
    private bool canChange = true;
    [SerializeField] private int evoIndex;
    [SerializeField]private GameObject oldPlayerPref = null;
    private Vector3 plPos;
    void Start()
    {
        events = FindObjectOfType<GameEvents>();
        FindParent();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision is BoxCollider && collision.CompareTag("Player") && canChange)
        {
            plPos = oldPlayerPref.transform.position;
            events.EvolutionChange(evoIndex, plPos);
            canChange = false;
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
    }
}
