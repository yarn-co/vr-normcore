using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;
using Fusion.XR.Shared.Grabbing;

[RequireComponent(typeof(NetworkGrabbable))]
public class NetworkPuck : NetworkBehaviour
{
    public Material TeamZeroMaterial;
    public Material TeamOneMaterial;

    private MeshRenderer myRenderer;

    public TextMeshPro text; 

    public int side = 1;

    [Networked(OnChanged = nameof(TeamChanged))]
    public int Team { get; set; }

    private static void TeamChanged(Changed<NetworkPuck> changed)
    {
        Debug.Log("TeamChanged: " + changed.Behaviour.Team);

        changed.Behaviour.SetTeam(changed.Behaviour.Team);
    }

    // Start is called before the first frame update
    void Awake()
    {
        myRenderer = gameObject.GetComponentInChildren<MeshRenderer>();

        var grabbable = GetComponent<NetworkGrabbable>();

        grabbable.onDidGrab.AddListener(OnDidGrab);
        grabbable.grabbable.onWillGrab.AddListener(OnWillGrab);
    }

    void Start(){

        Debug.Log("NetworkPuck Start");

        SetTeam(Team);

        text.text = "";
    }

    public void SetTeam(int t)
    {
        if (t == 1)
        {
            myRenderer.material = TeamOneMaterial;
        }
        else
        {
            myRenderer.material = TeamZeroMaterial;
        }
    }

    void OnDidGrab(NetworkGrabber newGrabber)
    {
        
    }

    void OnWillGrab(Grabber newGrabber)
    {
        Debug.Log("Give me all the pucks");

        GameObject[] pucks = GameObject.FindGameObjectsWithTag("Puck");

        foreach (GameObject puckX in pucks)
        {
            NetworkObject obj = puckX.GetComponent<NetworkPuck>().Object;

            if (!obj.HasStateAuthority)
            {
                obj.RequestStateAuthority();
            }
        }
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
