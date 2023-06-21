using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using TMPro;
using UnityEngine.InputSystem;
using Spacebar;
using Spacebar.Realtime;
using Normal.Realtime;

public class NormShuffleboard : MonoBehaviour
{
    public GameObject puck;

    private GameObject[] pucks;

    public InputActionProperty secondaryButtonAction;

    public GameObject XRRig;

    public Vector3 smallScale = new Vector3(0.1f, 0.1f, 0.1f);

    public Spacebar.Realtime.RealtimeAvatarManager RealtimeAvatarManager;
    public Spacebar.Realtime.RealtimeAvatar localAvatar;

    public bool buttonPress = false;

    private Normal.Realtime.Realtime Realtime;

    void Awake()
    {
        Realtime = GetComponent<Normal.Realtime.Realtime>();

        XRRig = GameObject.FindGameObjectWithTag("XRRig");

        RealtimeAvatarManager = GetComponent<Spacebar.Realtime.RealtimeAvatarManager>();
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

                localAvatar = RealtimeAvatarManager.localAvatar;

                if (localAvatar != null)
                {
                    Debug.Log("Shrink/Grow");

                    /*
                    PlayerScale playerScale = localAvatar.GetComponent<PlayerScale>();

                    if (playerScale.Scale == 1)
                    {
                        playerScale.Scale = 0.1f;
                    }
                    else
                    {
                        playerScale.Scale = 1;
                    }*/
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

        GameObject newPuck = Normal.Realtime.Realtime.Instantiate("NormPuck", new Normal.Realtime.Realtime.InstantiateOptions
        {
            ownedByClient = false,
            preventOwnershipTakeover = false,
            destroyWhenOwnerLeaves = false,
            destroyWhenLastClientLeaves = true,
            useInstance = Realtime,
        });

        newPuck.GetComponent<RealtimeTransform>().RequestOwnership();

        newPuck.transform.position = new Vector3(0, 1, zPos);
        newPuck.GetComponent<Puck>().team = team;
        //newPuck.GetComponent<Puck>().side = side;
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
