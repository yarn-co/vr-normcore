using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
public class RotateWheel : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Quaternion lastRotation;
    private IXRSelectInteractor interactor;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        interactor = args.interactorObject;
        lastRotation = interactor.transform.rotation;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        // Logic when releasing the wheel, if necessary
    }

    private void Update()
    {
        if (grabInteractable.isSelected)
        {
            transform.Rotate(Vector3.up * 10f);
            /*
            Quaternion currentRotation = grabInteractable.interactorsSelecting[0].transform.rotation;
            Quaternion deltaRotation = currentRotation * Quaternion.Inverse(lastRotation);
            transform.rotation *= deltaRotation;
            lastRotation = currentRotation;
            */
        }
    }
}