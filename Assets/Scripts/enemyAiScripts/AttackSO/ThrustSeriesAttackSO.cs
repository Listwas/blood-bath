using Extensions;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName =("EnemyAttacks/ThrustSeriesSO"))]
public class ThrustsAttackSO : AttackSO
{
    [Header("Thrusts variables")]
    //[SerializeField] private float thrustsRange = 3.5f;
    [SerializeField] private int thrustsRepeats = 3;
    [SerializeField] private float thrustsInitialDelay = 0.75f; //delay before the first thrust
    [SerializeField] private float thrustsMidDelay = 0.25f; //delay between the thrusts in the series
    [SerializeField] private Vector3 thrustsHitboxSize = new Vector3(0.5f, 0.5f, 2.5f);

    [Header("Thrusts indicator variables")]
    [SerializeField] private Color indicatorColor = Color.red;

    public override float attackRange => thrustsHitboxSize.z;

    public override void ExecuteAttack(Transform attacker, Transform target, Transform shootOrigin, LayerMask targetMask, MonoBehaviour context)
    {
        NavMeshAgent agent = attacker.GetComponent<NavMeshAgent>();

        context.StartCoroutine(MultipleThrustsRoutine(attacker, target, shootOrigin, agent, targetMask, context));

    }

    private IEnumerator MultipleThrustsRoutine(Transform attacker, Transform target, Transform shootOrigin, NavMeshAgent agent, LayerMask targetMask, MonoBehaviour context)
    {
        agent.isStopped = true;

        Vector3 directionToPlayer = (target.position - attacker.position).normalized;
        Vector3 directionToPlayerRight = Quaternion.Euler(0, 30f, 0) * directionToPlayer;
        Vector3 directionToPlayerLeft = Quaternion.Euler(0, -30f, 0) * directionToPlayer;

        for (int i = 0; i < thrustsRepeats; i++)
        {
            if(i == 0)
            {
                yield return context.StartCoroutine(SingleThrustRoutine(attacker, directionToPlayer, shootOrigin, targetMask, thrustsInitialDelay));
            }
            else
            {
                if (i % 3 == 0)
                {
                    yield return context.StartCoroutine(SingleThrustRoutine(attacker, directionToPlayer, shootOrigin, targetMask, thrustsMidDelay));
                }
                else if (i % 3 == 1)
                {
                    yield return context.StartCoroutine(SingleThrustRoutine(attacker, directionToPlayerRight, shootOrigin, targetMask, thrustsMidDelay));
                }
                else if (i % 3 == 2)
                {
                    yield return context.StartCoroutine(SingleThrustRoutine(attacker, directionToPlayerLeft, shootOrigin, targetMask, thrustsMidDelay));
                }
            }
        }

        agent.isStopped = false;
    }

    private IEnumerator SingleThrustRoutine(Transform attacker, Vector3 directionToPlayer, Transform shootOrigin, LayerMask targetMask, float thrustsDelay)
    {
        DisplayThurstsIndicatorStatic(attacker, directionToPlayer, thrustsDelay);

        yield return new WaitForSeconds(thrustsDelay);

        Vector3 boxCenter = attacker.position + directionToPlayer * (thrustsHitboxSize.z / 2f);
        boxCenter.y = shootOrigin.position.y;

        Quaternion rotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0f, directionToPlayer.z));

        Collider[] hits = Physics.OverlapBox(boxCenter, thrustsHitboxSize, rotation, targetMask);

        DrawHitboxScript.DebugDrawBox(boxCenter, thrustsHitboxSize, rotation, Color.red, 2f);

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                DealDamage(hit, attacker);
                break;
            }
        }

        yield return new WaitForSeconds(0.1f);
    }

    private void DisplayThurstsIndicatorStatic(Transform attacker, Vector3 directionToPlayer, float indicatorDuration)
    {
        Renderer renderer = attacker.GetComponentInChildren<Renderer>();
        float width = thrustsHitboxSize.x;
        float length = thrustsHitboxSize.z;

        Vector3 center = attacker.position + directionToPlayer * (thrustsHitboxSize.z / 2);
        center.y = 0f;

        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.position = center + Vector3.up * 0.03f;
        quad.transform.rotation = Quaternion.LookRotation(Vector3.down, directionToPlayer);
        quad.transform.localScale = new Vector3(width, length, 1f);

        Destroy(quad.GetComponent<Collider>());

        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = indicatorColor;
        quad.GetComponent<MeshRenderer>().material = mat;

        GameObject.Destroy(quad, indicatorDuration);
    }
}
