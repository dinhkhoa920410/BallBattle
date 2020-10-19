using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIController : MonoBehaviour
{
    public Text txtTime;
    public Text txtNamePlayerA;
    public Text txtNamePlayerB;
    public GameObject objRoundWin;
    public Slider energyBarA;
    public Slider energyBarB;
    public Slider energyHighlightA;
    public Slider energyHighlightB;
    public ARController arController;
    public GameObject ingameMenu;
    public MainMenuControl mainMenuControl;
    public Transform endGameWindow;
    public Text txtScoreA;
    public Text txtScoreB;
    public Button btnIngameMenu;
    public Button btnAR;
    private bool isPopUpShow;
    
    //private bool isFirstTimeTurnOnAR;
    void Start()
    {
       isPopUpShow = false;
    }

    // Update is called once per frame
    void Update()
    {
        setButtonsInteractable(!isPopUpShow);
        drawTime();
        updateEnergyBar();
        //slowUpdate(0.1f);
        updateHighlightBar();
    }
    
    public void setActive(bool isActive){
        transform.GetChild(0).gameObject.SetActive(isActive);
    }

    void drawTime(){
        txtTime.text = GameMaster.GM.timer.ToString();
    }
    
    void updateEnergyBar(){
        float energyA = GameMaster.GM.teamA.energy;
        float energyB = GameMaster.GM.teamB.energy;
        energyBarA.value = energyA / Team.MAX_ENERGY;
        energyBarB.value = energyB / Team.MAX_ENERGY;
    }
    
    void updateHighlightBar(){
        float energyA = GameMaster.GM.teamA.energy;
        float energyB = GameMaster.GM.teamB.energy;
        
        if(energyA < 1)
            energyHighlightA.value = 0;
        else if(energyA < 2)
            energyHighlightA.value = 1.0f/Team.MAX_ENERGY;
        else if(energyA < 3)
            energyHighlightA.value = 2.0f/Team.MAX_ENERGY;
        else if(energyA < 4)
            energyHighlightA.value = 3.0f/Team.MAX_ENERGY;
        else if(energyA < 5)
            energyHighlightA.value = 4.0f/Team.MAX_ENERGY;
        else if(energyA < 6)
            energyHighlightA.value = 5.0f/Team.MAX_ENERGY;
        else
            energyHighlightA.value = 6.0f/Team.MAX_ENERGY;

        if(energyB < 1)
            energyHighlightB.value = 0;
        else if(energyB < 2)
            energyHighlightB.value = 1.0f/Team.MAX_ENERGY;
        else if(energyB < 3)
            energyHighlightB.value = 2.0f/Team.MAX_ENERGY;
        else if(energyB < 4)
            energyHighlightB.value = 3.0f/Team.MAX_ENERGY;
        else if(energyB < 5)
            energyHighlightB.value = 4.0f/Team.MAX_ENERGY;
        else if(energyB < 6)
            energyHighlightB.value = 5.0f/Team.MAX_ENERGY;
        else
            energyHighlightB.value = 6.0f/Team.MAX_ENERGY;
    }

    public void refreshPlayerInfo(){
        txtNamePlayerA.text = GameMaster.GM.teamA.getInfo();
        txtNamePlayerB.text = GameMaster.GM.teamB.getInfo();
        txtNamePlayerA.color = Utility.getColorCode(GameMaster.GM.teamA.color);
        txtNamePlayerB.color = Utility.getColorCode(GameMaster.GM.teamB.color);
        energyBarA.GetComponentInChildren<Image>().color = Utility.setAlpha(Utility.getColorCode(GameMaster.GM.teamA.color), 0.4f);
        energyBarB.GetComponentInChildren<Image>().color = Utility.setAlpha(Utility.getColorCode(GameMaster.GM.teamB.color), 0.4f);
        energyHighlightA.GetComponentInChildren<Image>().color = Utility.getColorCode(GameMaster.GM.teamA.color);
        energyHighlightB.GetComponentInChildren<Image>().color = Utility.getColorCode(GameMaster.GM.teamB.color);

    }

    public void onClickNextRoundButton(){
        GameMaster.GM.startNewRound();
        objRoundWin.SetActive(false);
        isPopUpShow = false;
    }

    public void showEndRoundWindow(Team winner){
        GameMaster.GM.pauseGame();
        string winText;
        if(winner != null)
           winText = string.Format("Round {0}\n{1}\n{2}", GameMaster.GM.round, winner.getStringRole(), winner.playerName);
        else
            winText = string.Format("Round {0}\n\nDRAW", GameMaster.GM.round);
        objRoundWin.SetActive(true);
        isPopUpShow = true;
        objRoundWin.GetComponentInChildren<Text>().text = winText;
    }

    public void onClickBtnIngameMenu(){
        if(!isPopUpShow){
            ingameMenu.SetActive(true);
            isPopUpShow = true;
            GameMaster.GM.pauseGame();
        }
    }

    public void onClickBtnResume(){
        ingameMenu.SetActive(false);
        isPopUpShow = false;
        GameMaster.GM.resumeGame();
    }

    public void onClickBtnMainMenu(){
        setActive(false);
        ingameMenu.SetActive(false);
        isPopUpShow = false;
        mainMenuControl.setActive(true);
    }

    public void onClickBtnEndGame(){
        setActive(false);
        endGameWindow.gameObject.SetActive(false);
        isPopUpShow = false;
        mainMenuControl.setActive(true);
    }

    void refreshEndGameInfo(){
        Text txtNames = endGameWindow.GetChild(0).GetComponent<Text>();
        Text txtScores = endGameWindow.GetChild(1).GetComponent<Text>();
        Text txtWinner = endGameWindow.GetChild(3).GetComponent<Text>();
        txtNames.text = string.Format("{0}         vs         {1}", GameMaster.GM.teamA.playerName,GameMaster.GM.teamB.playerName);
        txtScores.text = string.Format("{0}        -        {1}", GameMaster.GM.teamA.winTimes, GameMaster.GM.teamB.winTimes);
        txtWinner.text = GameMaster.GM.teamA.winTimes>GameMaster.GM.teamB.winTimes?GameMaster.GM.teamA.playerName:GameMaster.GM.teamB.playerName;
    }

    public void showEndGameWindow(){
        GameMaster.GM.pauseGame();
        endGameWindow.gameObject.SetActive(true);
        refreshEndGameInfo();
    }

    public void refreshScore(){
        Debug.Log(GameMaster.GM.teamA.winTimes);
        Debug.Log(GameMaster.GM.teamB.winTimes);
        txtScoreA.text = GameMaster.GM.teamA.winTimes.ToString();
        txtScoreB.text = GameMaster.GM.teamB.winTimes.ToString();
    }

    void setButtonsInteractable(bool isActive){
        btnIngameMenu.interactable = isActive;
        btnAR.interactable = isActive;
    }
}
