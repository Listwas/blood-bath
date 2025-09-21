using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

[CreateAssetMenu(menuName = "Enemy Attacks/Charge Series Attack")]
public class ChargeSeriesAttackSO : AttackSO
{
    [Header("Charge attack variables")]
    [SerializeField] private float chargeDelay = 1.5f;
    [SerializeField] private float chargeSpeed = 10f;
    //[SerializeField] private float chargeMinDistance = 4f;
    [SerializeField] private float chargeMaxDistance = 10f;
    [SerializeField] private int chargeRepeats = 1;

    [Header("Charge indicator variables")]
    [SerializeField] private Color indicatorColor = Color.red;
    //[SerializeField] private float indicatorDuration = 2f;

    [SerializeField] private Vector3 hitboxSize = new Vector3 (1f, 2f, 1f);

    public override float attackRange => chargeMaxDistance;

    private bool isCharging = false;

    private void OnEnable()
    {
        isCharging = false;
    }

    public override void ExecuteAttack(Transform attacker, Transform target, Transform shootOrigin, LayerMask targetMask, MonoBehaviour context)
    {
        
        //float distanceToPlayer = Vector3.Distance(attacker.position, target.position);
        NavMeshAgent agent = attacker.GetComponent<NavMeshAgent>();
        Debug.Log("ExecuteAttack inside ChargedAttackSO is being called");



        Debug.Log("Starting charge!");

        Debug.Log("current value of isCharging: " + isCharging);
        if(!isCharging)
        {
            Debug.Log("Starting coroutine");
            context.StartCoroutine(ChargeSeriesRoutine(attacker, target, agent, targetMask, context));
        }


    }


    private void DisplayChargeIndicatorStatic(Transform attacker, Vector3 directionToPlayer, float indicatorDuration)
    {
        Renderer renderer = attacker.GetComponentInChildren<Renderer>();
        float width = renderer != null ? renderer.bounds.size.x : 1f;
        

        // Center of the indicator, halfway along the charge distance
        Vector3 center = attacker.position + directionToPlayer * (chargeMaxDistance / 2f);
        center.y = 0f;

        // Create and position the quad
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.position = center + Vector3.up * 0.03f; // Slightly above ground
        quad.transform.rotation = Quaternion.LookRotation(Vector3.down, directionToPlayer); // Rotate to face up, aligned with direction
        quad.transform.localScale = new Vector3(width, chargeMaxDistance, 1f); // height = charge range

        Destroy(quad.GetComponent<Collider>());

        // Set material and color
        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = indicatorColor;
        quad.GetComponent<MeshRenderer>().material = mat;

        // Destroy after duration
        GameObject.Destroy(quad, indicatorDuration);
    }

    private IEnumerator ChargeRoutine(Transform attacker, Transform target, NavMeshAgent agent, float chargeWaitDuration, LayerMask targetMask)
    {
        agent.isStopped = true;
        isCharging = true;


        Vector3 directionToPlayer = (target.position - attacker.position).normalized;
        Vector3 chargeDestination = attacker.position + directionToPlayer * chargeMaxDistance;

        //Debug.Log("DisplayChargeIndicatorStatic called!");
        DisplayChargeIndicatorStatic(attacker, directionToPlayer, chargeWaitDuration);


        yield return new WaitForSeconds(chargeWaitDuration);

        float chargeDistance = Vector3.Distance(attacker.position, chargeDestination);
        float alreadyTravelled = 0f;

        while (alreadyTravelled < chargeDistance)
        {
            float moveStep = chargeSpeed * Time.deltaTime;
            Vector3 move = directionToPlayer * moveStep;

            // Move attacker
            attacker.position += move;
            alreadyTravelled += move.magnitude;

            // Check collision with player
            Collider[] hits = Physics.OverlapBox(attacker.position, hitboxSize / 2f, attacker.rotation, targetMask);
            foreach (var hit in hits)
            {
                if(hit.gameObject.CompareTag("Player"))
                {
                    DealDamage(hit, attacker, damage);
                    break;
                }
                Debug.Log("Hit player!");
                
                
                break;
            }
            yield return null;
        }
        agent.isStopped = false;
        isCharging = false;
    }

    private IEnumerator ChargeSeriesRoutine(Transform attacker, Transform target, NavMeshAgent agent, LayerMask targetMask, MonoBehaviour context)
    {


        for (int i = 0; i < chargeRepeats; i++)
        {
            float chargeSeriesDelay;

            if(chargeRepeats != 1)
            {
                chargeSeriesDelay = 0.5f;
            }
            else
            {
                chargeSeriesDelay = chargeDelay;
            }

            

            while(isCharging)
            {
                yield return null;
            }

            context.StartCoroutine(ChargeRoutine(attacker, target, agent, chargeSeriesDelay, targetMask));

            // Optional short pause between charges
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    /*
    private void GetInPosition(NavMeshAgent agent, Transform target)
    {
        float bestDistance = float.MaxValue;
        Vector3 bestPosition = agent.transform.position;

        const int samplePoints = 16;
        float angleStep = 360f / samplePoints;

        for (int i = 0; i < samplePoints; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

            // Try positions from min to max distance along this direction
            for (float distance = chargeMinDistance; distance <= chargeMaxDistance; distance += 0.5f)
            {
                Vector3 testPos = target.position + direction * distance;

                if (NavMesh.SamplePosition(testPos, out NavMeshHit hit, 0.5f, NavMesh.AllAreas))
                {
                    float distToEnemy = Vector3.Distance(agent.transform.position, hit.position);
                    if (distToEnemy < bestDistance)
                    {
                        bestDistance = distToEnemy;
                        bestPosition = hit.position;
                    }
                    break; // found a valid position in this direction
                }
            }
        }

        agent.SetDestination(bestPosition);
    }
    */
}
