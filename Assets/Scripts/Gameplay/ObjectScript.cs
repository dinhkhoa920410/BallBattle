using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public float speed;
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public virtual void moveToward(Vector3 target){
        //move position
        target = new Vector3(target.x, transform.position.y, target.z);
        transform.position = Vector3.MoveTowards(transform.position, target, speed*Time.fixedDeltaTime);
    }
    public void moveToward(Transform target){
        moveToward(target.position);
    }
    public void moveToward(GameObject target){
        moveToward(target.transform.position);
    }


}
