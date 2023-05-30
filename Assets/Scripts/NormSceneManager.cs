using UnityEngine;
using UnityEngine.Animations;
using Normal.Realtime;

public class NormSceneManager : MonoBehaviour
{
    private Realtime _realtime;

    private RealtimeAvatar _avatar;

    private RealtimeAvatarManager _avatarManager;

    private PlayerModeSwitcher _modeSwitcher;

    private Camera _camera;

    private RigidFollower rightFollower;
    private RigidFollower leftFollower;

    private void Awake()
    {
        // Get the Realtime component on this game object
        _realtime = GetComponent<Realtime>();

        _avatarManager = _realtime.GetComponent<RealtimeAvatarManager>();

        _modeSwitcher = GetComponent<PlayerModeSwitcher>();

        _camera = Camera.main;

        // Notify us when Realtime successfully connects to the room
        //_realtime.didConnectToRoom += DidConnectToRoom;
    }

    private void Update()
    {
        if (_avatar == null && _avatarManager.localAvatar != null)
        {
            _avatar = _avatarManager.localAvatar;

            Debug.Log("Got local avatar");

            GotAvatar();
        }

        if (_avatar != null)
        { 
            if (_avatar.rightHand && rightFollower && !rightFollower.target)
            {
                Debug.Log("Added Follower on Left Hand");
                rightFollower.target = _avatar.rightHand;
            }

            if (_avatar.leftHand && leftFollower && !leftFollower.target)
            {
                Debug.Log("Added Follower on Right Hand");
                leftFollower.target = _avatar.leftHand;
            }
        }
    }

    private void GotAvatar()
    {

        if (_modeSwitcher.isXRMode)
        {
            //set up XR player

            Realtime.InstantiateOptions options = new Realtime.InstantiateOptions();
            options.ownedByClient = true;
            options.preventOwnershipTakeover = true;
            options.destroyWhenOwnerLeaves = true;
            options.destroyWhenLastClientLeaves = true;
            options.useInstance = _realtime;

            if (_avatar.rightHand != null)
            {
                GameObject rightFollowerObject = Realtime.Instantiate("FollowCube", _avatar.rightHand.transform.position, Quaternion.identity, options);
                rightFollower = rightFollowerObject.GetComponent<RigidFollower>();
                rightFollowerObject.GetComponent<RealtimeTransform>().RequestOwnership();
            }

            if (_avatar.leftHand != null)
            {
                GameObject leftFollowerObject = Realtime.Instantiate("FollowCube", _avatar.leftHand.transform.position, Quaternion.identity, options);
                leftFollower = leftFollowerObject.GetComponent<RigidFollower>();
                leftFollowerObject.GetComponent<RealtimeTransform>().RequestOwnership();
            }
        }
        else
        {
            //set up non-XR player

            // Get a reference to the player
            DesktopPlayer player = _avatar.GetComponent<DesktopPlayer>();

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
}