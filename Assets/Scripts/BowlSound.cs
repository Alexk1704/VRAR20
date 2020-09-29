using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlSound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip audioClip;
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "dice"){
            audioSource.PlayOneShot(audioClip);
        }
    }
}
