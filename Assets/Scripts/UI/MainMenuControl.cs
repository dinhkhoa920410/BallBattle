using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{
    public Text btnPlayText;
    public ColorChangeController colorChangeController;
    public GameplayUIController gameplayUIController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickBtnPlay(){
        if(GameMaster.GM.isFirstPlay){
            colorChangeController.setActive(true);
            colorChangeController.isBackToMMAfterFinish = false;
        }
        else{
            GameMaster.GM.startGamePlay();
        }
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void setActive(bool isActive){
        transform.GetChild(0).gameObject.SetActive(isActive);
        if(isActive == true){
            GameMaster.GM.pauseGame();
            refreshPlayButtonText();
        }
    }

    public void onClickBtnChangeInfo(){
        setActive(false);
        colorChangeController.setActive(true);
        colorChangeController.isBackToMMAfterFinish = true;
    }

    public void onClickBtnMazeMode(){

    }

    public void onClickExit(){
        if (Application.platform == RuntimePlatform.Android){
            AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call<bool>("moveTaskToBack", true);
        }else{
            Application.Quit();
        }
    }

    public void refreshPlayButtonText(){
        if(GameMaster.GM.isFirstPlay)
            btnPlayText.text = "Play";
        else
            btnPlayText.text = "Resume"; 
    }
}
