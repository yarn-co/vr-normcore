using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using System;

public class XRSubSystems : MonoBehaviour
{
    public XRGeneralSettings xrSettings;
    public XRManagerSettings xrManager;
    public XRLoader xrLoader;
    public XRDisplaySubsystem xrDisplay;
    public XRInputSubsystem xrInput;
    public XRMeshSubsystem xrMesh;

    public void Start()
    {
        xrSettings = XRGeneralSettings.Instance;
        if (xrSettings == null)
        {
            Debug.Log($"XRGeneralSettings is null.");
            return;
        }

        xrManager = xrSettings.Manager;
        if (xrManager == null)
        {
            Debug.Log($"XRManagerSettings is null.");
            return;
        }

        xrLoader = xrManager.activeLoader;
        if (xrLoader == null)
        {
            Debug.Log($"XRLoader is null.");
            return;
        }

        Debug.Log($"Loaded XR Device: {xrLoader.name}");

        xrDisplay = xrLoader.GetLoadedSubsystem<XRDisplaySubsystem>();
        Debug.Log($"XRDisplay: {xrDisplay != null}");

        if (xrDisplay != null)
        {
            if (xrDisplay.TryGetDisplayRefreshRate(out float refreshRate))
            {
                Debug.Log($"Refresh Rate: {refreshRate}hz");
            }
        }

        xrInput = xrLoader.GetLoadedSubsystem<XRInputSubsystem>();
        Debug.Log($"XRInput: {xrInput != null}");

        if (xrInput != null)
        {
            //xrInput.TrySetTrackingOriginMode(TrackingOriginModeFlags.Device);
            //xrInput.TryRecenter();

            //xrInput.trackingOriginUpdated += RecenterTest;
        }

        xrMesh = xrLoader.GetLoadedSubsystem<XRMeshSubsystem>();
        Debug.Log($"XRMesh: {xrMesh != null}");
    }
}