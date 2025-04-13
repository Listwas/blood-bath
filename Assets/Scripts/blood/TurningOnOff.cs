using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TurningOnOff : MonoBehaviour
{

    [SerializeField] private bloodCount bCScript;

    [SerializeField] private RawImage[] bloodImage;

    private int rand;

    [SerializeField] private int bloodyScreenChance;


    private int bloodIndex = 0;
    
    void Awake()
    {
        bCScript = FindObjectOfType<bloodCount>();
        WipeScreen();

    }

    void Update(){
        /*for (int i =0; i < bloodImage.Length; i++){
            if(i < bCScript.bloodOnScreenNum){
                bloodImage[i].enabled = true;
            }
            else{
                bloodImage[i].enabled = false;
            }
        }*/
        if(Input.GetKeyDown("l")){
            WipeScreen();}
    }

     private void OnEnable()
    {
        bCScript.OnHitBlood += showBloodOnScreen;
    }

    private void OnDisable()
    {
        bCScript.OnHitBlood -= showBloodOnScreen;
    }



    private void showBloodOnScreen(){
        rand = Random.Range(1, 101);
        if(rand <= bloodyScreenChance){
           bloodImage[bloodIndex].enabled = true;
           bloodIndex += 1;
        }
    }


    private void WipeScreen(){
          foreach (RawImage img in bloodImage)
            {
                img.enabled = false;
            }  
    }

}
