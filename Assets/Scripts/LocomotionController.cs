using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
    public XRController lRay;
    public XRController rRay;
    public InputHelpers.Button teleButton;
    public float activeThreshold = 0.1f;

    public XRRayInteractor lInteractorRay;
    public XRRayInteractor rInteractorRay;

    public bool enableL { get; set; } = true; 
    public bool enableR { get; set; } = true;

    // Start is called before the first frame update
    public bool checkIfActive(XRController controller){
        InputHelpers.IsPressed(controller.inputDevice, teleButton, out bool isActive, activeThreshold);
        return isActive;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 pos = new Vector3();
        Vector3 norm = new Vector3();
        int id = 0;
        bool validTar = false;

        if(lRay) {
            bool isLeftRayHovering = lInteractorRay.TryGetHitInfo(ref pos, ref norm, ref id, ref validTar);
            lRay.gameObject.SetActive(checkIfActive(lRay) && enableL && !isLeftRayHovering);
        }
        if(rRay) {
            bool isRightRayHovering = rInteractorRay.TryGetHitInfo(ref pos, ref norm, ref id, ref validTar);
            rRay.gameObject.SetActive(checkIfActive(rRay) && enableR && !isRightRayHovering);
        }
        
    }
}
