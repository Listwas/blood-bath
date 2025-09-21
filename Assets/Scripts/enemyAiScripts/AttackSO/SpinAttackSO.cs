using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class SpinAttackSO : AttackSO
{
    [Header("Spin attack variables")]
    [SerializeField] private float spinDuration = 1f;
    [SerializeField] private float spinRadius = 3f;
    [SerializeField] private float damageInterval = 0.2f;
    [SerializeField] private float rotationSpeed = 360f;

    [Header("Spin Attack Indicator variables")]
    [SerializeField] private Color indicatorColor = Color.red;

    public override float attackRange => spinRadius;

    public override void ExecuteAttack(Transform attacker, Transform target, Transform shootOrigin, LayerMask targetMask, MonoBehaviour context)
    {
        Debug.Log("ExecuteAttack of SpinAttackSO called!");
        if (Vector3.Distance(attacker.position, target.position) < spinRadius)
        {
            Vector3 directionToPlayer = target.position - attacker.position;

            NavMeshAgent agent = attacker.GetComponent<NavMeshAgent>();

            directionToPlayer.y = 0;

            if (directionToPlayer != Vector3.zero)
            {
                //context.StartCoroutine(SpinRoutine(attacker, target, agent, targetMask));

            }
        }
        throw new System.NotImplementedException();
    }
    /*
    private IEnumerator SpinRoutine (Transform attacker, Transform target, NavMeshAgent agent, LayerMask targetMask)
    {
        agent.isStopped = true;

        float elapsed = 0f;
        float nextDamageTick = 0f;

        while (elapsed < spinAttack.spinDuration)
        {
            // Rotate the enemy
            float rotationAmount = spinAttack.rotationSpeed * Time.deltaTime;
            transform.Rotate(0f, rotationAmount, 0f);

            // Apply damage in intervals
            if (elapsed >= nextDamageTick)
            {
                Collider[] hits = Physics.OverlapSphere(transform.position, spinAttack.spinRadius);
                foreach (Collider hit in hits)
                {
                    if (hit.CompareTag("Player"))
                    {
                        // Add a proper reference to your player damage system
                        var player = hit.GetComponent<PlayerImmunityAndKnockbackScript>();
                        if (player != null)
                        {
                            player.ReceiveHit(spinAttack.damage, transform);
                        }
                    }
                }

                nextDamageTick += spinAttack.damageInterval;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        agent.isStopped = false;
    }
    */
    private void DisplayBiteIndicatorStatic(Transform attacker, Vector3 directionToPlayer)
    {
        Debug.Log("Figuring out the bite indicator!");

        GameObject sector = new GameObject("BiteAttackIndicator");
        sector.transform.SetParent(attacker.transform);
        sector.transform.position = new Vector3(attacker.position.x, 0.03f, attacker.position.z);
        sector.transform.rotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));

        MeshFilter mf = sector.AddComponent<MeshFilter>();
        MeshRenderer mr = sector.AddComponent<MeshRenderer>();

        Mesh sectorMesh = GenerateSectorMesh(360, spinRadius, 30);

        mf.mesh = sectorMesh;

        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = indicatorColor;
        mr.material = mat;

        GameObject.Destroy(sector, spinDuration + 0.2f);
    }

    private Mesh GenerateSectorMesh(float biteAngle, float biteRadius, int segments)
    {
        Debug.Log("Calculating and generating the indicator mesh!");


        Mesh mesh = new Mesh();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        vertices.Add(Vector3.zero); // Tip of the cone (player's position)

        float halfAngle = biteAngle / 2f;
        float angleRad = biteAngle * Mathf.Deg2Rad;
        float angleStep = angleRad / segments;

        vertices.Add(Vector3.zero);

        for (int i = 0; i <= segments; i++)
        {
            float currentAngle = -halfAngle * Mathf.Deg2Rad + i * angleStep;
            float x = Mathf.Sin(currentAngle) * biteRadius;
            float z = Mathf.Cos(currentAngle) * biteRadius;
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
}
