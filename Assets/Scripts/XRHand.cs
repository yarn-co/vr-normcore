using Fusion.XR.Shared.Rig;
using JetBrains.Annotations;
using Normal.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public enum HandType
{
    Left,
    Right
}

namespace Assets.Scripts
{
    public class XRHand : MonoBehaviour
    {
        public HandType handType;

        public float indexValue;
        public float gripValue;
        public bool primaryTouch;
        public bool secondaryTouch;

        public SkinnedMeshRenderer handMeshRenderer;
        public Animator handAnimator;
        public InputDevice inputDevice;

        [Header("Animation layers and configuration")]
        public bool isRealtimeTrackedVersion = false;
        public string pinchAnimationParameter = "Pinch";
        public string flexAnimationParameter = "Flex";
        public string poseAnimationParameter = "Pose";
        public string pointAnimationParameter = "Point";
        public string pointAnimationLayer = "Point Layer";
        public string thumbAnimationLayer = "Thumb Layer";
        public float maxGripToPinch = 0.05f;

        int pointAnimationLayerIndex = -1;
        int thumbAnimationLayerIndex = -1;
        bool layerIndexFound = false;
        bool foundInputDevice = false;

        public RealtimeView _realtimeView;

        private void Awake()
        {
            if (isRealtimeTrackedVersion)
            {
                _realtimeView = GetComponentInParent<RealtimeView>();
                
            }
        }

        // Use this for initialization
        void Start()
        {
            if (handMeshRenderer == null) handMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            if (handAnimator == null) handAnimator = GetComponentInChildren<Animator>();
            if (handAnimator == null) handAnimator = GetComponentInParent<Animator>();

            if (_realtimeView.isOwnedLocallyInHierarchy)
            {
                inputDevice = GetInputDevice();

                if (isRealtimeTrackedVersion)
                {
                    handMeshRenderer.enabled = false;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {   
            if(!foundInputDevice)
            {
                inputDevice = GetInputDevice();
            }

            if (isRealtimeTrackedVersion)
            {

            }
            else
            {
                AnimateHand();
            }
        }

        InputDevice GetInputDevice()
        {
            InputDeviceCharacteristics controllerCharacteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller;

            if(handType == HandType.Left)
            {
                controllerCharacteristics |= InputDeviceCharacteristics.Left;
            }
            else
            {
                controllerCharacteristics |= InputDeviceCharacteristics.Right;
            }

            List<InputDevice> inputDevices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, inputDevices);

            if (inputDevices.Count > 0)
            {
                foundInputDevice = true;
                return inputDevices[0];
            }

            return new InputDevice();
        }

        void AnimateHand()
        {
            inputDevice.TryGetFeatureValue(CommonUsages.trigger, out indexValue);
            inputDevice.TryGetFeatureValue(CommonUsages.grip, out gripValue);
            inputDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out primaryTouch);
            inputDevice.TryGetFeatureValue(CommonUsages.secondaryTouch, out secondaryTouch);

            if (!layerIndexFound)
            {
                layerIndexFound = true;
                pointAnimationLayerIndex = handAnimator.GetLayerIndex(pointAnimationLayer);
                thumbAnimationLayerIndex = handAnimator.GetLayerIndex(thumbAnimationLayer);
            }

            // Apply layers
            float pointRaisedLayerWeight = 1f - indexValue;
            handAnimator.SetLayerWeight(pointAnimationLayerIndex, pointRaisedLayerWeight);

            float thumbRaisedLayerWeight = 1f;

            if (primaryTouch || secondaryTouch)
            {
                thumbRaisedLayerWeight = 0f;
            }

            handAnimator.SetLayerWeight(thumbAnimationLayerIndex, thumbRaisedLayerWeight);

            float flexParameterValue = gripValue;
            handAnimator.SetFloat(flexAnimationParameter, flexParameterValue);

        }
    }
}