using UnityEngine;
using UnityEngine.UI;

public class TurningOnOff : MonoBehaviour
{
    public BloodCount bCScript;
    public RawImage[] bloodImage;

    void Start() { bCScript = FindObjectOfType<BloodCount>(); }

    void Update()
    {
        for (int i = 0; i < bloodImage.Length; i++) {
            if (i < bCScript.bloodOnScreenNum) {
                bloodImage[i].enabled = true;
            } else {
                bloodImage[i].enabled = false;
            }
        }
    }
}
