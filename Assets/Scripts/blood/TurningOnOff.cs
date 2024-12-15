using UnityEngine;
using UnityEngine.UI;

public class TurningOnOff : MonoBehaviour
{

    public bloodCount bCScript;

    public RawImage[] bloodImage;
    
    void Start()
    {
        bCScript = FindObjectOfType<bloodCount>();
    }

    void Update(){
        for (int i =0; i < bloodImage.Length; i++){
            if(i < bCScript.bloodOnScreenNum){
                bloodImage[i].enabled = true;
            }
            else{
                bloodImage[i].enabled = false;
            }
            
        }
    }


}
