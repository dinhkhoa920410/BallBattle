using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public static GameMaster GM;

    ///////
    public const int MAX_ROUND = 5;
    public const float TIME_LIMIT = 140;
    public const float RUSH_TIME = 15;
    Vector2 WINDOW_UI_SCALE = new Vector2(1600,900);
    Vector2 ANDROID_UI_SCALE = new Vector2(800, 600);
    //
    public Transform AttackerList;
    public Transform DefenderList;
    public GameObject prefabAttacker;
    public GameObject prefabDefender;
    public BallScript ball;
    public GameplayUIController gameplayUI;
    public FieldScript field;
    public Transform goalTeamA;
    public Transform goalTeamB;
    public Team teamA;
    public Team teamB;
    public Timer timer;
    public int round;
    public Team[] teamList;
    public bool isFirstPlay;
    public bool isRushTime;
    public GameObject rushTime;
    bool isAlreadyEndRound;
    public bool isPause;
    public CanvasScaler mainUIScaler;
    public CanvasScaler changeClothesUIScaler;

    void Awake(){
        if(GM != null)
            GameObject.Destroy(GM);
        else
            GM = this;

        DontDestroyOnLoad(this);

        //Scale UI for seperate platform
        #if UNITY_ANDROID
            mainUIScaler.referenceResolution = ANDROID_UI_SCALE;
            changeClothesUIScaler.referenceResolution = ANDROID_UI_SCALE;
        #elif UNITY_STANDALONE_WIN
            mainUIScaler.referenceResolution = WINDOW_UI_SCALE;
            changeClothesUIScaler.referenceResolution = WINDOW_UI_SCALE;
        #endif
    }

    // Start is called before the first frame update
    void Start()
    {
        teamA = new Team(Team.ROLE_ATTACKER, Team.TEAM_A, Utility.CYAN);
        teamB = new Team(Team.ROLE_DEFENDER, Team.TEAM_B, Utility.BROWN);
        teamList = new Team[]{teamA, teamB};
        round = 1;
        isAlreadyEndRound = false;
        isFirstPlay = true;
        timer = new Timer(TIME_LIMIT);
        timer.start();
        field.init();
        gameplayUI.refreshPlayerInfo();
        pauseForChangeColor();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPause){
            gameUpdate();
            createPlayerOnClick();
        }
    }
    public void startGamePlay(){
        initOnEnterGameplay();
        resumeGame();
    }

    void initOnEnterGameplay(){
        field.init();
        gameplayUI.setActive(true);
        gameplayUI.refreshPlayerInfo();
    }

    void createPlayerOnClick(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask teamALayerMask = 1<<LayerMask.NameToLayer("TeamA");
        LayerMask teamBLayerMask = 1<<LayerMask.NameToLayer("TeamB");
        if(Input.GetMouseButtonDown(0)){
            if(Physics.Raycast(ray, out hit, 1000, teamALayerMask))
                createPlayerBaseOnTeamRole(teamA,hit.point);
            if(Physics.Raycast(ray, out hit, 1000, teamBLayerMask))
                createPlayerBaseOnTeamRole(teamB,hit.point);
        }
    }

    AttackerScript createAttacker(Vector3 position){
        return Instantiate(prefabAttacker, position, Quaternion.identity, AttackerList).GetComponent<AttackerScript>();
    }
    DefenderScript createDefender(Vector3 position){
        return Instantiate(prefabDefender, position, Quaternion.identity, DefenderList).GetComponent<DefenderScript>();
    }
    void createPlayerBaseOnTeamRole(Team team, Vector3 position){
        if(team.energy >= team.energyCost){
            SoccerPlayerScript playerScript;
            if(team.role == Team.ROLE_ATTACKER)
                playerScript = createAttacker(position);
            else
                playerScript = createDefender(position);
            team.energy -= team.energyCost;
            playerScript.initPlayer(team);
            if(team == teamA)
                playerScript.transform.rotation = Quaternion.LookRotation(goalTeamB.position - playerScript.transform.position);
            else
                playerScript.transform.rotation = Quaternion.LookRotation(goalTeamA.position - playerScript.transform.position);
        }
    }


    void switchRole(){
        //Fixed bug switch role 2 times because delay deactive of defender
        int temp = teamA.role;
        teamA.role = teamB.role;
        teamB.role = temp;
    }

    void gameUpdate(){
        energyRegenOverTime();
        timer.timerUpdate();
    }

    void energyRegenOverTime(){
        teamA.energyRegen = Team.ENERGY_REGEN*(isRushTime?2:1);
        teamB.energyRegen = Team.ENERGY_REGEN*(isRushTime?2:1);
        if(teamA.energy < Team.MAX_ENERGY)
            teamA.energy += teamA.energyRegen*Time.deltaTime;
        else
            teamA.energy = Team.MAX_ENERGY;

        if(teamB.energy < Team.MAX_ENERGY)
            teamB.energy += teamB.energyRegen*Time.deltaTime;
        else
            teamB.energy = Team.MAX_ENERGY;
    }

    public void pauseGame(){
        Time.timeScale = 0;
        isPause = true;
    }
    public void pauseForChangeColor(){
        Time.timeScale = 1;
        isPause = true;
    }
    public void resumeGame(){
        Time.timeScale = 1;
        isPause = false;
    }

    public void endRound(Team winner){
        if(!isAlreadyEndRound){
            if(winner != null){
                if(winner.side == teamA.side){
                    teamA.winTimes++;
                }
                else if(winner.side == teamB.side){
                    teamB.winTimes++;
                }
            }
            if(round<MAX_ROUND || teamA.winTimes == teamB.winTimes){
                gameplayUI.refreshScore();
                gameplayUI.showEndRoundWindow(winner);
                round++;
                switchRole();
            }else{
                endGame();
            }
            destroyAllSoccerPlayer();
            ball.setBallActive(false);
            isAlreadyEndRound = true;
        }
    }
    void endGame(){
        gameplayUI.showEndGameWindow();
        resetScores();
        isFirstPlay = true;
    }

    void destroyAllSoccerPlayer(){
        for(int i=0;i<AttackerList.childCount;i++){
            Destroy(AttackerList.GetChild(i).gameObject);
        }
        for(int i=0;i<DefenderList.childCount;i++){
            Destroy(DefenderList.GetChild(i).gameObject);
        }
    }

    public int getAttackerSide(){
        if(teamA.role == Team.ROLE_ATTACKER)
            return teamA.side;
        else
            return teamB.side;
    }

    public void startNewRound(){
        isAlreadyEndRound = false;
        teamA.energyRegen = Team.ENERGY_REGEN;
        teamB.energyRegen = Team.ENERGY_REGEN;
        gameplayUI.refreshPlayerInfo();
        timer.restart();
        ball.initNewRound();
        resumeGame();
        teamA.initNewRound();
        teamB.initNewRound();
    }

    void resetScores(){
        teamA.winTimes = 0;
        teamB.winTimes = 0;
    }

    public void rushTimeBegin(){
        isRushTime = true;
        rushTime.SetActive(true);
    }
}
