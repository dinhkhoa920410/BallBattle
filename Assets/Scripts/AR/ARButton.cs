using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARButton : MonoBehaviour
{
    public bool isOn;
    public Transform onPart;
    public Transform offPart;
    public Transform indicator;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick(){
        isOn = !isOn;
        moveIndicator();
    }

    void moveIndicator(){
        Debug.Log("OldPos: "+indicator.position);
        Debug.Log("OFFPos: "+offPart.position);
        if(isOn == true)
            indicator.position = offPart.position;
        else
            indicator.position = onPart.position;
        Debug.Log("NewPos: "+indicator.position);
    }
}
