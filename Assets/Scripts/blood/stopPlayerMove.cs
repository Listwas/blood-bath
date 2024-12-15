using System.Collections;
using UnityEngine;

public class stopPlayerMove : MonoBehaviour
{
    private PlayerEvade evade;
    private ThirdPersonMovement move;
    public int blockMovementTime = 5;
    public float blockMovementDelay = 0.2f; //może być bez
    void Start()
    {
        evade = FindObjectOfType<PlayerEvade>();
        move = FindObjectOfType<ThirdPersonMovement>();
    }

    public IEnumerator moveBlock()
    {
        if(evade != null & move != null)
        {
            yield return new WaitForSeconds(blockMovementDelay);
            stopMove();
            yield return new WaitForSeconds(blockMovementTime);
            startMoveAgain();
        }

    }
    public void stopMove()
    {
        Debug.Log("Stop Move");
        evade.enabled = false;
        move.enabled = false;
    }

    public void startMoveAgain()
    {
        Debug.Log("Start Move");
        evade.enabled = true;
        move.enabled = true;
    }
}
