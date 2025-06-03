using UnityEngine;
using Extensions;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;
using static UnityEngine.UI.Image;

[CreateAssetMenu(menuName = "EnemyAttacks/BiteAttack")]
public class BiteAttackSO : AttackSO
{
    [Header("Bite indicator values")]
    [SerializeField] private float biteAngle = 60f;
    [SerializeField] private float biteRadius = 2f;
    [SerializeField] private float biteWaitDuration = 1f;
    [SerializeField] private float biteTriggerRange = 3f;
    [SerializeField] private Color indicatorColor = Color.red;

    public override float attackRange => biteTriggerRange;

    public override void ExecuteAttack(Transform attacker, Transform target, Transform origin, LayerMask targetMask, MonoBehaviour context)
    {
        Debug.Log("ExecuteAttack of BiteSO called!");
        if(Vector3.Distance(attacker.position, target.position) < biteTriggerRange)
        {
            Vector3 directionToPlayer = target.position - attacker.position;

            NavMeshAgent agent = attacker.GetComponent<NavMeshAgent>();

            directionToPlayer.y = 0;

            if (directionToPlayer != Vector3.zero)
            {
                context.StartCoroutine(BiteRoutine(attacker, target, agent, targetMask));

                /*
                //Debug.Log("attacking");
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                attacker.rotation = Quaternion.Slerp(attacker.rotation, lookRotation, Time.deltaTime * 5f);

                Vector3 boxCenter = origin.position + attacker.forward * hitboxOffset.z + Vector3.up * hitboxOffset.y;

                Debug.Log("DisplayBiteIndicatorStatic called!");
                DisplayBiteIndicatorStatic(attacker, target, directionToPlayer);

                // Check if the player is inside the hitbox
                Collider[] hits = Physics.OverlapBox(boxCenter, hitboxSize, attacker.rotation, targetMask);
                foreach (var hit in hits)
                {
                    //Debug.Log($"Hit object: {hit.name} on layer {LayerMask.LayerToName(hit.gameObject.layer)}");

                    if (hit.gameObject.CompareTag("Player"))
                    {
                        //Debug.Log("Bite hit the player with a hitbox!");
                        DealDamage(hit, attacker);
                        break;
                        // You can call TakeDamage() here if needed
                    }
                }
                */
            }
        }
    }

    private IEnumerator BiteRoutine(Transform attacker, Transform target, NavMeshAgent agent, LayerMask targetMask)
    {
        agent.isStopped = true;

        Vector3 directionToPlayer = (target.position - attacker.position).normalized;

        Debug.Log("DisplayBiteIndicator called!");
        DisplayBiteIndicatorStatic(attacker, target, directionToPlayer);

        yield return new WaitForSeconds(biteWaitDuration);

        Collider[] hits = Physics.OverlapSphere(attacker.position, biteRadius, targetMask);

        foreach(var hit in hits)
        {
            Debug.Log($"Hit: {hit.name}");
            Vector3 toTarget = (hit.transform.position - attacker.position).normalized;

            float angleToTarget = Vector3.Angle(directionToPlayer, toTarget);

            if (angleToTarget <= biteAngle / 2f)
            {
                DealDamage(hit, attacker);
                Debug.Log($"Hit: {hit.name} damage");
                break;
            }
        }

        agent.isStopped = false;
        yield return null;
    }

    private void DisplayBiteIndicatorStatic(Transform attacker, Transform target, Vector3 directionToPlayer)
    {
        Debug.Log("Figuring out the bite indicator!");

        GameObject sector = new GameObject("BiteAttackIndicator");
        sector.transform.position = new Vector3(attacker.position.x, 0.03f, attacker.position.z);
        sector.transform.rotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));

        MeshFilter mf = sector.AddComponent<MeshFilter>();
        MeshRenderer mr = sector.AddComponent<MeshRenderer>();

        Mesh sectorMesh = GenerateSectorMesh(biteAngle, biteRadius, 30);

        mf.mesh = sectorMesh;

        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = indicatorColor;
        mr.material = mat;

        GameObject.Destroy(sector, biteWaitDuration + 0.01f);
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
