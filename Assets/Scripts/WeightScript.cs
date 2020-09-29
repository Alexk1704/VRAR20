using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightScript : MonoBehaviour {
    private float totalWeightRight, totalWeightLeft;
    private List<Rigidbody> leftRigidBodies, rightRigidBodies;
    public GameObject leftBowl, rightBowl, scale, table;
    private int weightCount;

    private AudioSource buttonAudioSource, scaleAudioSource;
    public AudioClip buttonSound, failButtonSound, weightSound;
    private Animator leftBowlAnimator, rightBowlAnimator;
    private bool leftDown, rightDown;
    private bool hit = false;
    private GameObject button;

    public float buttonDownDist = 0.0205f;
    public float buttonReturnSpd = 0.005f;
    private float buttonOriginY;
    public float hitTimer = 2f;
    private float hitable;
    IEnumerator coroutine = null;
    private Vector3[] origPos;
    
    void Start() {
        button = transform.GetChild(0).gameObject; // Capsule Body
        buttonOriginY = button.transform.position.y;
        
        leftRigidBodies = new List<Rigidbody>();
        rightRigidBodies = new List<Rigidbody>();

        buttonAudioSource = table.GetComponent<AudioSource>();
        scaleAudioSource = scale.GetComponent<AudioSource>();
        leftBowlAnimator = leftBowl.GetComponent<Animator>();
        rightBowlAnimator = rightBowl.GetComponent<Animator>();
        origPos = new Vector3[12];

        var rand = new System.Random();
        var rnd = rand.Next(12);
        GameObject.FindGameObjectsWithTag("dice")[rnd].GetComponent<Rigidbody>().mass = 200; 
        for(int i = 0; i < GameObject.FindGameObjectsWithTag("dice").Length; i++){
            origPos[i] = GameObject.FindGameObjectsWithTag("dice")[i].transform.position;
        }

        leftDown = false; rightDown = false;
        weightCount = 0;
    }

    public void respawnDices(){
        GameObject[] currentDices = GameObject.FindGameObjectsWithTag("dice");
        for(int i = 0; i < currentDices.Length; i++) {
            currentDices[i].GetComponent<Rigidbody>().mass = 100;
            currentDices[i].transform.position = origPos[i];
        }
        var rand = new System.Random();
        var rnd = rand.Next(12);
        currentDices[rnd].GetComponent<Rigidbody>().mass = 200;
    }

    void Update() {
        if(hit == true && weightCount < 3){
            buttonAudioSource.PlayOneShot(buttonSound);
            hit = false;
            button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y - buttonDownDist, button.transform.position.z);

            weightCount++;
            coroutine = WeightCoRoutine();
            StartCoroutine(coroutine);
        } 
        if(hit == true && weightCount >= 3) {
            buttonAudioSource.PlayOneShot(failButtonSound);
            hit = false;
            button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y - buttonDownDist, button.transform.position.z);
        }
        // zurückkehren
        if(button.transform.position.y < buttonOriginY){
            button.transform.position += new Vector3(0, buttonReturnSpd, 0);
        }
    }

    IEnumerator WeightCoRoutine(){
        calculateWeight();
        if(totalWeightLeft > totalWeightRight) {
            scaleAudioSource.PlayOneShot(weightSound);
            leftBowlAnimator.SetTrigger("leftbowldown");
            leftDown = true;
        }
        if(totalWeightRight > totalWeightLeft) {
            scaleAudioSource.PlayOneShot(weightSound);
            rightBowlAnimator.SetTrigger("rightbowldown");
            rightDown = true;
        } 
        yield return new WaitForSecondsRealtime(5);
        IEnumerator coroutine = emptyCoRoutine();
        StartCoroutine(coroutine);
    }

    IEnumerator emptyCoRoutine (){
        leftBowlAnimator.ResetTrigger("leftbowldown");
        rightBowlAnimator.ResetTrigger("rightbowldown");
        leftBowlAnimator.ResetTrigger("leftbowlup");
        leftBowlAnimator.ResetTrigger("leftbowltransition");
        rightBowlAnimator.ResetTrigger("rightbowlup");
        rightBowlAnimator.ResetTrigger("rightbowltransition");

        if(leftDown) {
            leftBowlAnimator.SetTrigger("leftbowlup");
            scaleAudioSource.PlayOneShot(weightSound);
            yield return new WaitForSeconds(leftBowlAnimator.GetCurrentAnimatorStateInfo(0).length);   
            leftBowlAnimator.SetTrigger("leftbowltransition");
            leftDown = false;
        }
        if(rightDown) {
            rightBowlAnimator.SetTrigger("rightbowlup");
            scaleAudioSource.PlayOneShot(weightSound);
            yield return new WaitForSeconds(rightBowlAnimator.GetCurrentAnimatorStateInfo(0).length);   
            rightBowlAnimator.SetTrigger("rightbowltransition");
            rightDown = false;
        }
         
        Collider[] left = Physics.OverlapSphere(leftBowl.transform.position, 0.75f);
        Collider[] right = Physics.OverlapSphere(rightBowl.transform.position, 0.75f);
        
        for(int i = 0; i < left.Length; i++){
            if(left[i].gameObject.tag == "dice"){
                left[i].gameObject.transform.position = new Vector3(7.45f, 2.0f, 47.5f);
            }
        }

        for(int i = 0; i < right.Length; i++){
            if(right[i].gameObject.tag == "dice"){
                right[i].gameObject.transform.position = new Vector3(7.45f, 2.0f, 47.5f);
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if((other.name == "Right Hand" || other.name == "Left Hand") && (hitable < Time.time)){
            hitable = Time.time + hitTimer; // warten bis Zeit abgelaufen ist
            hit = true; // knopf gedrückt
        }
    }

    private void calculateWeight(){
        totalWeightLeft = 0;
        totalWeightRight = 0;

        Collider[] left = Physics.OverlapSphere(leftBowl.transform.position, 0.75f);
        Collider[] right = Physics.OverlapSphere(rightBowl.transform.position, 0.75f);
        
        for(int i = 0; i < left.Length; i++){
            if(left[i].gameObject.tag == "dice"){
                leftRigidBodies.Add(left[i].gameObject.GetComponent<Rigidbody>());
            }
        }
        for(int i = 0; i < right.Length; i++){
            if(right[i].gameObject.tag == "dice"){
                rightRigidBodies.Add(right[i].gameObject.GetComponent<Rigidbody>());
            }
        }

        for (int i = 0; i < leftRigidBodies.Count; i++) {
            totalWeightLeft += leftRigidBodies[i].mass;
        }
        for (int i = 0; i < rightRigidBodies.Count; i++) {
            totalWeightRight += rightRigidBodies[i].mass;
        }
        leftRigidBodies.Clear(); rightRigidBodies.Clear();
        Debug.Log("LEFT: " + totalWeightLeft +" RIGHT: " + totalWeightRight);
    }
}