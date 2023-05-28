#if !(UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || (UNITY_ANDROID && !UNITY_EDITOR))
#define OVRPLUGIN_UNSUPPORTED_PLATFORM
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using UnityEngine.XR.Management;

public class ControllerSwitcher : MonoBehaviour
{
    public GameObject keyboardController;
    public Transform keyboardHead;
    public GameObject XRRig;
    public Transform XRHead;
    public Transform XRLeftHand;
    public Transform XRRightHand;
    public Transform realtime;

    private void Awake()
    {
        StartCoroutine(StartXR());
    }

    public IEnumerator StartXR()
    {
        Debug.Log("HERE Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Directing to Normal Interaciton Mode...!.");
            StopXR();
            DirectToNormal();
        }
        else
        {
            Debug.Log("Initialization Finished.Starting XR Subsystems...");

            //Try to start all subsystems and check if they were all successfully started ( thus HMD prepared).
            bool loaderSuccess = XRGeneralSettings.Instance.Manager.activeLoader.Start();
            if (loaderSuccess)
            {
                Debug.Log("All Subsystems Started!");
            }
            else
            {
                Debug.LogError("Starting Subsystems Failed. Directing to Normal Interaciton Mode...!");
                StopXR();
                DirectToNormal();
            }
        }
    }

    void StopXR()
    {
         Debug.Log("XR stopped completely.");
    }
    void DirectToNormal()
    {
        Debug.Log("Fell back to Mouse & Keyboard Interaciton!");
    }

    /*

    public void OnAvatarCreated(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
    {
        if (!isLocalAvatar) return;

        avatar.localPlayer.root = XRRig.transform;
        avatar.localPlayer.head = XRHead;
        avatar.localPlayer.leftHand = XRLeftHand;
        avatar.localPlayer.rightHand = XRRightHand;

    }*/
}