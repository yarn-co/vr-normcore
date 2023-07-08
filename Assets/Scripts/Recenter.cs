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

    private XROrigin origin;

    private GameObject mainCamera;

    private XRSubSystems xrSystems;

    private bool started = false;

    private bool firstTime = true;

    private bool switching = false;

    public float height = 0f;

    private float cameraHeight = 0f;

    public float testHeight = 0f;

    private bool recentering = false;

    private History<float> HeightHistory = new(20);

    void Start()
    {
        Debug.Log("Recenter Start");

        xrSystems = GetComponent<XRSubSystems>();

        origin = Origin.GetComponent<XROrigin>();

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
        testHeight = origin.CameraInOriginSpaceHeight;

        if (!started)
        {
            if (xrSystems.xrInput != null)
            {
                Debug.Log("HAS XR INPUT SYSTEM! Camera Height: " + testHeight);

                started = true;

                xrSystems.xrInput.trackingOriginUpdated += RecenterTest;

                //xrSystems.xrInput.TrySetTrackingOriginMode(TrackingOriginModeFlags.Device);
                //xrSystems.xrInput.TryRecenter();
            }
        }
        

        if (recentering)
        {
            //Debug.Log("Recentering...");

            if (origin.CurrentTrackingOriginMode.ToString() == "Floor" && !switching){

                origin.CameraYOffset = 0;

                height = origin.CameraInOriginSpaceHeight;

                Debug.Log("Floor Mode Height: " + height);

                if(height > 0)
                {
                    HeightHistory.AddEntry(height);
                }
            }

            if(origin.CurrentTrackingOriginMode.ToString() == "Floor" && height > 0){

                Debug.Log("Switching to Device");

                switching = true;

                origin.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
            }
            
            if(origin.CurrentTrackingOriginMode.ToString() == "Device" && (height > 0 || cameraHeight != 0))
            {
                if (cameraHeight != 0)
                {
                    Debug.Log("Using Stored Camera Height");

                    if (HeightHistory.data.Count > 0)
                    {
                        float pastHeight = HeightHistory.Peek();

                        Debug.Log("Past Height: " + pastHeight);

                        height = pastHeight;

                        cameraHeight = 0;
                    }
                }

                

                //Debug.Log("Set Height: " + height);

                origin.CameraYOffset = height;

                if (firstTime)
                {
                    //origin.transform.position = new Vector3(origin.transform.position.x, 0, origin.transform.position.z);

                    origin.MoveCameraToWorldLocation(new Vector3(0, origin.CameraInOriginSpacePos.y, 0));

                    firstTime = false;
                }
                switching = false;

                recentering = false;

                Debug.Log("End Recentering: " + height);
            }
        }
        else
        {
            HeightHistory.AddEntry(testHeight);
        }
    }

    public void Reset()
    {
        recentering = true;

        cameraHeight = origin.CameraInOriginSpaceHeight;

        //HeightHistory.AddEntry(cameraHeight);

        Debug.Log("Camera Local Height at Reset: " + cameraHeight);
    }
}
