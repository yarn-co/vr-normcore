using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puck : MonoBehaviour
{
    public Material TeamZeroMaterial;
    public Material TeamOneMaterial;

    private MeshRenderer myRenderer;

    public TextMeshPro text; 

    public int side = 1;

    public int team;
    /*
    public int team {
        get { return _team; }
        set {
            _team = value;
            if(value == 1){
                myRenderer.material = TeamOneMaterial;
            }else{
                myRenderer.material = TeamZeroMaterial;
            }
        }
    }*/



    // Start is called before the first frame update
    void Awake()
    {
        myRenderer = GetComponent<MeshRenderer>();
    }

    void Start(){

        if(team == 1){
            myRenderer.material = TeamOneMaterial;
        }else{
            myRenderer.material = TeamZeroMaterial;
        }

        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ScoreZone"){

            var scoreZone = other.GetComponent<ScoreZone>();

            if(scoreZone.side != side){

                Debug.Log("Triggered " + other.tag + " " + scoreZone.points);

                text.text = scoreZone.points.ToString();
            }
        }
    }
}
