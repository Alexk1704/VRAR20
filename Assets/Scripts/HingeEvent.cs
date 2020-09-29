using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HingeEvent : MonoBehaviour
{
    private HingeJoint hinge;
    public enum HingeJointState {Min,Max,None}
    public HingeJointState hingeJointState = HingeJointState.None; // Zustand des HingeJoint, entweder min, max oder iwas dazwischen
    public UnityEvent minEvent; // Event das bei minimalen Grenzwert ausgelöst wird
    public UnityEvent maxEvent; // Analog für MAX

    void Start(){
        hinge = GetComponent<HingeJoint>();
    }

    private void FixedUpdate(){ 
        if(hinge.angle == 0){ // Minimalen Grenzwert erreicht
            if(hingeJointState != HingeJointState.Min)
                hingeJointState = HingeJointState.Min;
        }
        else if (hinge.angle > 44.9){ // Maximalen Grenzwert erreicht   
            if(hingeJointState != HingeJointState.Max){
                maxEvent.Invoke();
            }
            hingeJointState = HingeJointState.Max;
        }
        else{ // Alles dazwischen = nichts
            hingeJointState = HingeJointState.None;
        }
    }
}