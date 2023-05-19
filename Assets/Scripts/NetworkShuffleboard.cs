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

    public void MakePuckForZero()
    {
        MakePuck(0);
    }

    public void MakePuckForOne()
    {
        MakePuck(1);
    }

    public void MakePuck(int team)
    {

        //GameObject newPuck = Instantiate(puck, new Vector3(0, 1, 0.3f), Quaternion.identity);

        //newPuck.GetComponent<Puck>().team = team;

        NetworkObject newPuck = Game.Instance.runner.Spawn(puck, new Vector3(0, 1, 0.3f), Quaternion.identity);

        newPuck.GetComponent<NetworkPuck>().Team = team;
    }

    public void OwnAllPucks()
    {
        Debug.Log("Give me all the pucks");

        GameObject[] pucks = GameObject.FindGameObjectsWithTag("Puck");

        foreach (GameObject puckX in pucks)
        {
            NetworkObject obj = puckX.GetComponent<NetworkPuck>().Object;

            if (!obj.HasStateAuthority && puckX.GetComponent<Grabbable>().currentGrabber != null)
            {
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

        Game.Instance.GetComponent<Recenter>().Reset();
    }
}
