using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject[] vertices;
    public Material shaderMaterial;
    public int lengthOfLineRenderer = 21;
    public float widthMul = 0.2f;
    // Start is called before the first frame update
    void Awake()
    {
        LineRenderer myLineDrawer = gameObject.AddComponent<LineRenderer>();
        myLineDrawer.positionCount = lengthOfLineRenderer;
        myLineDrawer.material = shaderMaterial;
        myLineDrawer.widthMultiplier = widthMul;
        /*
        for(int i = 0; i < vertices.Length; i++){
            lr.SetPosition(i, vertices[i].transform.position);
            if(i != vertices.Length)
                lr.SetPosition(i+1, vertices[i+1].transform.position);
        }
        */
        //lr.SetPosition(0, start);
        //lr.SetPosition(1, end);
        //GameObject.Destroy(myLine, duration);
    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        var points = new Vector3[lengthOfLineRenderer];
        for(int i = 0; i < lengthOfLineRenderer; i++){
            points[i] = vertices[i].transform.position;
        }
        lr.SetPositions(points);
    }
}
