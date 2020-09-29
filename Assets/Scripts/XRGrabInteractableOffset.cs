using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableOffset : XRGrabInteractable
{
    private Vector3 initAttachLocalPos;
    private Quaternion initAttachLocalRot;
    // Start is called before the first frame update
    void Start()
    {
        if(!attachTransform){ // attachment point for our offset
            GameObject grabPivot = new GameObject("Grab Pivot");
            grabPivot.transform.SetParent(transform, false);
            attachTransform = grabPivot.transform;
        }

        initAttachLocalPos = attachTransform.localPosition;
        initAttachLocalRot = attachTransform.localRotation;
    }

    protected override void OnSelectEnter(XRBaseInteractor interactor){
        if(interactor is XRDirectInteractor){
            attachTransform.position = interactor.transform.position;
            attachTransform.rotation = interactor.transform.rotation;
        } else {
            attachTransform.localPosition = initAttachLocalPos;
            attachTransform.localRotation = initAttachLocalRot;
        }
        base.OnSelectEnter(interactor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
