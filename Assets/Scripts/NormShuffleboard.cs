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
using UnityEngine.UIElements;

public class NormShuffleboard : MonoBehaviour
{
    public GameObject puck;

    private GameObject[] pucks;

    public InputActionProperty secondaryButtonAction;

    public GameObject XRRig;

    public Spacebar.Realtime.RealtimeAvatarManager RealtimeAvatarManager;
    public Spacebar.Realtime.RealtimeAvatar localAvatar;

    public bool buttonPress = false;

    private float[] scales = { 1f, 0.05f, 1f, 5f};
    private int currentScale = 0;

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
                    NormPlayer normPlayer = localAvatar.GetComponent<NormPlayer>();

                    currentScale++;

                    if (currentScale >= scales.Length) currentScale = 0;

                    normPlayer.Scale = scales[currentScale];
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

        Vector3 newPosition = new Vector3(0, 1f, zPos);

        Debug.Log("Making Puck for Team: " + team + " Side: " + side);

        newPuck.transform.position = newPosition;
        newPuck.GetComponent<Rigidbody>().position = newPosition;
        newPuck.GetComponent<NormPuck>().Team = team;
        newPuck.GetComponent<NormPuck>().Side = side;
    }

    public void ClearPucks()
    {

        pucks = GameObject.FindGameObjectsWithTag("Puck");

        foreach (GameObject puckX in pucks)
        {
            //puckX.GetComponent<RealtimeTransform>().RequestOwnership();
            Realtime.Destroy(puckX);
        }
    }

    public void Reset()
    {
        ClearPucks();

        //Game.Instance.GetComponent<Recenter>().Reset();
    }
}
