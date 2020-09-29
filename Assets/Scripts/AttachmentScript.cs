using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentScript : MonoBehaviour {
    public GameObject powercore;
    private GameObject attached = null;
    public bool active;
    public float correctMass = 1f;

    void OnTriggerStay(Collider other) {
        string gameObjectName = other.GetComponent<Collider>().gameObject.name;
        if(gameObjectName == "core_0" || gameObjectName == "core_1" || gameObjectName == "core_2" || gameObjectName == "core_3"){
            attached = other.GetComponent<Collider>().gameObject;
            Rigidbody rb = attached.GetComponent<Rigidbody>();
            if(rb.mass == correctMass){
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
