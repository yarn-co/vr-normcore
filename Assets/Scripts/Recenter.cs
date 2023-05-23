using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using TMPro;

[RequireComponent(typeof(XRSubSystems))]
public class Recenter : MonoBehaviour
{
    public GameObject Origin;

    private XROrigin originScript;

    private GameObject mainCamera;

    private XRSubSystems xrSystems;

    private bool started = false;

    private bool switching = false;

    public float height = 0f;

    private float cameraHeight = 0f;

    public float testHeight = 0f;

    private bool recentering = false;

    private History<float> HeightHistory = new(20);

    void Start()
    {
        //Debug.Log("Start");

        xrSystems = GetComponent<XRSubSystems>();

        originScript = Origin.GetComponent<XROrigin>();

        mainCamera = GameObject.FindWithTag("MainCamera");

        

        Reset();
    }

    void RecenterTest(XRInputSubsystem xrInput)
    {
        Debug.Log("RECENTER EVENT");

        if (!recentering)
        {
            Debug.Log("RESET ON RECENTER EVENT");

            Reset();
        }
    }

    // Update is called once per frame
    void Update()
    {
        testHeight = originScript.CameraInOriginSpaceHeight;

        HeightHistory.AddEntry(testHeight);

        if (!started)
        {
            if (xrSystems.xrInput != null)
            {
                Debug.Log("HAS XR INPUT SYSTEM");

                started = true;

                xrSystems.xrInput.trackingOriginUpdated += RecenterTest;

                //xrSystems.xrInput.TrySetTrackingOriginMode(TrackingOriginModeFlags.Device);
                //xrSystems.xrInput.TryRecenter();
            }
        }
        

        if (recentering)
        {
            if (originScript.CurrentTrackingOriginMode.ToString() == "Floor" && !switching){

                originScript.CameraYOffset = 0;

                height = originScript.CameraInOriginSpaceHeight;

                Debug.Log("Floor Mode Height: " + height);
            }

            if(originScript.CurrentTrackingOriginMode.ToString() == "Floor" && height > 0){

                Debug.Log("Switching to Device");

                switching = true;

                originScript.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
            }
            
            if(originScript.CurrentTrackingOriginMode.ToString() == "Device" && (height > 0 || cameraHeight != 0))
            {
                if (cameraHeight != 0)
                {
                    Debug.Log("Using Stored Camera Height");

                    float pastHeight = HeightHistory.Peek();

                    Debug.Log("Past Height: " + pastHeight);

                    height = pastHeight;

                    cameraHeight = 0;
                }

                Debug.Log("End Recentering: " + height);

                Debug.Log("Set Height: " + height);

                originScript.CameraYOffset = height;

                switching = false;

                recentering = false;
            }
        }
    }

    public void Reset()
    {
        recentering = true;

        cameraHeight = originScript.CameraInOriginSpaceHeight;

        Debug.Log("Camera Local Height at Reset: " + cameraHeight);

        //originScript.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
    }
}
