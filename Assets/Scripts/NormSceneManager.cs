using UnityEngine;
using UnityEngine.Animations;
using Normal.Realtime;
using Unity.XR.CoreUtils;
using UnityEngine.XR;
using Cinemachine;

namespace Spacebar.Realtime
{
    public class NormSceneManager : MonoBehaviour
    {
        private Normal.Realtime.Realtime _realtime;

        private RealtimeAvatar _avatar;

        private RealtimeAvatarManager _avatarManager;

        private PlayerModeSwitcher _modeSwitcher;

        private Camera _camera;
        
        private RigidFollower rightFollower;
        private RigidFollower leftFollower;

        public CinemachineFreeLook CMFreeLook;

        public bool userPresent = false;

        private void Awake()
        {
            // Get the Realtime component on this game object
            _realtime = GetComponent<Normal.Realtime.Realtime>();

            _avatarManager = GetComponent<RealtimeAvatarManager>();

            _modeSwitcher = GetComponent<PlayerModeSwitcher>();

            _camera = Camera.main;

            bool present = isVRUserPresent();
        }

        public bool isVRUserPresent()
        {
            //Debug.Log("Detecting VR User Presence..");

            InputDevice headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);

            if (headDevice.isValid == false)
            {
                userPresent = false;
                return false;
            }

            bool presenceFeatureSupported = headDevice.TryGetFeatureValue(CommonUsages.userPresence, out userPresent);

            //Debug.Log(headDevice.isValid + " ** " + presenceFeatureSupported + " ** " + userPresent);

            return userPresent;
        }

        private void Update()
        {
            bool present = isVRUserPresent();

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

                Normal.Realtime.Realtime.InstantiateOptions options = new Normal.Realtime.Realtime.InstantiateOptions();
                options.ownedByClient = true;
                options.preventOwnershipTakeover = true;
                options.destroyWhenOwnerLeaves = true;
                options.destroyWhenLastClientLeaves = true;
                options.useInstance = _realtime;

                //_avatar.leftHand.Find("CustomHandLeft").gameObject.SetActive(false);
                //_avatar.rightHand.Find("CustomHandRight").gameObject.SetActive(false);

                /*
                 * Block Hands!
                 *
                if (_avatar.rightHand != null)
                {
                    GameObject rightFollowerObject = Normal.Realtime.Realtime.Instantiate("FollowCube", _avatar.rightHand.transform.position, Quaternion.identity, options);
                    rightFollower = rightFollowerObject.GetComponent<RigidFollower>();
                    rightFollowerObject.GetComponent<RealtimeTransform>().RequestOwnership();
                }

                if (_avatar.leftHand != null)
                {
                    GameObject leftFollowerObject = Normal.Realtime.Realtime.Instantiate("FollowCube", _avatar.leftHand.transform.position, Quaternion.identity, options);
                    leftFollower = leftFollowerObject.GetComponent<RigidFollower>();
                    leftFollowerObject.GetComponent<RealtimeTransform>().RequestOwnership();
                }
                */

            }
            else
            {
                //set up non-XR player

                // Get a reference to the player
                DesktopPlayer player = _avatar.GetComponent<DesktopPlayer>();

                CMFreeLook.LookAt = _avatar.gameObject.transform;
                CMFreeLook.Follow = _avatar.gameObject.transform;

                // Get the constraint used to position the camera behind the player
                //ParentConstraint cameraConstraint = _camera.GetComponent<ParentConstraint>();

                // Add the camera target so the camera follows it
                //ConstraintSource constraintSource = new ConstraintSource { sourceTransform = player.cameraTarget, weight = 1.0f };
                //int constraintIndex = cameraConstraint.AddSource(constraintSource);

                // Set the camera offset so it acts like a third-person camera.
                //cameraConstraint.SetTranslationOffset(constraintIndex, new Vector3(0.0f, 1.0f, -4.0f));
                //cameraConstraint.SetRotationOffset(constraintIndex, new Vector3(15.0f, 0.0f, 0.0f));
            }


        }
    }
}