using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinousMovement : MonoBehaviour
{
    public XRNode inputSrc;
    public float addHeight = 0.15f;
    private Vector2 inputAxis;
    private CharacterController character;
    private XRRig rig;
    private float fallingSpd;
    public LayerMask groundLayer;
    public float spd;
    public float gravity = -9.81f; // aprox. earth gravity

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();  
        rig = GetComponent<XRRig>();  
    }

    // Update is called once per frame
    void Update()
    {
        // Listen to input node
        InputDevice dvc = InputDevices.GetDeviceAtXRNode(inputSrc);
        dvc.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
    }

    // Updated each time unity updates physics of the game
    private void FixedUpdate(){
        followHead();
        // move along with the head with hYaw
        Quaternion hYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);
        Vector3 direction = hYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        character.Move(direction * Time.fixedDeltaTime * spd);

        bool onGround = checkGround();
        if(onGround)
            fallingSpd = 0;
        else
            fallingSpd += gravity * Time.fixedDeltaTime;
        
        character.Move(Vector3.up * fallingSpd * Time.fixedDeltaTime);
    }

    void followHead(){
        character.height = rig.cameraInRigSpaceHeight + addHeight; // height exactly at the place
        Vector3 center = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(center.x, character.height/2 + character.skinWidth, center.z);
    }

    // are we on the ground?
    bool checkGround(){
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.02f;
        bool hit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hit;
    }
}
