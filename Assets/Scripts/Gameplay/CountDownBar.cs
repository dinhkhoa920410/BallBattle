using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownBar : MonoBehaviour
{
    public Slider inactiveBar;
    float remainingTime=0;
    
    void Update()
    {
        //Set to a Fixed rotation
        transform.rotation = Quaternion.LookRotation(Vector3.up);
        if(remainingTime>0){
            remainingTime -= Time.deltaTime;
            inactiveBar.value = remainingTime/3.5f;
        }
    }

    public void start(float time){
        this.remainingTime = time;
    }
}
