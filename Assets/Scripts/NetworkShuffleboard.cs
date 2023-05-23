using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using TMPro;
using UnityEngine.InputSystem;
using Fusion;
using Fusion.XR.Shared.Grabbing;

[RequireComponent(typeof(Grabbable))]

public class NetworkShuffleboard : MonoBehaviour
{

    public NetworkObject puck;

    private GameObject[] pucks;

    public InputActionProperty secondaryButtonAction;

    public GameObject Rig;

    public Vector3 smallScale = new Vector3(0.1f, 0.1f, 0.1f);

    public NetworkObject Player;

    public bool buttonPress = false;

    void Awake()
    {
        Rig = GameObject.FindGameObjectWithTag("Rig");
    }

    void Start()
    {
        
        secondaryButtonAction.action.AddBinding("<XRController>{RightHand}/secondaryButton");
        secondaryButtonAction.action.AddBinding("<XRController>{LeftHand}/secondaryButton");
        secondaryButtonAction.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (secondaryButtonAction.action.ReadValue<float>() == 1)
        {
            if (!buttonPress)
            {
                buttonPress = true;

                Debug.Log("secondaryButton: " + secondaryButtonAction.action.ReadValue<float>());

                Player = Game.Instance.GetPlayer();

                if (Player != null)
                {
                    PlayerScale playerScale = Player.GetComponent<PlayerScale>();

                    if (playerScale.Scale == 1)
                    {
                        playerScale.Scale = 0.1f;
                    }
                    else
                    {
                        playerScale.Scale = 1;
                    }
                }
            }
        }
        else
        {
            buttonPress = false;
        }
}

    public void MakePuckForRedOne()
    {
        MakePuck(0,1);
    }

    public void MakePuckForBlueOne()
    {
        MakePuck(1,1);
    }

    public void MakePuckForRedTwo()
    {
        MakePuck(0, 2);
    }

    public void MakePuckForBlueTwo()
    {
        MakePuck(1, 2);
    }

    public void MakePuck(int team, int side)
    {
        float zPos = 0.4f;

        if(side == 2)
        {
            zPos = 3.2f;
        }

        NetworkObject newPuck = Game.Instance.runner.Spawn(puck, new Vector3(0, 1, zPos), Quaternion.identity);

        newPuck.GetComponent<NetworkPuck>().Team = team;
        newPuck.GetComponent<NetworkPuck>().Side = side;
    }

    public void OwnAllPucks()
    {
        Debug.Log("Give me all the pucks");

        GameObject[] pucks = GameObject.FindGameObjectsWithTag("Puck");

        foreach (GameObject puckX in pucks)
        {
            NetworkObject obj = puckX.GetComponent<NetworkPuck>().Object;

            NetworkGrabbable networkGrabbable = puckX.GetComponent<NetworkGrabbable>();

            Debug.Log("Current Grabber: " + (networkGrabbable.CurrentGrabber == null));

            if (!networkGrabbable.isTakingAuthority && networkGrabbable.CurrentGrabber == null)
            {
                Debug.Log("Requesting Authority of " + obj);
                
                obj.RequestStateAuthority();
            }
        }
    }

    public void ClearPucks()
    {

        pucks = GameObject.FindGameObjectsWithTag("Puck");

        foreach (GameObject puckX in pucks)
        {
            Destroy(puckX);
        }
    }

    public void Reset()
    {
        ClearPucks();

        //Game.Instance.GetComponent<Recenter>().Reset();
    }
}
