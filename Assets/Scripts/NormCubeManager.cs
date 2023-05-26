using UnityEngine;
using UnityEngine.Animations;
using Normal.Realtime;

public class NormCubeManager : MonoBehaviour
{
    private Realtime _realtime;

    private RealtimeAvatar _avatar;

    private RealtimeAvatarManager _avatarManager;

    private RigidFollower rightFollower;
    private RigidFollower leftFollower;

    private void Awake()
    {
        // Get the Realtime component on this game object
        _realtime = GetComponent<Realtime>();

        _avatarManager = _realtime.GetComponent<RealtimeAvatarManager>();

        // Notify us when Realtime successfully connects to the room
        _realtime.didConnectToRoom += DidConnectToRoom;
    }

    private void Update()
    {
        if(_avatar == null && _avatarManager.localAvatar != null)
        {
            _avatar = _avatarManager.localAvatar;

            Debug.Log("Got local avatar");
        }

        if (_avatar && _avatar.rightHand && rightFollower && !rightFollower.target)
        {
            Debug.Log("Added Follower on Left Hand");
            rightFollower.target = _avatar.rightHand;
        }

        if (_avatar && _avatar.leftHand && leftFollower && !leftFollower.target)
        {
            Debug.Log("Added Follower on Right Hand");
            leftFollower.target = _avatar.leftHand;
        }
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
        GameObject rightFollowerObject = Realtime.Instantiate("FollowCube", new Vector3(0,10,0), Quaternion.identity, options);
        rightFollower = rightFollowerObject.GetComponent<RigidFollower>();
        rightFollowerObject.GetComponent<RealtimeTransform>().RequestOwnership();

        GameObject leftFollowerObject = Realtime.Instantiate("FollowCube", new Vector3(0, 10, 0), Quaternion.identity, options);
        leftFollower = leftFollowerObject.GetComponent<RigidFollower>();
        leftFollowerObject.GetComponent<RealtimeTransform>().RequestOwnership();

    }
}