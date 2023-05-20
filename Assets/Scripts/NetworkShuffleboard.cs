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

    void Awake()
    {

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
        if (secondaryButtonAction != null && secondaryButtonAction.action != null && secondaryButtonAction.action.ReadValue<float>() == 1)
        {
            OwnAllPucks();
            //Game.Instance.GetComponent<Recenter>().Reset();
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

            Debug.Log("Current Grabber: " + puckX.GetComponent<Grabbable>().currentGrabber);

            //if (!obj.HasStateAuthority && puckX.GetComponent<Grabbable>().currentGrabber != null)
            //{
                obj.RequestStateAuthority();
            //}
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

        Game.Instance.GetComponent<Recenter>().Reset();
    }
}
