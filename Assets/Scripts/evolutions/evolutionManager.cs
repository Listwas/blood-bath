using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
[System.Serializable]
public class EvolutionSkills
{
    public string skillName;
    public bool isSkillActive;
}
[System.Serializable]
public class Evolution
{
    public int evolutionIndex;
    public int cameraDistance;
    public GameObject playerPrefab;
    public List<EvolutionSkills> skills;
    //camera pos?
}


public class evolutionManager : MonoBehaviour
{
    public Evolution currentEvolution;
    public List<Evolution> allEvolutions;

    private GameEvents events;
    private CinemachineFreeLook camComponent;
    private GameObject player;
    void Awake()
    {
        events = FindObjectOfType<GameEvents>();
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        FindPlayerParent(allPlayers);
    }

    private void OnEnable()
    {
        events.OnEvolutionChange += Evolve;
        //events.OnSkillChange += ChangeSkillStatus;
    }
    private void OnDisable()
    {
        events.OnEvolutionChange -= Evolve;
        //events.OnSkillChange -= ChangeSkillStatus;
    }
    private void Evolve(int index, Vector3 position)
    {
        currentEvolution = allEvolutions[index];
        ChangePlayerPrefab(position);
    }

    private void ChangePlayerPrefab(Vector3 position)
    {
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
        FindPlayerParent(allPlayers);
        Destroy(player);
        GameObject newPlayerPref = Instantiate(currentEvolution.playerPrefab, position, Quaternion.identity);
        ChangeCamera(newPlayerPref);

        Debug.Log("New Player spawned");
    }
    private void FindPlayerParent(GameObject[] objects)
    {
            foreach (GameObject obj in objects)
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
                player = obj;
                Debug.Log("Old Player found" + player.name);
                break;
            }
        }

        if (player == null)
        {
            Debug.LogError("Nie znaleziono głównego obiektu gracza!");
            return;
        }
    }
    private void ChangeCamera(GameObject newPlayerPref)
    {
        GameObject camera = GameObject.FindGameObjectWithTag("Camera");
        camComponent = camera.GetComponent<CinemachineFreeLook>();

        camComponent.Follow = newPlayerPref.transform;
        camComponent.LookAt = newPlayerPref.transform;
        if (camComponent != null)
        {
            camComponent.m_Lens.FieldOfView = currentEvolution.cameraDistance;
        }
    }
    private void ChangeSkillStatus(string skillName, bool status)
    {
        //var skill = player.GetComponent(skillName);
        //skill.enabled = status;
        for (int i = 0; i < currentEvolution.skills.Count; i++)
        {
            if (currentEvolution.skills[i].skillName == skillName)
            {
                currentEvolution.skills[i].isSkillActive = status;
            }
        }
    }


}
