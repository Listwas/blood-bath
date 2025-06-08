using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Attacks/Attack Parameters/Scratch Parameters")]
public class ScratchSeriesParametersSO : ScriptableObject
{
    [Tooltip("Damage of the scratch")]
    public int damage = 10;

    [Tooltip("Angle in degrees of the scratch cone.")]
    public float angle = 60f;

    [Tooltip("Radius (length) of the scratch cone.")]
    public float radius = 2f;

    [Tooltip("Distance of the lunge before the attack (set 0 if you dont want to lunge)")]
    public float lungeDistance = 0f;

    [Tooltip("Duration of the lunge")]
    public float lungeDuration = 0.1f;
}