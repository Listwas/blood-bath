using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public event System.Action<string, int> OnHealEnter;//event gdy wejdzie sie w healing orb

    public event System.Action<Vector3> OnEnemyDeath;//enemy death event

    public void HealEnter(string healType, int healIndex){
        OnHealEnter?.Invoke(healType, healIndex);
    }

    public void EnemyDied(Vector3 enemyPosition)
    {
        if (OnEnemyDeath != null)
            OnEnemyDeath.Invoke(enemyPosition);
    }


}
