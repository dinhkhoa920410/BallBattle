using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerScript : SoccerPlayerScript
{
    const float NORMAL_SPEED = 1.5f;
    const float CARRYING_SPEED = 0.75f;
    const float REACTIVATE_TIME = 2.5f;
    const float SPAWN_TIME = 0.5f;
    public Transform goalTeamA;
    public Transform goalTeamB;
    public BallScript ball;
    public Transform attackerList;
    public bool isKeepingBall;
    public bool isInAnimation;
    FieldScript field;
    

    // Start is called before the first frame update
    void Start()
    {
        isInAnimation = false;
        //find object
        goalTeamA = GameObject.Find("GoalTeamA").transform;
        goalTeamB = GameObject.Find("GoalTeamB").transform;
        ball = GameObject.Find("Ball").GetComponent<BallScript>();
        attackerList = GameObject.Find("AttackerList").transform;
        field = GameObject.Find("Field").GetComponent<FieldScript>();
        //////
        speed = NORMAL_SPEED;
        spawnTime = SPAWN_TIME;
        reactiveTime = REACTIVATE_TIME;
        isKeepingBall = false;
        //Using coroutine to make an delay action
        StartCoroutine(setActiveWithDelayTime(true, spawnTime));
    }

    // Update is called once per frame
    void Update()
    {
        if(GameMaster.GM.isRushTime)
            if(speed<NORMAL_SPEED)
                speed = NORMAL_SPEED;
    }

    void FixedUpdate(){
        if(!GameMaster.GM.isPause){
            if(!isInAnimation){
                if(isPlayerActive()){
                    if(ball.isActive)
                        chasingBall();
                    else if(isKeepingBall)
                        moveToGoal();
                    else
                        moveForward();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other){
        switch(other.tag){
            case "Ball": 
                isKeepingBall = true;
                speed = CARRYING_SPEED;
                ball.setBallActive(false);
                setHighLight(true);
                break;
                
            case "Defender":
                if(isKeepingBall){
                    if(other.GetComponent<DefenderScript>().isActive){
                        GetComponentInChildren<Animator>().SetTrigger("Hurt");
                        passBall();
                        speed = NORMAL_SPEED;
                        setPlayerActive(false);
                        StartCoroutine(setActiveWithDelayTime(true, reactiveTime));
                    }
                }
                break;

            case "Wall":
                die();
                break;

            case "Goal":
                if(isKeepingBall)
                    GameMaster.GM.endRound(GameMaster.GM.teamList[team]);
                break;
        }
    }
    void OnTriggerStay(Collider other){
        if(other.CompareTag("Defender")){
            if(isKeepingBall){
                if(other.GetComponent<DefenderScript>().isActive){
                    GetComponentInChildren<Animator>().SetTrigger("Hurt");
                    passBall();
                    speed = NORMAL_SPEED;
                    setPlayerActive(false);
                    StartCoroutine(setActiveWithDelayTime(true, reactiveTime));
                }
            }
        }
    }

    void init(){

    }

    void chasingBall(){
        moveToward(ball.gameObject);
    }

    void moveToGoal(){
        if(team == Team.TEAM_A){
            moveToward(goalTeamB);
        }
        else if(team == Team.TEAM_B)
            moveToward(goalTeamA);
    }

    GameObject findNearestAvailaleTeammate(){
        if(attackerList.childCount>1){
            int index=0;
            float minDistance = field.getLength()*2;

            for(int i=index;i<attackerList.childCount;i++){
                //check active
                if(attackerList.GetChild(i).GetComponent<AttackerScript>().isActive){
                    float distance = (transform.position - attackerList.GetChild(i).transform.position).magnitude;
                    if(distance<minDistance && distance!=0){
                        minDistance = distance;
                        index=i;
                    }
                }
            }
            return (minDistance<field.getLength()*2) ? attackerList.GetChild(index).gameObject : null;
        }
        else
            return null;
    }

    Vector3 getBallRespawnPosition(GameObject teammate){
            Vector3 directionToTeamate = teammate.transform.position - transform.position;
            float offsetPlayerToBall = GetComponent<CapsuleCollider>().bounds.size.x/2 + ball.getBallRadius();
            Vector3 respawnPosition = transform.position + directionToTeamate.normalized*offsetPlayerToBall;

            return new Vector3(respawnPosition.x, ball.transform.position.y, respawnPosition.z);
    }
    void passBall(){
        if(findNearestAvailaleTeammate() != null){
            ball.setBallActive(true);
            ball.transform.position = getBallRespawnPosition(findNearestAvailaleTeammate());
            ball.setTarget(findNearestAvailaleTeammate());
            isKeepingBall = false;
        }else{
            GameMaster.GM.endRound(GameMaster.GM.teamList[(team+1)%2]);
        }
    }

    void moveForward(){
        Vector3 movingDestination;
        Vector3 movingDirection;
        if(team == Team.TEAM_A){
            movingDirection = (goalTeamB.position - field.transform.position).normalized;        
        }
        else{
            movingDirection = (goalTeamA.position - field.transform.position).normalized;
        }
        movingDestination = transform.position + new Vector3(movingDirection.x, transform.position.y, movingDirection.z);
        moveToward(movingDestination);
    }

    void die(){
        isActive = false;
        transform.GetChild(3).gameObject.SetActive(true);
        GetComponentInChildren<Animator>().SetTrigger("Death");
    }

    void setHighLight(bool isActive){
        transform.GetChild(0).gameObject.SetActive(isActive);
    }

    /*public override IEnumerator setActiveWithDelayTime(bool isActive, float delayTime){
        yield return new WaitForSeconds(delayTime);
        setArrowActive(isActive);
    }*/

    public override void setPlayerActive(bool isActive){
        base.setPlayerActive(isActive);
        setArrowActive(isActive);
        GetComponentInChildren<Animator>().SetBool("isActive", isActive);
        if(isActive == false)
            setHighLight(false);
    }
}
