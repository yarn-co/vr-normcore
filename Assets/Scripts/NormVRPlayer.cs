using UnityEngine;
using Normal.Realtime;
using Rewired;
using Unity.XR.CoreUtils;
using static UnityEngine.UI.Image;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;

public class NormVRPlayer : MonoBehaviour
{
    public int playerId = 0;

    private Player _rewiredPlayer; //Rewired player

    private NormPlayer _normPlayer;

    private RealtimeView _realtimeView;

    private Spacebar.Realtime.RealtimeAvatar _realtimeAvatar;

    private ColorSync _colorSync;

    [SerializeField] private Transform _character = default;

    private GameObject _controller;

    private GameObject _XRRig; 

    private XROrigin _XROrigin;

    private TeleportationProvider _teleportationProvider;

    private void Awake()
    {
        // Store a reference to the RealtimeView for easy access
        _realtimeView = GetComponent<RealtimeView>();
        _realtimeAvatar = GetComponent<Spacebar.Realtime.RealtimeAvatar>();

        _normPlayer = GetComponent<NormPlayer>();
        _normPlayer.onScaleChange += OnScaleChange;

        _colorSync = GetComponent<ColorSync>();
        _colorSync.onColorChange += OnColorChange;

        _controller = GameObject.FindGameObjectWithTag("GameController");
        _XROrigin = _controller.GetComponentInChildren<XROrigin>();
        _XRRig = GameObject.FindGameObjectWithTag("XRRig");

        _teleportationProvider = _XROrigin.GetComponent<TeleportationProvider>();
        _teleportationProvider.beginLocomotion += OnTeleportStart;
        _teleportationProvider.endLocomotion += OnTeleportEnd;

        if (ReInput.players != null)
        {
            _rewiredPlayer = ReInput.players.GetPlayer(playerId);
        }
    }

    public void OnTeleportStart(LocomotionSystem system)
    {
        Debug.Log("Teleport Start");
    }

    public void OnTeleportEnd(LocomotionSystem system)
    {
        Debug.Log("Teleport End");

        if (_normPlayer.Scale != 1f)
        {
            Debug.Log("Teleport Adjust for Scale");

            Vector3 cameraInOriginSpace = _XROrigin.CameraInOriginSpacePos;

            Vector3 cameraInOriginSpaceScaled = cameraInOriginSpace * _normPlayer.Scale;

            Vector3 originPosition = _XROrigin.transform.position;

            Vector3 fixedPosition =  originPosition + (_XROrigin.transform.rotation * (cameraInOriginSpace - cameraInOriginSpaceScaled));

            //Debug.Log("cameraInOriginSpace: " + cameraInOriginSpace);
            //Debug.Log("cameraInOriginSpaceScaled: " + cameraInOriginSpaceScaled);
            //Debug.Log("fixedPosition: " + fixedPosition);

            _XROrigin.transform.position = new Vector3(fixedPosition.x, originPosition.y, fixedPosition.z);
        }
    }

    public void OnScaleChange()
    {
        //Debug.Log("NormVRPlayer OnScaleChange: " + _normPlayer.Scale);

        float newScale = _normPlayer.Scale;

        Vector3 scaleVector = new(newScale, newScale, newScale);

        if (_realtimeView.isOwnedLocallyInHierarchy)
        {
            Vector3 newPosition = _XROrigin.Camera.transform.position;

            Vector3 cameraInOriginSpace = _XROrigin.CameraInOriginSpacePos;

            Vector3 cameraInOriginSpaceScaled = _XROrigin.transform.rotation * (cameraInOriginSpace * newScale);

            newPosition -= cameraInOriginSpaceScaled;

            GameObject cameraOffset = _XROrigin.CameraFloorOffsetObject;

            //Debug.Log("cameraInOriginSpace: " + cameraInOriginSpace + " cameraInOriginSpaceScaled: " + cameraInOriginSpaceScaled);

            _XROrigin.transform.localScale = scaleVector;

            Vector3 originPositionAfterScale = _XROrigin.transform.position;

            _XROrigin.transform.position = new Vector3(newPosition.x, originPositionAfterScale.y, newPosition.z);
        }

        //player.transform.localScale = scaleVector;
    }

    public void OnColorChange()
    {
        SkinnedMeshRenderer leftHandMesh = _realtimeAvatar.leftHand.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

        leftHandMesh.material.color = _colorSync._model.color;

        SkinnedMeshRenderer rightHandMesh = _realtimeAvatar.rightHand.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

        rightHandMesh.material.color = _colorSync._model.color;

        if (_realtimeView.isOwnedLocallyInHierarchy)
        {
            XRHand[] localHands = _XROrigin.GetComponentsInChildren<XRHand>();
            
            foreach(XRHand localHand in localHands)
            {
                SkinnedMeshRenderer handMesh = localHand.GetComponentInChildren<SkinnedMeshRenderer>();
                handMesh.material.color = _colorSync._model.color;
            }

            //Debug.Log("Changing Color of Local Player Hands: " + localHands);
        }
    }

    private void Start()
    {
        // Call LocalStart() only if this instance is owned by the local client
        if (_realtimeView.isOwnedLocallyInHierarchy)
            LocalStart();
    }

    private void LocalStart()
    {
        // Request ownership of the Player and the character RealtimeTransforms
        GetComponent<RealtimeTransform>().RequestOwnership();

        _character.GetComponent<RealtimeTransform>().RequestOwnership();

        _colorSync.SetColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f));
    }

    /*
    private void Update()
    {
        // Call LocalUpdate() only if this instance is owned by the local client
        if (_realtimeView.isOwnedLocallyInHierarchy)
            //LocalUpdate();
    }

    private void FixedUpdate()
    {
        // Call LocalUpdate() only if this instance is owned by the local client
        if (_realtimeView.isOwnedLocallyInHierarchy)
            LocalFixedUpdate();
    }

    private void LocalFixedUpdate()
    {
        // Move the player based on the input
        //MovePlayer();

        // Animate the character to match the player movement
        //AnimateCharacter();
    }
    */
   
}