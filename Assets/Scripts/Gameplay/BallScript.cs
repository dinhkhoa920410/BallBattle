using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : ObjectScript
{
    const float BALL_SPEED = 1.5f;
    FieldScript field;
    private GameObject ballModel;
    private Transform target;
    // Start is called before the first frame update
    void Awake(){
        field = GameObject.Find("Field").GetComponent<FieldScript>();
    }

    void Start()
    {
        speed = BALL_SPEED;
        isActive = true;
        ballModel = transform.GetComponentInChildren<SphereCollider>().gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isActive && target != null){
            moveToward(target);
        }
    }

    public void setTarget(GameObject target){
        this.target = target.transform;
    }

    public void setBallActive(bool isActive){
        ballModel.SetActive(isActive);
        this.isActive = isActive;
    }

    public float getBallRadius(){
        return ballModel.GetComponent<MeshRenderer>().bounds.size.x/2;
    }

    void randomReposition(){
        Vector3 newPosition;
        float minX,maxX,minZ,maxZ;
        minX = field.getCornerPostion(FieldScript.TOP_LEFT).x;
        maxX = field.getCornerPostion(FieldScript.TOP_RIGHT).x;
        if(GameMaster.GM.teamA.role == Team.ROLE_ATTACKER){
            minZ = field.getCornerPostion(FieldScript.BOTTOM_LEFT).z + getBallRadius();
            maxZ = (field.getCornerPostion(FieldScript.TOP_LEFT) + field.getCornerPostion(FieldScript.BOTTOM_LEFT)).z/2 - getBallRadius();
        }
        else{
            minZ = (field.getCornerPostion(FieldScript.TOP_LEFT) + field.getCornerPostion(FieldScript.BOTTOM_LEFT)).z/2 + getBallRadius();
            maxZ = field.getCornerPostion(FieldScript.TOP_LEFT).z - getBallRadius();
        }
        newPosition = new Vector3(Random.Range(minX, maxX), transform.position.y, Random.Range(minZ, maxZ));
        transform.position = newPosition;
    }
    public void initNewRound(){
        setBallActive(true);
        target = null;
        randomReposition();
    }

    public override void moveToward(Vector3 target){
        base.moveToward(target);

        Vector3 movingDirection = target - transform.position;
        Vector3 rotateDirection = new Vector3(movingDirection.z, 0, -movingDirection.x).normalized;
        transform.Rotate(rotateDirection*5);
    }
}
