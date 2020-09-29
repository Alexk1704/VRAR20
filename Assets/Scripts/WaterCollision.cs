using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    Animator animator;
    private AudioSource bowlAudio;
    public AudioClip splash;

    void Start(){
        animator = GameObject.Find("waterbowl").GetComponent<Animator>();  
        bowlAudio = GameObject.Find("waterbowl").GetComponent<AudioSource>();
    }

    IEnumerator waterAnim(float mass, bool returning){
        resetTriggers();
        if(!returning){ // water lowers
            if(mass == 2.5){
                animator.SetTrigger("25collision");
            }
            if(mass == 5.0){
                animator.SetTrigger("50collision");
            }
            if(mass == 10.0){
                animator.SetTrigger("100collision");
            }     
        } else { // water rising
            if(mass == 2.5){
                animator.SetTrigger("25stopcollision");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); 
                animator.SetTrigger("25transition");
            }
            if(mass == 5.0){
                animator.SetTrigger("50stopcollision");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); 
                animator.SetTrigger("50transition");
            }
            if(mass == 10.0){
                animator.SetTrigger("100stopcollision");
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); 
                animator.SetTrigger("100transition");
            }
        }
        bowlAudio.PlayOneShot(splash);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); 
    }

    private void resetTriggers(){
        animator.ResetTrigger("25collision");
        animator.ResetTrigger("50collision");
        animator.ResetTrigger("100collision");
        animator.ResetTrigger("25stopcollision");
        animator.ResetTrigger("50stopcollision");
        animator.ResetTrigger("100stopcollision");
        animator.ResetTrigger("25transition");
        animator.ResetTrigger("50transition");
        animator.ResetTrigger("100transition");
    }

    void OnCollisionEnter(Collision other) {
        GameObject cgo = other.collider.gameObject;
        if(cgo.name == "core_0" || cgo.name == "core_1" || cgo.name == "core_2" || cgo.name == "core_3"){
            float cgoMass = cgo.GetComponent<Rigidbody>().mass;
            IEnumerator coroutine = waterAnim(cgoMass, false);
            StartCoroutine(coroutine);
        }   
    }
    void OnCollisionExit(Collision other) {
        GameObject cgo = other.collider.gameObject;
        if(cgo.name == "core_0" || cgo.name == "core_1" || cgo.name == "core_2" || cgo.name == "core_3"){
            float cgoMass = cgo.GetComponent<Rigidbody>().mass;
            IEnumerator coroutine = waterAnim(cgoMass, true);
            StartCoroutine(coroutine);
        }     
    }
}
