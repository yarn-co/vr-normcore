using UnityEngine;
using Normal.Realtime;

public class NormPlayer : RealtimeComponent<NormPlayerModel>
{
    public NormPlayerModel _model;
    public Transform _character;
    private Rigidbody _rigidbody;
    private RealtimeView _realtimeView;
    private RealtimeTransform _realtimeTransform;
    
    public int team = 0;
    public int side = 1;

    private float scale = 1f;
    public float Scale
    {
        get
        {
            return scale;
        }
        set
        {
            Debug.Log("NormPlayer SetScale: " + value);

            scale = value;
            _model.scale = scale;
        }
    }

    protected override void OnRealtimeModelReplaced(NormPlayerModel previousModel, NormPlayerModel currentModel)
    {
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.teamDidChange -= TeamChanged;
            previousModel.scaleDidChange -= ScaleChanged;
        }

        if (currentModel != null)
        {
            _model = currentModel;
            _model.teamDidChange += TeamChanged;
            _model.scaleDidChange += ScaleChanged;

            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
            {
                _model.team = team;
            }
        }
    }
    private void ScaleChanged(NormPlayerModel model, float value)
    {
        //Debug.Log("TeamChanged: " + value);

        scale = value;
    }

    private void TeamChanged(NormPlayerModel model, int value)
    {
        //Debug.Log("TeamChanged: " + value);

        team = value;
    }

    private void Awake()
    {
        // Store a reference to the rigidbody for easy access
        _rigidbody = GetComponent<Rigidbody>();

        // Store a reference to the RealtimeView for easy access
        _realtimeView = GetComponent<RealtimeView>();

        _realtimeTransform = GetComponent<RealtimeTransform>();

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
        _realtimeTransform.RequestOwnership();
    }

    private void Update()
    {
        // Call LocalUpdate() only if this instance is owned by the local client
        if (_realtimeView.isOwnedLocallyInHierarchy)
            LocalUpdate();
    }

    private void LocalUpdate()
    {
        // Move the camera using the mouse
        //RotateCamera();

        // Use WASD input and the camera look direction to calculate the movement target
        //CalculateTargetMovement();

        // Check if we should jump this frame
        //CheckForJump();
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

}