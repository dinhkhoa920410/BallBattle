using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderRangeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other){
        GetComponentInParent<DefenderScript>().OnRangeTrigger(other);
    }

    void OnTriggerStay(Collider other){
        GetComponentInParent<DefenderScript>().OnRangeTrigger(other);
    }
}
