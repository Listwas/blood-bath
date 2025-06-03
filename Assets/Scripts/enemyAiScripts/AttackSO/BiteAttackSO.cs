using UnityEngine;
using Extensions;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

[CreateAssetMenu(menuName = "EnemyAttacks/BiteAttack")]
public class BiteAttackSO : AttackSO
{
    [SerializeField] private Vector3 hitboxSize = new Vector3(2f, 1f, 2f);
    [SerializeField] private Vector3 hitboxOffset = new Vector3(0, 0.5f, 1f);

    public override float attackRange => hitboxSize.x - hitboxOffset.x;

    public override void ExecuteAttack(Transform attacker, Transform target, Transform origin, LayerMask targetMask, MonoBehaviour context)
    {
        Debug.Log("ExecuteAttack of BiteSO called!");
        if(Vector3.Distance(attacker.position, target.position) < 2f)
        {
            Vector3 directionToPlayer = target.position - attacker.position;

            directionToPlayer.y = 0;

            if (directionToPlayer != Vector3.zero)
            {
                //Debug.Log("attacking");
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                attacker.rotation = Quaternion.Slerp(attacker.rotation, lookRotation, Time.deltaTime * 5f);

                Vector3 boxCenter = origin.position + attacker.forward * hitboxOffset.z + Vector3.up * hitboxOffset.y;

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

                DrawHitboxScript.DebugDrawBox(boxCenter, hitboxSize, attacker.rotation, Color.red, 1f);
            }
        }
    }
}
