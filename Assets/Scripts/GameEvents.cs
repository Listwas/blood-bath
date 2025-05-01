using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public event System.Action<string> OnHealEnter;//event gdy wejdzie sie w healing orb

    public event System.Action<Vector3> OnEnemyDeath;//enemy death event
    public event System.Action OnEnemyHit;//

    public void HealEnter(string healType){
        OnHealEnter?.Invoke(healType);
    }

    public void EnemyDied(Vector3 enemyPosition)
    {
        if (OnEnemyDeath != null)
            OnEnemyDeath.Invoke(enemyPosition);
    }

    public void EnemyHit(){
        OnEnemyHit.Invoke();
    }


}
