using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Timer
{
    public float timeRemaining;
    public bool IsRunning;
    float lifeTime;
    bool is15secTriggered=false;
    
    void Update()
    {

    }

    public Timer(float lifeTime)
    {
        this.lifeTime = lifeTime;
        timeRemaining = lifeTime;
        IsRunning = false;
    }

    public void timerUpdate(){
        if (IsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                if(timeRemaining < GameMaster.RUSH_TIME)
                    if(!is15secTriggered){
                        GameMaster.GM.rushTimeBegin();
                        is15secTriggered = true;
                    }
            }
            else
            {
                timeRemaining = 0;
                IsRunning = false;
                GameMaster.GM.endRound(null);
            }
        }
    }

    public void start(){
        IsRunning = true;
    }

    public void restart(){
        IsRunning = true;
        timeRemaining = lifeTime;
        is15secTriggered = false;
    }

    public override string ToString(){
        return string.Format("{0:0}:{1:0}", (int) timeRemaining/60, timeRemaining%60);
    }
}
