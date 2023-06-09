using UnityEngine;
using Normal.Realtime;
using Rewired;

public class NormVRPlayer : MonoBehaviour
{
    public int playerId = 0;
    private Player player; //Rewired player

    // Physics
    private Vector3 _targetMovement;
    private Vector3 _movement;

    private bool _jumpThisFrame;
    private bool _jumping;

    private Rigidbody _rigidbody;

    private RealtimeView _realtimeView;

    private ColorSync _colorSync;

    [SerializeField] private Transform _character = default;

    public float speed = 6f;

    private Vector3 inputMovement = new();
    private Vector2 moveVector = new();

    private void Awake()
    {
        // Set physics timestep to 60hz
        Time.fixedDeltaTime = 1.0f / 60.0f;

        // Store a reference to the rigidbody for easy access
        _rigidbody = GetComponent<Rigidbody>();

        // Store a reference to the RealtimeView for easy access
        _realtimeView = GetComponent<RealtimeView>();

        _colorSync = GetComponent<ColorSync>();

        player = ReInput.players.GetPlayer(playerId);
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

    private void Update()
    {
        // Call LocalUpdate() only if this instance is owned by the local client
        if (_realtimeView.isOwnedLocallyInHierarchy)
            LocalUpdate();
    }

    private void LocalUpdate()
    {
        GetInput();

        // Use WASD input and the camera look direction to calculate the movement target
        CalculateTargetMovement();

        // Check if we should jump this frame
        CheckForJump();
    }

    private void GetInput()
    {
        // Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
        // whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.

        moveVector.x = player.GetAxis("Move Horizontal"); // get input by name or action id
        moveVector.y = player.GetAxis("Move Vertical");

        //Debug.Log("Move: " + moveVector);

        if (player.GetButtonDown("Jump"))
        {
            //Jump();
        }

        CalculateTargetMovement();
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
        MovePlayer();

        // Animate the character to match the player movement
        //AnimateCharacter();
    }

    private void CalculateTargetMovement()
    {
        // Get input movement. Multiple by 6.0 to increase speed.
        Vector3 inputMovement = new Vector3();
        inputMovement.x = moveVector.x * 6.0f;
        inputMovement.z = moveVector.y * 6.0f;

        // Use the camera look direction to convert the input movement from camera space to world space
        _targetMovement = inputMovement;
    }

    private void CheckForJump()
    {
        // Jump if the space bar was pressed this frame and we're not already jumping, trigger the jump
        if (Input.GetKeyDown(KeyCode.Space) && !_jumping)
            _jumpThisFrame = true;
    }

    private void MovePlayer()
    {
        // Start with the current velocity
        Vector3 velocity = _rigidbody.velocity;

        // Smoothly animate towards the target movement velocity
        _movement = Vector3.Lerp(_movement, _targetMovement, Time.fixedDeltaTime * 5.0f);
        velocity.x = _movement.x;
        velocity.z = _movement.z;

        // Jump
        if (_jumpThisFrame)
        {
            // Instantaneously set the vertical velocity to 6.0 m/s
            velocity.y = 6.0f;

            // Mark the player as currently jumping and clear the jump input
            _jumping = true;
            _jumpThisFrame = false;
        }

        // Reset jump after the apex
        if (_jumping && velocity.y < -0.1f)
            _jumping = false;

        // Set the velocity on the rigidbody
        //_rigidbody.velocity = velocity;
    }

    // Rotate the character to face the direction we're moving. Lean towards the target movement direction.
    private void AnimateCharacter()
    {
        
    }

    // Given a forward vector, get a y-axis rotation that points in the same direction that's parallel to the ground plane
    private static Vector3 ProjectVectorOntoGroundPlane(Vector3 vector)
    {
        Vector3 planeNormal = Vector3.up;
        Vector3.OrthoNormalize(ref planeNormal, ref vector);
        return vector;
    }

    // Get the rigidbody velocity along the ground plane
    private static float GetRigidbodyForwardVelocity(Rigidbody rigidbody)
    {
        Vector3 forwardVelocity = rigidbody.velocity;
        forwardVelocity.y = 0.0f;
        return forwardVelocity.magnitude;
    }

    // Get the difference between two angles along the ground plane
    private static float SignedAngle2D(Vector3 a, Vector3 b)
    {
        float angle = Mathf.Atan2(a.z, a.x) - Mathf.Atan2(b.z, b.x);
        if (angle <= -Mathf.PI) angle += 2.0f * Mathf.PI;
        if (angle > Mathf.PI) angle -= 2.0f * Mathf.PI;
        return angle;
    }
}