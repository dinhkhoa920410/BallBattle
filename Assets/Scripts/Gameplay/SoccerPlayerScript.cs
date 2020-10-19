using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerPlayerScript : ObjectScript
{
    public GameObject[] clothes;
    public float reactiveTime;
    public float spawnTime;
    public int color;
    public int team;
    public CountDownBar countDownBar;
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void setPlayerActive(bool isActive){
        this.isActive = isActive;
        setGreyScale(!isActive);
        if(isActive == false)
            countDownBar.start(reactiveTime);
    }
    public virtual IEnumerator setActiveWithDelayTime(bool isActive, float delayTime){
        yield return new WaitForSeconds(delayTime);
        setPlayerActive(isActive);
    }
    public bool isPlayerActive(){
        return isActive;
    }

    public void applyColor(int color){
        //Material playerMaterial = GetComponent<MeshRenderer>().materials[0];
        for(int i=0; i<clothes.Length; i++){
            clothes[i].GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Utility.getColorCode(color));
        }
        //playerMaterial.SetColor("_Color", Utility.getColorCode(color));
    }

    public void initPlayer(Team team){
        this.team = team.side;
        color = team.color;
        applyColor(color);
    }

    public void setGreyScale(float rate){
        Material playerMaterial = GetComponent<MeshRenderer>().materials[0];
    }

    public void setGreyScale(bool isGrayscale){
        if(isGrayscale)
            for(int i=0; i<clothes.Length; i++){
                clothes[i].GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Color.grey);
            }
        else
            applyColor(color);
    }

    public void setArrowActive(bool isActive){
        transform.GetChild(1).gameObject.SetActive(isActive);
    }

    public override void moveToward(Vector3 target){
        base.moveToward(target);
        
        Vector3 movingDirection = target - transform.position;
        //only rotate around axis-Y
        movingDirection.y = 0;
        //rotate the facing direction to target
        transform.rotation = Quaternion.LookRotation(movingDirection);
    }
}
