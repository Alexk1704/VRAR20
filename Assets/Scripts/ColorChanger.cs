using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ColorChanger : MonoBehaviour
{
    private XRBaseInteractable interactable;
    private MeshRenderer meshrend;
    public Material prev; // previous material
    public Material selected; // new mat
    public bool active = false;
    public bool wipe = false;
    public bool isLast = false;
    public bool contained = false;
    public static int index = -1;
    private Material current;

    private void setMat(XRBaseInteractor interactor){
        if(current == prev){
            index += 1;
            meshrend.material = selected;
            current = selected;
            active = true;
        } else {   
            if(index == 11){
                Debug.Log("Setting last!!!");
                isLast = true;
            } else {
                index -= 1;
                wipe = true;
            } 
            meshrend.material = prev;
            current = prev;
            active = false;       
        }
    }

    public void setIndex(int id){
        index = id;
    }

    public void resetCurrent(){
        current = prev;
    }

    private void Awake(){
        interactable = GetComponent<XRBaseInteractable>();
        meshrend = GetComponent<MeshRenderer>();
        prev = meshrend.material;
        current = prev;
        // activated -> change material to previous
        interactable.onFirstHoverEnter.AddListener(setMat);
    }
    
    private void OnDestroy(){
        interactable.onFirstHoverEnter.RemoveListener(setMat);
    }
}
