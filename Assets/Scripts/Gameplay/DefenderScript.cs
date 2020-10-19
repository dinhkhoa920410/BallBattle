using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderScript : SoccerPlayerScript
{
    const float NORMAL_SPEED = 1.0f;
    const float RETURN_SPEED = 2.0f;
    const float REACTIVATE_TIME = 4.0f;
    const float SPAWN_TIME = 0.5f;
    const float DETECTION_RANGE_SCALE = 0.35f;
    FieldScript field;
    float range;
    Vector3 originalPosition;
    Transform rangeObject;
    public Transform target;
    public bool isInAnimation;
    // Start is called before the first frame update
    void Start()
    {
        isInAnimation = true;
        field = GameObject.Find("Field").GetComponent<FieldScript>();
        speed = NORMAL_SPEED;
        spawnTime = SPAWN_TIME;
        reactiveTime = REACTIVATE_TIME;
        range = field.getWidth() * DETECTION_RANGE_SCALE;
        originalPosition = transform.position;
        //Using coroutine to make an delay action
        StartCoroutine(setActiveWithDelayTime(true, spawnTime));
        
        //get transform of range object
        rangeObject = transform.GetChild(0).transform;
        rangeObject.localScale *= transferRangeToScale(range);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!GameMaster.GM.isPause){
            if(!isInAnimation){
                if(target != null){
                    if(isAttackerKeepingBall(target.gameObject)){
                        moveToward(target);
                    }
                    else{
                        target = null;
                        speed = RETURN_SPEED;
                        StartCoroutine(initForNextStandby(true, backToOriginalPosTime()));
                        StartCoroutine(hideArrowAtOrigin(backToOriginalPosTime()));
                    }
                }
                else if(!isAtOriginalPosision()){
                    moveToward(originalPosition);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Attacker")){
            if(target != null){
                if(other.gameObject == target.gameObject){
                    isInAnimation = true;
                    GetComponentInChildren<Animator>().SetTrigger("Kick");
                    StartCoroutine(delay1FrameSetDeactive());
                    target = null;
                }
            }
        }
    }
    void OnTriggerStay(Collider other){
        if(other.CompareTag("Attacker")){
            if(target != null){
                if(other.gameObject == target.gameObject){
                    GetComponentInChildren<Animator>().SetTrigger("Kick");
                    if(GameMaster.GM.isRushTime)
                        deActiveInRushTime();
                    else
                        deactive();
                    target = null;
                }
            }
        }
    }

    public void OnRangeTrigger(Collider other){
        if(other.CompareTag("Attacker")){
            if(isAttackerKeepingBall(other.gameObject)){
                chaseBallKeeper(other.gameObject);
                setArrowActive(true);
                GetComponentInChildren<Animator>().SetBool("isRunning", true);
            }
        }
    }
    IEnumerator delay1FrameSetDeactive(){
        yield return new WaitForFixedUpdate();
        if(GameMaster.GM.isRushTime)
            deActiveInRushTime();
        else
            deactive();
    }
    
    float transferRangeToScale(float range){
        //float selfWidth = GetComponent<MeshRenderer>().bounds.size.x;
        float selfWidth = GetComponent<CapsuleCollider>().bounds.size.x;
        return range/selfWidth;
    }

    void chaseBallKeeper(GameObject ballKeeper){
        target = ballKeeper.transform;
        rangeObject.gameObject.SetActive(false);
    }

    public void deactive(){
        setPlayerActive(false);
        GetComponentInChildren<Animator>().SetBool("isActive", false);
        speed = RETURN_SPEED;
        StartCoroutine(initForNextStandby(true, reactiveTime));
        StartCoroutine(hideArrowAtOrigin(backToOriginalPosTime()));
        
    }

    IEnumerator initForNextStandby(bool isActive, float delayTime){
        yield return new WaitForSeconds(delayTime);
        setPlayerActive(isActive);
        GetComponentInChildren<Animator>().SetBool("isActive", true);
        speed = NORMAL_SPEED;
        rangeObject.gameObject.SetActive(true);
    }
    public override IEnumerator setActiveWithDelayTime(bool isActive, float delayTime){
        yield return base.setActiveWithDelayTime(isActive, delayTime);
        GetComponentInChildren<Animator>().SetBool("isActive", true);
        rangeObject.gameObject.SetActive(true);
    }

    bool isAtOriginalPosision(){
        return transform.position == originalPosition;
    }
    
    float backToOriginalPosTime(){
        float distance = (transform.position - originalPosition).magnitude;
        return distance/speed;
    }

    bool isAttackerKeepingBall(GameObject attacker){
        return attacker.GetComponent<AttackerScript>().isKeepingBall;
    }

    IEnumerator hideArrowAtOrigin(float delayTime){
        yield return new WaitForSeconds(delayTime);
        setArrowActive(false);
        GetComponentInChildren<Animator>().SetBool("isRunning", false);
    }

    void deActiveInRushTime(){
        isActive = false;
        setGreyScale(!isActive);
        speed = RETURN_SPEED;
        countDownBar.start(backToOriginalPosTime()+1);
        StartCoroutine(initForNextStandby(true, backToOriginalPosTime()+1));
        StartCoroutine(hideArrowAtOrigin(backToOriginalPosTime()+1));
    }
}
