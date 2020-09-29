using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public float timeLimit = 100;
    private float minsLeft = 0.0f;
    private float secLeft = 0.0f;
    public Text time;
    public bool gameOver = false;

    void getTime(float time) {
        time=time+1;
        minsLeft = Mathf.FloorToInt(time/60); 
        secLeft = Mathf.FloorToInt(time%60);
    }

    void convertToTxt(float mins, float secs) {
        time.text = string.Format("{0:00}:{1:00}", mins, secs);
    }

    void Update() {
        if(!gameOver){
            if(timeLimit > 0){
                timeLimit -= Time.deltaTime;
                getTime(timeLimit);
                convertToTxt(minsLeft, secLeft);
            } else{
                gameOver = true;
                timeLimit = 0;     
            }
        }
    }
}
