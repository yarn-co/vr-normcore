#if !(UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || (UNITY_ANDROID && !UNITY_EDITOR))
#define XR_UNSUPPORTED_PLATFORM
#endif

using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;
using Spacebar.Realtime;

public class PlayerModeSwitcher : MonoBehaviour
{
    public bool forceDesktop = false;
    public GameObject keyboardController;
    public Transform keyboardHead;
    public GameObject keyboardAvatarPrefab;

    public GameObject XRRig;
    public GameObject XRAvatarPrefab;
    public Transform XRHead;
    public Transform XRLeftHand;
    public Transform XRRightHand;
    
    public Spacebar.Realtime.RealtimeAvatarManager avatarManager;
    public bool isXRMode = false;

    private void Awake()
    {
        StartCoroutine(StartXR());
        StartCoroutine(RequestMicrophone());
    }

    public void Start()
    {
        avatarManager.avatarCreated += OnAvatarCreated;
    }

    private void OnDestroy()
    {
        StopXR();
    }

    public IEnumerator RequestMicrophone()
    {
        // Request microphone permission
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);

        // Check microphone permission
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            Debug.Log("Microphone found");
        }
        else
        {
            Debug.Log("Microphone not found");
        }

    }

    public IEnumerator StartXR()
    {
        if (forceDesktop)
        {
            NotXRMode();
            yield break;
        }
        
        Debug.Log("HERE Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Directing to Normal Interaciton Mode...!.");
            StopXR();
            NotXRMode();
        }
        else
        {
            Debug.Log("Initialization Finished.Starting XR Subsystems...");

            //Try to start all subsystems and check if they were all successfully started ( thus HMD prepared).
            bool loaderSuccess = XRGeneralSettings.Instance.Manager.activeLoader.Start();
            if (loaderSuccess)
            {
                Debug.Log("All Subsystems Started!");
                XRMode();
            }
            else
            {
                Debug.LogError("Starting Subsystems Failed. Directing to Normal Interaciton Mode...!");
                StopXR();
                NotXRMode();
            }
        }
    }

    void StopXR()
    {
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader)
        {
            XRGeneralSettings.Instance.Manager.activeLoader.Stop();
        }
        Debug.Log("XR stopped completely.");
    }

    void NotXRMode()
    {
        Debug.Log("NOT XR MODE!");

        isXRMode = false;
        XRRig.SetActive(false);
        keyboardController.SetActive(true);

        avatarManager.localAvatarPrefab = keyboardAvatarPrefab;
    }

    void XRMode()
    {
        Debug.Log("YAAAAY XR MODE!");

        isXRMode = true;
        XRRig.SetActive(true);
        keyboardController.SetActive(false);

        avatarManager.localAvatarPrefab = XRAvatarPrefab;
    }

    public void OnAvatarCreated(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
    {
        Debug.Log("YAAAAY Avatar Created!");

        if (!isLocalAvatar) return;

        if (isXRMode)
        { 
            avatar.localPlayer.root = XRRig.transform;
            avatar.localPlayer.head = XRHead;
            avatar.localPlayer.leftHand = XRLeftHand;
            avatar.localPlayer.rightHand = XRRightHand;
        }
    }
}