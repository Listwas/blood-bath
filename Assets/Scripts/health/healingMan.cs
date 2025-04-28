using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NormalHeal{
    public int NormalHealingAmount;
}
[System.Serializable]
public class TimedHeal{
    public int TimedHealingAmountPerTime;
    public float TimeBetweenHeals;
    public int HowManyRepeat;
}
public class healingMan : MonoBehaviour
{
    [SerializeField]private List<NormalHeal> NormalHealsList;
    [SerializeField]private List<TimedHeal> TimedHealsList;

    [SerializeField]private CombatScript cS;
    [SerializeField]private GameEvents events;

    void Awake()
    {
        cS = FindObjectOfType<CombatScript>();
        events = FindObjectOfType<GameEvents>();
    }
//sub do eventu leczacych orbow
    private void OnEnable()
    {
        events.OnHealEnter += findHeal;
    }

    private void OnDisable()
    {
        events.OnHealEnter -= findHeal;
    }

    private void findHeal(string healType, int healIndex){
        if(healType == "normal"){
            doNormalHealing(healIndex);
            Debug.Log("normal healing with" + healIndex + "index");
        }
        else if(healType == "timed"){
            StartCoroutine(doTimedHealing(healIndex));
            Debug.Log("timed healing with" + healIndex + "index good");
        }
    }

    private void doNormalHealing(int healIndex){
        cS.current_health += NormalHealsList[healIndex].NormalHealingAmount;
        if(cS.current_health > cS.max_health){
            cS.current_health = cS.max_health;
        }
    }

    private IEnumerator doTimedHealing(int healIndex){
        for(int i = 1; i <= TimedHealsList[healIndex].HowManyRepeat; i++){
                    cS.current_health += TimedHealsList[healIndex].TimedHealingAmountPerTime;
                    
                    if(cS.current_health > cS.max_health){
                        cS.current_health = cS.max_health;
                    }
                    yield return new WaitForSeconds(TimedHealsList[healIndex].TimeBetweenHeals);
                }
    }
    
}
