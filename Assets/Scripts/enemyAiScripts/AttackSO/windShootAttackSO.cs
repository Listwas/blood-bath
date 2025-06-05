using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder;

[CreateAssetMenu(menuName ="EnemyAttacks/WindProjectileAttack")]
public class WindShootAttackSO : AttackSO
{
    [Header("Basic projectile settings")]
    [SerializeField] private float projectileRange = 15f;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Sinusoid path adjustments")]
    [SerializeField] private float sinusoidFrequency = 5f;
    [SerializeField] private float sinusoidAmplitude = 0.5f;

    public override float attackRange => projectileRange;

    public override bool isRanged => true;

    [Header("Projectile Path Type")]
    [SerializeField ]private ProjectilePathType projectilePathType;

    public override void ExecuteAttack(Transform attacker, Transform target, Transform shootOriginPoint, LayerMask targetMask, MonoBehaviour context)
    {
        Vector3 directionToPlayer = (target.position - attacker.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, shootOriginPoint.position, Quaternion.identity);
        GameObject bullet2 = null;
        
        if(projectilePathType == ProjectilePathType.SinusoidDouble)
        {
            bullet2 = Instantiate(bulletPrefab, shootOriginPoint.position, Quaternion.identity);
        }
        else
        {
            Destroy(bullet2);
        }

        context.StartCoroutine(MoveProjectile(bullet, bullet2, directionToPlayer, shootOriginPoint, context));
    }

    private IEnumerator MoveProjectile(GameObject bullet, GameObject bullet2, Vector3 directionToPlayer, Transform shootOriginPoint, MonoBehaviour context)
    {
        
        switch (projectilePathType)
        {
            case ProjectilePathType.Staright:
                yield return StraightPath(bullet, directionToPlayer);
                break;
            case ProjectilePathType.SinusoidSingle:
                yield return SinusoidPath(bullet, directionToPlayer, shootOriginPoint, true);
                break;
            case ProjectilePathType.SinusoidDouble:
                context.StartCoroutine(SinusoidPath(bullet, directionToPlayer, shootOriginPoint, true));
                context.StartCoroutine(SinusoidPath(bullet2, directionToPlayer, shootOriginPoint, false));
                yield return null;
                break;
            //case ProjectilePathType.Spiral:
                //yield return SpiralPath(bullet, directionToPlayer);
                //break;
        }
    }

    private IEnumerator StraightPath(GameObject bullet, Vector3 directionToPlayer)
    {
        float distanceTravelled = 0f;

        while (distanceTravelled < projectileRange)
        {
            float moveStep = projectileSpeed * Time.deltaTime;
            bullet.transform.position += directionToPlayer * moveStep;
            distanceTravelled += moveStep;

            yield return null;
        }

        GameObject.Destroy(bullet);
    
    }
    
    private IEnumerator SinusoidPath(GameObject bullet, Vector3 directionToPlayer, Transform shootOriginPoint, bool isRight)
    {
        float currentTime = 0f;

        while (Vector3.Distance(shootOriginPoint.position, bullet.transform.position) < projectileRange)
        {
            float step = projectileSpeed * Time.deltaTime;
            currentTime += Time.deltaTime;

            // Forward movement
            Vector3 forward = directionToPlayer.normalized * step;

            // Sine offset perpendicular to movement direction
            Vector3 right;
            if(isRight)
            {
                right = Vector3.Cross(Vector3.up, directionToPlayer.normalized);
            }
            else
            {
                right = Vector3.Cross(Vector3.up, -directionToPlayer.normalized);
            }
            float sineOffset = Mathf.Sin(currentTime * sinusoidFrequency) * sinusoidAmplitude;

            bullet.transform.position += forward + right * (sineOffset - Mathf.Sin((currentTime - Time.deltaTime) * sinusoidFrequency) * sinusoidAmplitude);

            yield return null;
        }

        GameObject.Destroy(bullet);
    }

    /*
    private IEnumerator SpiralPath(GameObject bullet, Vector3 directionToPlayer)
    {

    }
    */

    private enum ProjectilePathType
    {
        Staright,
        SinusoidSingle,
        SinusoidDouble,
        //Spiral,
    }
}
