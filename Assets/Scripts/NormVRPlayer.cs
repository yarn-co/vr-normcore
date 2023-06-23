using UnityEngine;
using Normal.Realtime;
using Rewired;
using Unity.XR.CoreUtils;
using static UnityEngine.UI.Image;
using UnityEngine.XR.Interaction.Toolkit;

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

        if (ReInput.players != null)
        {
            _rewiredPlayer = ReInput.players.GetPlayer(playerId);
        }
    }

    public void OnScaleChange()
    {
        Debug.Log("NormVRPlayer OnScaleChange: " + _normPlayer.Scale);

        float newScale = _normPlayer.Scale;

        Vector3 scaleVector = new(newScale, newScale, newScale);

        if (_realtimeView.isOwnedLocallyInHierarchy)
        {
            _XROrigin.transform.localScale = scaleVector;
            //behavior.Origin.CameraFloorOffsetObject.transform.localPosition = new Vector3(0, behavior.Origin.CameraYOffset * newScale, 0);
        }

        //player.transform.localScale = scaleVector;
    }

    public void OnColorChange()
    {
        SkinnedMeshRenderer leftHandMesh = _realtimeAvatar.leftHand.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

        leftHandMesh.material.color = _colorSync._model.color;

        SkinnedMeshRenderer rightHandMesh = _realtimeAvatar.rightHand.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

        rightHandMesh.material.color = _colorSync._model.color;
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