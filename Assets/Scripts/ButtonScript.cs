using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public GameObject icosahedron;
    private enum icoState {norm, x_1, y_1, z_1};
    private static icoState state;
    private AudioSource audioSource;
    public AudioClip soundClip;
    private Animator animator;

    private bool hit = false;
    private GameObject button;

    public float buttonDownDist = 0.0205f;
    public float buttonReturnSpd = 0.005f;
    private float buttonOriginY;
    public float hitTimer = 2f; // Zeit nach der der Button wieder verfügbar ist
    private float hitable;
    private IEnumerator coroutine = null;
    private static bool animRunning = false;

    void Start()
    {
        animator = icosahedron.GetComponent<Animator>();
        state = icoState.norm;
        audioSource = gameObject.AddComponent<AudioSource>();
        button = transform.GetChild(0).gameObject; // Capsule Body
        buttonOriginY = button.transform.position.y;
    }

    IEnumerator AnimCoRoutine(int btn) {
        resetAllTriggers();
        //Debug.Log("Entered Coroutine: " + animRunning + " at: " + Time.time);
        if(btn == 0){ // x
            if(state == icoState.norm && !animRunning){
                animRunning = true; // Coroutine führt animation aus
                animator.SetTrigger("ico_rot_x");
                state = icoState.x_1;
            } if(state == icoState.x_1 && !animRunning) {
                animRunning = true;
                animator.SetTrigger("ico_rot_x2");
                //Debug.Log("Finished x to x2 transition at: " + Time.time);
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); 
                animator.SetTrigger("ico_rot_x2_to_default");
                state = icoState.norm;
                //Debug.Log("\tFinished x2 to norm transition at: " + Time.time);
            } if(state == icoState.y_1 && !animRunning) {
                animRunning = true; 
                animator.SetTrigger("ico_rot_y2");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);  
                animator.SetTrigger("ico_rot_y2_to_default");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);   
                animator.SetTrigger("ico_rot_x");
                state = icoState.x_1;
            } if(state == icoState.z_1 && !animRunning) {
                animRunning = true;
                animator.SetTrigger("ico_rot_z2");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);   
                animator.SetTrigger("ico_rot_z2_to_default");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);    
                animator.SetTrigger("ico_rot_x");
                state = icoState.x_1;    
            } 
        }
        if(btn == 1){ // y
            if(state == icoState.norm && !animRunning){
                animRunning = true; // Coroutine führt animation aus
                animator.SetTrigger("ico_rot_y");
                state = icoState.y_1;
                //Debug.Log("Finished norm to y transition at: " + Time.time);
            } if(state == icoState.y_1 && !animRunning) {
                animRunning = true;
                animator.SetTrigger("ico_rot_y2");
                //Debug.Log("Finished y to y2 transition at: " + Time.time);
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
                animator.SetTrigger("ico_rot_y2_to_default");
                state = icoState.norm;
                //Debug.Log("\tFinished y2 to norm transition at: " + Time.time);
            } if(state == icoState.x_1 && !animRunning) {
                animRunning = true;
                animator.SetTrigger("ico_rot_x2");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);        
                animator.SetTrigger("ico_rot_x2_to_default");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
                animator.SetTrigger("ico_rot_y");
                state = icoState.y_1;
            } if(state == icoState.z_1 && !animRunning) {
                animRunning = true;
                animator.SetTrigger("ico_rot_z2");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
                animator.SetTrigger("ico_rot_z2_to_default");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
                animator.SetTrigger("ico_rot_y");
                state = icoState.y_1;
            } 
        }
        if(btn == 2){ // z
            if(state == icoState.norm && !animRunning){
                animRunning = true; // Coroutine führt animation aus
                animator.SetTrigger("ico_rot_z");
                state = icoState.z_1;
            } if(state == icoState.z_1 && !animRunning) {
                animRunning = true;
                animator.SetTrigger("ico_rot_z2");
                //Debug.Log("Finished z to z2 transition at: " + Time.time);
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
                animator.SetTrigger("ico_rot_z2_to_default");
                state = icoState.norm;
            } if(state == icoState.y_1 && !animRunning) {
                animRunning = true;
                animator.SetTrigger("ico_rot_y2");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
                animator.SetTrigger("ico_rot_y2_to_default");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);    
                animator.SetTrigger("ico_rot_z");
                state = icoState.z_1;
            } if(state == icoState.x_1 && !animRunning) {
                animRunning = true;     
                animator.SetTrigger("ico_rot_x2");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
                animator.SetTrigger("ico_rot_x2_to_default");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
                animator.SetTrigger("ico_rot_z");
                state = icoState.z_1;
            }
        } 
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        animRunning = false;
    }

    private void resetAllTriggers(){
        animator.ResetTrigger("ico_rot_x_to_default");
        animator.ResetTrigger("ico_rot_x");
        animator.ResetTrigger("ico_rot_x2");
        animator.ResetTrigger("ico_rot_y_to_default");
        animator.ResetTrigger("ico_rot_y");
        animator.ResetTrigger("ico_rot_y2");
        animator.ResetTrigger("ico_rot_z_to_default");
        animator.ResetTrigger("ico_rot_z");
        animator.ResetTrigger("ico_rot_z2");
    }

    // Update is called once per frame
    void Update() {
        if(hit == true){
            audioSource.PlayOneShot(soundClip);
            hit = false;
            button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y - buttonDownDist, button.transform.position.z);
            if(!animator.IsInTransition(0)){
                if(button.name == "capsule_x"){
                    //Debug.Log("Pressed x button at: " + Time.time);
                    coroutine = AnimCoRoutine(0);
                    StartCoroutine(coroutine);
                } if(button.name == "capsule_y"){
                    //Debug.Log("Pressed y button at: " + Time.time);
                    coroutine = AnimCoRoutine(1);
                    StartCoroutine(coroutine);
                } if(button.name == "capsule_z"){
                    //Debug.Log("Pressed z button at: " + Time.time);
                    coroutine = AnimCoRoutine(2);
                    StartCoroutine(coroutine);
                }
            }
        //Debug.Log("Finished Button hit at: " + Time.time);
        }
        // zurückkehren
        if(button.transform.position.y < buttonOriginY){
            button.transform.position += new Vector3(0, buttonReturnSpd, 0);
        }
    }

    void OnTriggerEnter(Collider other) {
        if((other.name == "Right Hand" || other.name == "Left Hand") && (hitable < Time.time)){
            hitable = Time.time + hitTimer; // warten bis Zeit abgelaufen ist
            hit = true; // knopf gedrückt
        }
    }
}
