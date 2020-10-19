using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChangeController : MonoBehaviour
{
    public ColorModel modelA;
    public ColorModel modelB;
    public FieldScript field;
    public Transform listOfColor;
    public int teamIndex;
    public InputField inputField;
    public Camera cameraA;
    public Camera cameraB;
    public MainMenuControl mainMenu;
    public bool isBackToMMAfterFinish;
    public GameplayUIController gameplayUIController;
    public Text labelName;
    public Text labelColor;
    public Transform lightForModel;

    // Start is called before the first frame update
    void Start()
    {
        showColor();
        teamIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setActive(bool isActive){
        transform.GetChild(0).gameObject.SetActive(isActive);
        if(isActive==true){
            GameMaster.GM.pauseForChangeColor();
            initOnEnter();
        }
    }

    public void onClickColor(int colorIndex){
        GameMaster.GM.teamList[teamIndex].color = colorIndex;
        changeFieldAndModelColor();
    }

    public void onClickButtonNext(){
        applyPlayerName();
        if(teamIndex==0){
            teamIndex=1;
            refreshOnChangePage();
        }else if(teamIndex==1){
            teamIndex=0;
            finishChangeColor();
        }
    }

    public void onClickButtonBack(){
        applyPlayerName();
        if(teamIndex==0){
            isBackToMMAfterFinish=true;
            finishChangeColor();
        }else if(teamIndex==1){
            teamIndex=0;
            refreshOnChangePage();
        } 
    }
    void changeFieldAndModelColor(){
        field.init();
        modelA.initModel();
        modelB.initModel();
    }

    void showColor(){
        for(int i=0; i<listOfColor.childCount; i++){
            listOfColor.GetChild(i).GetComponent<Image>().color = Utility.getColorCode(i);
        }
    }

    void applyPlayerName(){
        GameMaster.GM.teamList[teamIndex].playerName = inputField.text;
    }

    void switchCamera(){
        if(teamIndex==0){
            cameraA.gameObject.SetActive(true);
            cameraB.gameObject.SetActive(false);
        }else if(teamIndex==1)
        {
            cameraA.gameObject.SetActive(false);
            cameraB.gameObject.SetActive(true);
        }
    }

    void initText(){
        inputField.text = GameMaster.GM.teamList[teamIndex].playerName;
        labelName.text = string.Format("Player {0} name:", teamIndex+1);
        labelColor.text = string.Format("Player {0} color:", teamIndex+1);
    }

    void finishChangeColor(){
        setActive(false);
        if(isBackToMMAfterFinish){
            mainMenu.setActive(true);
        }else{
            GameMaster.GM.startGamePlay();
            GameMaster.GM.startNewRound();
            GameMaster.GM.isFirstPlay = false;
        }
    }

    void disableAlreadyPickedColor(){
        listOfColor.GetChild(GameMaster.GM.teamList[teamIndex].color).GetComponent<Button>().interactable = true;
        listOfColor.GetChild(GameMaster.GM.teamList[(teamIndex+1)%2].color).GetComponent<Button>().interactable = false;
    }

    void changeLightDirection(){
        if(teamIndex ==0)
            lightForModel.rotation = Quaternion.LookRotation(new Vector3(0, 0, 180));
        else
            lightForModel.rotation = Quaternion.identity;
    }

    void refreshOnChangePage(){
        switchCamera();
        initText();
        changeFieldAndModelColor();
        disableAlreadyPickedColor();
        //changeLightDirection();
    }

    void initOnEnter(){
        teamIndex = 0;
        refreshOnChangePage();
    }
}
