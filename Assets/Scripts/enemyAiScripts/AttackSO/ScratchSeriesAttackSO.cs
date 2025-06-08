using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Enemy Attacks/Scratch Series Attack")]
public class ScratchSeriesAttackSO : AttackSO
{
    [Header("The 'damage' variable above does not affect the damage of the scratches. That is defined in ScratchSeriesParameters ScriptableObjects.")]

    [Header("Scratch variables")]
    //[SerializeField] private float biteAngle = 60f;
    //[SerializeField] private float biteRadius = 2f;
    [SerializeField] private float biteWaitDuration = 1f;
    [SerializeField] protected float biteTriggerRange = 3f;

    /*
    [Header("Lunge variables")]
    [SerializeField] private float lungeDistance = 1f;
    [SerializeField] private float lungeDuration = 0.1f;
    */

    [Header("Scratch indicator variables")]
    [SerializeField] private Color indicatorColor = Color.red;

    [Header("Variables of individual scratches")]
    [SerializeField] private List<ScratchSeriesParametersSO> scratchesParameters = new();
    //private int scratchSeriesIndicator = 0;


    public override float attackRange => biteTriggerRange;

    private void OnEnable()
    {
        //scratchSeriesIndicator = 0;
    }

    public override void ExecuteAttack(Transform attacker, Transform target, Transform origin, LayerMask targetMask, MonoBehaviour context)
    {
        Debug.Log("ExecuteAttack of BiteSO called!");
        if (Vector3.Distance(attacker.position, target.position) < biteTriggerRange)
        {
            Vector3 directionToPlayer = target.position - attacker.position;

            NavMeshAgent agent = attacker.GetComponent<NavMeshAgent>();

            directionToPlayer.y = 0;

            if (directionToPlayer != Vector3.zero)
            {
                context.StartCoroutine(ScratchSeriesRoutine(attacker, target, agent, targetMask, context));
            }
        }
    }

    private IEnumerator ScratchSeriesRoutine(Transform attacker, Transform target, NavMeshAgent agent, LayerMask targetMask, MonoBehaviour context)
    {
        agent.isStopped = true;

        Vector3 directionToPlayer = (target.position - attacker.position).normalized;

        for(int i = 0; i < scratchesParameters.Count; i++)
        {
            if(i != 0)
            {
                yield return new WaitForSeconds(0.5f);
            }

            yield return context.StartCoroutine(BiteRoutine(attacker, directionToPlayer, targetMask, i));
        }

        agent.isStopped = false;
        yield return null;
    }

    protected IEnumerator BiteRoutine(Transform attacker, Vector3 directionToPlayer, LayerMask targetMask, int i)
    {
        //agent.isStopped = true;

        //Vector3 directionToPlayer = (target.position - attacker.position).normalized;

        Debug.Log("DisplayBiteIndicator called!");
        DisplayBiteIndicatorStatic(attacker, directionToPlayer, scratchesParameters[i].angle, scratchesParameters[i].radius);

        yield return new WaitForSeconds(biteWaitDuration);

        if (scratchesParameters[i].lungeDistance != 0f)
        {
            yield return LungeTowardTarget(attacker, directionToPlayer, scratchesParameters[i].lungeDistance, scratchesParameters[i].lungeDuration);
        }

        Collider[] hits = Physics.OverlapSphere(attacker.position, scratchesParameters[i].radius, targetMask);

        foreach (var hit in hits)
        {
            Debug.Log($"Hit: {hit.name}");
            Vector3 toTarget = (hit.transform.position - attacker.position).normalized;

            float angleToTarget = Vector3.Angle(directionToPlayer, toTarget);

            if (angleToTarget <= scratchesParameters[i].angle / 2f)
            {
                DealDamage(hit, attacker, scratchesParameters[i].damage);
                Debug.Log($"Hit: {hit.name} damage");
                break;
            }
        }
        yield return null;
    }

    private void DisplayBiteIndicatorStatic(Transform attacker, Vector3 directionToPlayer, float scratchAngle, float scratchRadius)
    {
        Debug.Log("Figuring out the bite indicator!");

        GameObject sector = new GameObject("BiteAttackIndicator");
        sector.transform.SetParent(attacker.transform);
        sector.transform.position = new Vector3(attacker.position.x, 0.03f, attacker.position.z);
        sector.transform.rotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));

        MeshFilter mf = sector.AddComponent<MeshFilter>();
        MeshRenderer mr = sector.AddComponent<MeshRenderer>();

        Mesh sectorMesh = GenerateSectorMesh(scratchAngle, scratchRadius, 30);

        mf.mesh = sectorMesh;

        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = indicatorColor;
        mr.material = mat;

        GameObject.Destroy(sector, biteWaitDuration + 0.1f);
    }

    private Mesh GenerateSectorMesh(float scratchAngle, float scratchRadius, int segments)
    {
        Debug.Log("Calculating and generating the indicator mesh!");


        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        vertices.Add(Vector3.zero); // Tip of the cone (player's position)

        float halfAngle = scratchAngle / 2f;
        float angleRad = scratchAngle * Mathf.Deg2Rad;
        float angleStep = angleRad / segments;

        vertices.Add(Vector3.zero);

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = -halfAngle * Mathf.Deg2Rad + i * angleStep;
            float x = Mathf.Sin(currentAngle) * scratchRadius;
            float z = Mathf.Cos(currentAngle) * scratchRadius;
            vertices.Add(new Vector3(x, 0f, z));
        }

        for (int i = 1; i <= segments; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    private IEnumerator LungeTowardTarget(Transform attacker, Vector3 directionToPlayer, float lungeDistance, float lungeDuration)
    {
        Vector3 startPosition = attacker.position;
        directionToPlayer.y = 0f;
        Vector3 endPosition = startPosition + directionToPlayer * lungeDistance;

        float timeElapsed = 0f;
        while (timeElapsed < lungeDuration)
        {
            attacker.position = Vector3.Lerp(startPosition, endPosition, timeElapsed / lungeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        attacker.position = endPosition;
    }
}
