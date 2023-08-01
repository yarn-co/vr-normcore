#if !(UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || (UNITY_ANDROID && !UNITY_EDITOR))
#define XR_UNSUPPORTED_PLATFORM
#endif

using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;
using Spacebar.Realtime;
using TMPro;

public class PlayerModeSwitcher : MonoBehaviour
{
    public bool screenMode = false;
    public GameObject screenController;
    public Transform screenHead;
    public GameObject screenAvatarPrefab;

    public float screenFixedUpdateSpeed;
    public float screenFixedUpdateMax;

    public float XRFixedUpdateSpeed;
    public float XRFixedUpdateMax;

    public GameObject XRController;
    public GameObject XRRig;
    public GameObject XRAvatarPrefab;
    public Transform XRHead;
    public Transform XRLeftHand;
    public Transform XRRightHand;
    
    public Spacebar.Realtime.RealtimeAvatarManager avatarManager;
    public bool isXRMode = false;

    public TMP_Text initialText;
    public GameObject arrowPrefab;  // Assign this in the Inspector
    private GameObject arrowInstance;
    public GameObject arrowTarget;
    private Transform cameraTransform;

    private void Awake()
    {
        // Set physics timestep
        if (screenMode)
        {
            Time.fixedDeltaTime = 1.0f / screenFixedUpdateSpeed;
            Time.maximumDeltaTime = 1.0f / screenFixedUpdateMax;
        }
        else
        {
            Time.fixedDeltaTime = 1.0f / XRFixedUpdateSpeed;
            Time.maximumDeltaTime = 1.0f / XRFixedUpdateMax;
        }

        
        StartCoroutine(StartXR());
        StartCoroutine(RequestMicrophone());
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            Debug.Log("Application is focussed");
        }
        else
        {
            Debug.Log("Application lost focus");
        }
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
        if (screenMode)
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
        XRController.SetActive(false);
        screenController.SetActive(true);

        avatarManager.localAvatarPrefab = screenAvatarPrefab;
    }

    void XRMode()
    {
        Debug.Log("YAAAAY XR MODE!");

        isXRMode = true;
        XRController.SetActive(true);
        screenController.SetActive(false);

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

            cameraTransform = Camera.main.transform;
            StartCoroutine(DisplayInitialText());
            StartCoroutine(SpawnInitialArrow(arrowTarget.transform));
            
            /*
            // Get the PlayerRespawn script from the avatar's GameObject.
            PlayerRespawn respawnScript = avatar.gameObject.GetComponent<PlayerRespawn>();
            // If the avatar has a PlayerRespawn script, set its xrRig field.
            if (respawnScript != null)
            {
                respawnScript.SetXRRig(XRRig.transform);
            }
            else
            {
                Debug.LogWarning("Avatar does not have a PlayerRespawn script.");
            }
            */
        }
    }

    public void RemoveArrow()
    {
        if (arrowInstance != null)
        {
            Destroy(arrowInstance);
            arrowInstance = null;
        }
    }

    IEnumerator SpawnInitialArrow(Transform targetTransform)
    {
        arrowInstance = Instantiate(arrowPrefab, cameraTransform);
        arrowInstance.transform.localPosition = new Vector3(0, 0, 3);
        arrowInstance.transform.localRotation = Quaternion.identity;
        Debug.Log("Arrow instantiated." + arrowInstance.transform);

        ArrowPointer arrowPointer = arrowInstance.GetComponent<ArrowPointer>();
        arrowPointer.SetTarget(targetTransform);

        yield return new WaitForSeconds(20f);

        RemoveArrow();
    }

    IEnumerator DisplayInitialText()
    {
        initialText.text = "Welcome to Level 1! <color=black>Your objective is to reach the green platform.</color>";
        yield return new WaitForSeconds(10.0f);
        initialText.text = "";
    }
}