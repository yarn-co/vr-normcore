using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Puck : MonoBehaviour
{
    public Material TeamZeroMaterial;
    public Material TeamOneMaterial;

    private MeshRenderer myRenderer;
    private Rigidbody myRigidbody;

    public TextMeshPro text;

    public int team = 0;
    public int Team
    {
        get
        {
            return team;
        }
        set
        {
            SetTeam(value);
        }
    }

    public int side = 1;
    public int Side
    {
        get
        {
            return side;
        }
        set
        {
            side = value;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Start(){

        text.text = "";
    }

    public void SetTeam(int value)
    {
        Debug.Log("Puck SetTeam: " + value);

        team = value;

        if (value == 1)
        {
            myRenderer.material = TeamOneMaterial;
        }
        else
        {
            myRenderer.material = TeamZeroMaterial;
        }
    }

    public void FirstGrab()
    {
        myRigidbody.useGravity = true;
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
