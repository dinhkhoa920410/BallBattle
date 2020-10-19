using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorModel : MonoBehaviour
{
    public int color;
    public int team;
    public GameObject[] clothes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void applyColor(int color){
        for(int i=0; i<clothes.Length; i++){
            clothes[i].GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", Utility.getColorCode(color));
        }
    }

    public void initModel(){
        if(team == GameMaster.GM.teamA.side)
            color = GameMaster.GM.teamA.color;
        else
            color = GameMaster.GM.teamB.color;
        applyColor(color);
    }
}
