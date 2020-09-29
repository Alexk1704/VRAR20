using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject corePrefab;
    private GameObject pcore1, pcore2, pcore3, pcore4;
    private Vector3[] coreSpawns;
    private float[] coreMasses;
    private GameObject[] cores;
    public GameObject icosahedron, dicePillar, panel;
    private GameObject[] selectedVertices;
    private bool foundHamiltonian = false;
    private bool foundHam = false; 
    private bool foundPowerCores = false;
    private static int currentFilled = -1;
    Animator doorAnimator, door2Animator, door3Animator, clockAnimator, canvasAnimator;
    public GameObject door1, door2, door3, clock, canvas, linedrawer, pillar;
    private AudioSource door1_audioSrc, door2_audioSrc, door3_audioSrc, cameraAudio, pillar_audioSrc;
    public AudioClip door_success, door_failure, ham_victory, pillar_activate, game_lost;
    private int diceCount, coreCount;
    private bool gameLost = false;
    private bool won = false;
    public float restartDelay = 5f;

    public void checkPowerCores(){
        if(checkCoresActive() && coreCount <= 3){
            door1_audioSrc.PlayOneShot(door_success);
            doorAnimator.SetTrigger("opendoor1");
            clockAnimator.SetTrigger("moveclockto2");
            canvasAnimator.SetTrigger("movecanvas2");
            linedrawer.SetActive(true);
            pillar_audioSrc.PlayOneShot(pillar_activate);
            foundPowerCores = true;
            GetComponent<TimeController>().timeLimit += 300;
        } else if(!checkCoresActive() && coreCount <= 3){
            door1_audioSrc.PlayOneShot(door_failure);
            coreCount += 1;
            if(coreCount == 3){
                cameraAudio.PlayOneShot(game_lost);
                gameLost = true;
            }  
        }
    }

    private bool checkCoresActive(){
        if(pcore1.GetComponent<AttachmentScript>().getActive() && 
            pcore2.GetComponent<AttachmentScript>().getActive() && 
                pcore3.GetComponent<AttachmentScript>().getActive() && 
                    pcore4.GetComponent<AttachmentScript>().getActive()){
                        return true;
                    }
        else
            return false;
    }

    private void instantiateCores(){
        var rand = new System.Random();
        initCoreMasses();
        for (int ctr = 0; ctr < 4; ctr++){
            // Instantiate at position from array and zero rotation
            GameObject core = Instantiate(corePrefab, coreSpawns[ctr], Quaternion.identity);
            core.name = "core_" + ctr;
            cores[ctr] = core;
            while(true){
                var rnd = rand.Next(4);
                if(coreMasses[rnd] != -1.0f){
                    core.GetComponent<Rigidbody>().mass = coreMasses[rnd];
                    coreMasses[rnd] = -1.0f;
                    break;
                }
            }
        }  
    }

    public void resetCores(){
        for (int ctr = 0; ctr < 4; ctr++){
            GameObject core = cores[ctr];
            Destroy(core);
            cores[ctr] = null;
        }
        instantiateCores();
    }

    void initCoreMasses(){
        coreMasses[0] = 1f;
        coreMasses[1] = 2.5f;
        coreMasses[2] = 5f;
        coreMasses[3] = 10f;
    }

    void initCorePositions(){
        coreSpawns[0] = new Vector3(13.337f, 1.275f, 11.6f);
        coreSpawns[1] = new Vector3(12.8f, 2.225f, 13.5f);
        coreSpawns[2] = new Vector3(13.5f, 0.15f, 8.6f);
        coreSpawns[3] = new Vector3(1.255f, 1.115f, 2f);
    }

    void Start() {
        cameraAudio = GetComponent<AudioSource>();

        cores = new GameObject[4];
        coreSpawns = new Vector3[4];
        coreMasses = new float[4];
        coreCount = 0;

        pcore1 = GameObject.Find("powercore_1");
        pcore2 = GameObject.Find("powercore_2");
        pcore3 = GameObject.Find("powercore_3");
        pcore4 = GameObject.Find("powercore_4");

        initCorePositions();
        instantiateCores();

        canvasAnimator = canvas.GetComponent<Animator>();
        clockAnimator = clock.GetComponent<Animator>();
        doorAnimator = door1.GetComponent<Animator>();
        door2Animator = door2.GetComponent<Animator>();
        door3Animator = door3.GetComponent<Animator>();
        door1_audioSrc = door1.GetComponent<AudioSource>();
        door2_audioSrc = door2.GetComponent<AudioSource>();
        door3_audioSrc = door3.GetComponent<AudioSource>();
        pillar_audioSrc = pillar.GetComponent<AudioSource>();

        selectedVertices = new GameObject[13];

        diceCount = 0; 
    }

    private bool checkForHamiltonian() {
        var vertices = GameObject.FindGameObjectsWithTag("vertex");
        var vertexCount = vertices.Length;

        if(selectedVertices[12] != null){ // everything glows & last vertex deselected
            Debug.Log("SELECTED ALL VERTICES! First: " + selectedVertices[0].name + " Last: " + selectedVertices[12].name);
            if(selectedVertices[0].name == selectedVertices[12].name){
                for(int k = 0; k <= 9; k+=3){
                    if((selectedVertices[k].GetComponent<ColorChanger>().prev.name == selectedVertices[k+1].GetComponent<ColorChanger>().prev.name) && 
                        (selectedVertices[k].GetComponent<ColorChanger>().prev.name == selectedVertices[k+2].GetComponent<ColorChanger>().prev.name)) {
                            foundHamiltonian = true;
                    } else {
                        resetAllVertices();
                        break;
                    }
                }
            } else {
                resetAllVertices();
            }
        }
        // check if wipe occured
        for(int i = 0; i < vertexCount; i++){
            if(vertices[i].GetComponent<ColorChanger>().wipe){
                resetAllVertices();
                break;
            }
        }
        for (int j = 0; j < vertexCount; j++){
            if(currentFilled == 11){
                Debug.Log("REACHED 12 VERTICES!");
                if(vertices[j].GetComponent<ColorChanger>().isLast){
                    currentFilled += 1;
                    selectedVertices[currentFilled] = vertices[j];
                    Debug.Log(vertices[j].name + "at index: " + currentFilled + " is last!");
                }
            }
            if(vertices[j].GetComponent<ColorChanger>().active && !vertices[j].GetComponent<ColorChanger>().contained){
                currentFilled += 1;
                selectedVertices[currentFilled] = vertices[j]; 
                vertices[j].GetComponent<ColorChanger>().contained = true;
                Debug.Log("Added Vertex: " + vertices[j].name + " at pos: " + currentFilled);
            }
        }
        return foundHamiltonian;
    }

    private void resetAllVertices() { // reset everything
        Debug.Log("Wiping vertices!");
        var vertices = GameObject.FindGameObjectsWithTag("vertex");
        var vertexCount = vertices.Length;
        for(int i = 0; i < vertexCount; i++){
            vertices[i].GetComponent<MeshRenderer>().material = vertices[i].GetComponent<ColorChanger>().prev;
            vertices[i].GetComponent<ColorChanger>().resetCurrent();
            vertices[i].GetComponent<ColorChanger>().active = false;
            selectedVertices[i] = null;
            vertices[i].GetComponent<ColorChanger>().wipe = false;
            vertices[i].GetComponent<ColorChanger>().isLast = false;
            vertices[i].GetComponent<ColorChanger>().contained = false;
            vertices[i].GetComponent<ColorChanger>().setIndex(-1);
        }
        selectedVertices[12] = null;
        currentFilled = -1;
    }

    public void checkHamiltionian(){
        if(foundHam) {
            door2_audioSrc.PlayOneShot(door_success);
            door2Animator.SetTrigger("opendoor2");
            clockAnimator.SetTrigger("moveclockto3");
            canvasAnimator.SetTrigger("movecanvas3");
        } else {
            door2_audioSrc.PlayOneShot(door_failure);
        }
    }

    public void checkDice(){
        if(dicePillar.GetComponent<DiceAttach>().getActive() && diceCount <= 3){
            door3_audioSrc.PlayOneShot(door_success);
            door3Animator.SetTrigger("opendoor3");
            won = true;
        }
        else if (!dicePillar.GetComponent<DiceAttach>().getActive() && diceCount <= 3) {
            door1_audioSrc.PlayOneShot(door_failure);
            diceCount += 1;
            if(diceCount == 3){
                cameraAudio.PlayOneShot(game_lost);
                gameLost = true;
            }  
        }
    }

    void Update() {
        if(GetComponent<TimeController>().gameOver){
            gameLost = true;
        }
        if(gameLost && !won){
            Invoke("restartGame", restartDelay);
        }
        if(foundPowerCores){
            if(!foundHam && checkForHamiltonian()){
                foundHam = true;
                cameraAudio.PlayOneShot(ham_victory);
                GetComponent<TimeController>().timeLimit += 300;
            }
        }
    }

    public void playPrize(){
        panel.SetActive(true);
        VideoPlayer vp = GameObject.FindObjectOfType<VideoPlayer>();
        vp.Play();
    }

    public void restartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quitGame(){
        Application.Quit();
    }
}
