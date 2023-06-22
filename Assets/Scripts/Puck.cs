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
    public int team = 0;

    // Start is called before the first frame update
    void Awake()
    {
        myRenderer = GetComponent<MeshRenderer>();
    }

    void Start(){

        text.text = "";
    }

    public void SetTeam(int newTeam)
    {
        Debug.Log("Puck SetTeam: " +  newTeam);

        team = newTeam;

        if (newTeam == 1)
        {
            myRenderer.material = TeamOneMaterial;
        }
        else
        {
            myRenderer.material = TeamZeroMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ScoreZone"){

            var scoreZone = other.GetComponent<ScoreZone>();

            if(scoreZone.side != side){

                Debug.Log("Triggered Puck Side: " + side + " scoreZone Side " + side);

                text.text = scoreZone.points.ToString();
            }
        }
    }
}
