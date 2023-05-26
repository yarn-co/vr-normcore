using UnityEngine;
using UnityEngine.Animations;
using Normal.Realtime;

public class NormPlayerManager : MonoBehaviour
{
    [SerializeField] public GameObject _camera = default;

    private Realtime _realtime;

    private void Awake()
    {
        // Get the Realtime component on this game object
        _realtime = GetComponent<Realtime>();

        // Notify us when Realtime successfully connects to the room
        _realtime.didConnectToRoom += DidConnectToRoom;
    }

    private void DidConnectToRoom(Realtime realtime)
    {
        Realtime.InstantiateOptions options = new Realtime.InstantiateOptions();
        options.ownedByClient = true;
        options.preventOwnershipTakeover = true;
        options.destroyWhenOwnerLeaves = true;
        options.destroyWhenLastClientLeaves = true;
        options.useInstance = _realtime;

        // Instantiate the Player for this client once we've successfully connected to the room
        GameObject playerGameObject = Realtime.Instantiate("NormPlayer", options);

        // Get a reference to the player
        NormPlayer player = playerGameObject.GetComponent<NormPlayer>();

        // Get the constraint used to position the camera behind the player
        ParentConstraint cameraConstraint = _camera.GetComponent<ParentConstraint>();

        // Add the camera target so the camera follows it
        ConstraintSource constraintSource = new ConstraintSource { sourceTransform = player.cameraTarget, weight = 1.0f };
        int constraintIndex = cameraConstraint.AddSource(constraintSource);

        // Set the camera offset so it acts like a third-person camera.
        cameraConstraint.SetTranslationOffset(constraintIndex, new Vector3(0.0f, 1.0f, -4.0f));
        cameraConstraint.SetRotationOffset(constraintIndex, new Vector3(15.0f, 0.0f, 0.0f));
    }
}