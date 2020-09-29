using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Diagnostics;

public class HandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;
    public bool showController = false; // show controller or hands

    private InputDevice targetDvc;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;
    // Start is called before the first frame update
    void Start()
    {
        TryInit();
    }
    void UpdateHandAnimation(){
        if(targetDvc.TryGetFeatureValue(CommonUsages.trigger, out float triggerVal)){
            handAnimator.SetFloat("Trigger", triggerVal); // get trigger button input and give it to trigger animation as a float value
        } else{
            handAnimator.SetFloat("Trigger", 0);
        }

        if(targetDvc.TryGetFeatureValue(CommonUsages.grip, out float gripVal)){
            handAnimator.SetFloat("Grip", gripVal); // analogous to trigger button
        } else {
            handAnimator.SetFloat("Grip", 0);
        }
    }
    void TryInit(){
        List<InputDevice> dvcs = new List<InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, dvcs); // Listen to right controller from all devices (with specific characteristics)
        // foreach(var dvc in dvcs) UnityEngine.Debug.Log(dvc.name + dvc.characteristics);
        if(dvcs.Count > 0){
            targetDvc = dvcs[0]; // listen to first device available
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDvc.name);
            if(prefab){ // search for corresponding controller model for target device
                spawnedController = Instantiate(prefab, transform);
            } else {
                UnityEngine.Debug.Log("No corresponding controller model available!");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }
            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(!targetDvc.isValid){ // not nil
            TryInit();
        } else {
            if(showController){
            spawnedHandModel.SetActive(false);
            spawnedController.SetActive(true);
            } else {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
                UpdateHandAnimation();
            }
            /* 
            // Buttons we listen to can have 3 types of output values (boolean: pressed or not (button) / axis: float between 0-1 (trigger) / Vector2 (touchpad)) 
            if(targetDvc.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryBtnVal) && primaryBtnVal){
                UnityEngine.Debug.Log("Pressed primary button!");
            }
            if(targetDvc.TryGetFeatureValue(CommonUsages.trigger, out float triggerVal) && triggerVal > 0.1f){
                UnityEngine.Debug.Log("Pressed trigger button!");
            }
            if(targetDvc.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisVal) && primary2DAxisVal != Vector2.zero){
                UnityEngine.Debug.Log("Pressed touchpad button!");
            }
            */
        }
    }
}
