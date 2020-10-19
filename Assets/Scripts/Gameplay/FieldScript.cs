using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldScript : MonoBehaviour
{
    public const int TOP_LEFT = 2;
    public const int TOP_RIGHT = 3;
    public const int BOTTOM_RIGHT = 4;
    public const int BOTTOM_LEFT = 5;

    BoxCollider landTeamA;
    BoxCollider landTeamB;
    void Awake(){
        landTeamA = GameObject.Find("LandTeamA").GetComponent<BoxCollider>();
        landTeamB = GameObject.Find("LandTeamB").GetComponent<BoxCollider>();
    }

    public void init(){
        MeshRenderer[] ObjectsTeamA = transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();
        MeshRenderer[] ObjectsTeamB = transform.GetChild(1).GetComponentsInChildren<MeshRenderer>();
        for(int i=0;i<ObjectsTeamA.Length;i++){
            ObjectsTeamA[i].materials[0].SetColor("_Color", Utility.getColorCode(GameMaster.GM.teamA.color));
            ObjectsTeamB[i].materials[0].SetColor("_Color", Utility.getColorCode(GameMaster.GM.teamB.color));
        }
    }
    public float getWidth(){
        return GetComponent<MeshRenderer>().bounds.size.z;
    }

    public float getLength(){
        return GetComponent<MeshRenderer>().bounds.size.x;
    }


    public Vector3 getLandPosition(int side){
        if(side == Team.TEAM_A)
            return landTeamA.transform.position;
        else
            return landTeamB.transform.position;
    }
    public Vector3 getLandPosition(Team team){
        return getLandPosition(team.side);
    }

    public float getLandWidth(int side){
        if(side == Team.TEAM_A)
            return landTeamA.bounds.size.z;
        else
            return landTeamB.bounds.size.z;
    }
    public float getLandWidth(Team team){
        return getLandWidth(team.side);
    }

    public float getLandLength(int side){
        if(side == Team.TEAM_A)
            return landTeamA.bounds.size.x;
        else
            return landTeamB.bounds.size.x;
    }
    public float getLandLength(Team team){
        return getLandLength(team.side);
    }
    public Vector3 getCornerPostion(int corner){
        Vector3 position;
        switch(corner){
            case TOP_LEFT: 
                position = transform.GetChild(TOP_LEFT).position;
                break;
            case TOP_RIGHT: 
                position = transform.GetChild(TOP_RIGHT).position;
                break;
            case BOTTOM_LEFT: 
                position = transform.GetChild(BOTTOM_LEFT).position;
                break;
            case BOTTOM_RIGHT: 
                position = transform.GetChild(BOTTOM_RIGHT).position;
                break;
            default:
                position = Vector3.zero;
                break;
        }
        return position;
    }
}
