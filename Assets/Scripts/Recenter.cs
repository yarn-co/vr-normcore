using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using TMPro;

public class Recenter : MonoBehaviour
{
    public GameObject Origin;

    private XROrigin originScript;

    private GameObject mainCamera;

    private bool started = false;

    private float height = 0f;

    private float offsetHeight = 0f;

    void Awake(){
        //Debug.Log("Awake: " + Game.Instance.started);
    }

    void Start()
    {
        //Debug.Log("Start");

        originScript = Origin.GetComponent<XROrigin>();
        
        started = false;

        height = 0;

        //Debug.Log("Tracking Mode: " + originScript.CurrentTrackingOriginMode);

        mainCamera = GameObject.FindWithTag("MainCamera");

        offsetHeight = mainCamera.transform.localPosition.y;

        originScript.CameraYOffset = 0;

        originScript.Camera.transform.position = new Vector3(0f, 0f, 0f);

        originScript.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Floor;
    }

    // Update is called once per frame
    void Update()
    {
        if(!started){

            if(originScript.CurrentTrackingOriginMode.ToString() == "Floor"){
                    
                height = originScript.CameraInOriginSpaceHeight;

                //Debug.Log("Floor Mode Height: " + height);
            }

            if(originScript.CurrentTrackingOriginMode.ToString() == "Floor" && height > 0f){

                originScript.RequestedTrackingOriginMode = XROrigin.TrackingOriginMode.Device;
            }
            
            if(originScript.CurrentTrackingOriginMode.ToString() == "Device" && height > 0f){
                
                //Debug.Log("Set Height: " + height + " - " + offsetHeight);

                originScript.CameraYOffset = height - offsetHeight;

                originScript.Camera.transform.position = new Vector3(0f, originScript.CameraYOffset, 0f);

                //Origin.transform.position = new Vector3(0f, 0f, 0f);

                Game.Instance.started = true;

                started = true;
            }

        }
    }

    public void Reset()
    {
        Start();
    }
}
