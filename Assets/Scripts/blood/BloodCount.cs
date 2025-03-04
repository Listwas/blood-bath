using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCount : MonoBehaviour
{
    [Header("Blood Counting")]
    public int playerBloodPoints = 0; // ile krwi ma na sobie gracz
    public int addBloodPoints = 5;    // ile krwi dodane po wejsciu w plame krwi
    public float DMGMulti = 1.0f;     // mnożnik dmg
    public int bloodOnScreenNum = 0;  // ile img krwi na ekranie

    [Header("Fear Mode")]
    public int bloodPointsFearMode = 50; // jak dużo krwi gracz musi mieć do osiągnięcia fear mode
    public int fearModeTime = 5;
    [Header("When reset")]
    public int resetDMGMulti = 4;
    public int resetBloodPoints = 100;

    public void bloodLogic()
    {
        addBlood();
        addDMGMulti();
    }

    // zwiększa liczbe pkt krwi
    public void addBlood()
    {
        if (playerBloodPoints < bloodPointsFearMode - addBloodPoints) {
            playerBloodPoints += addBloodPoints;
            Debug.Log($"Blood points: {playerBloodPoints}");
        } else {
            StartCoroutine(fearMode());
        }
    }

    // zwiekszanie mnożnika dmg
    public void addDMGMulti()
    {
        if (playerBloodPoints % 5 == 0) {
            Debug.Log("Add dmg");
            DMGMulti += 0.1f;
            bloodOnScreenNum += 1;
            Reset_DMGMulti();
        }
    }

    // fear mode czyli enemies boją sie mekka i wiecej dmg do ataków
    public IEnumerator fearMode()
    {
        DMGMulti *= 2;
        Debug.Log($"Fear Mode, DMG multi {DMGMulti:F1}");
        yield return new WaitForSeconds(fearModeTime);
        DMGMulti /= 2;
        Debug.Log($"Fear Mode, DMG multi{DMGMulti:F1}");
    }

    // resety
    public void Reset_DMGMulti()
    {
        if (DMGMulti >= resetDMGMulti) {
            DMGMulti = 1.0f;
        }
    }
    
    public void Reset_BloodPoints()
    {
        if (playerBloodPoints >= resetBloodPoints) {
            playerBloodPoints = 0;
        }
    }
}
