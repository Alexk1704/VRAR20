using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAttach : MonoBehaviour {
    private GameObject attached = null;
    private bool active = false;

    void OnTriggerStay(Collider other) {
        if(other.GetComponent<Collider>().gameObject.tag == "dice"){
            attached = other.GetComponent<Collider>().gameObject;
            Rigidbody rb = attached.GetComponent<Rigidbody>();
            if(rb.mass == 200){
                this.active = true;
            } 
            else{
                this.active = false;
            }  
        } 
    }

    public bool getActive() {
        return active;
    }

    public void setActive(bool active) {
        this.active = active;
    }
}
