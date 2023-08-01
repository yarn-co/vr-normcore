using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowPointer : MonoBehaviour
{
    public Transform target;  // The target object that the arrow will point towards
    public float distanceFromCamera = 3.0f;
    public float maxAngleFromCenter = 30.0f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Calculate the target direction in world space
        Vector3 targetDirection = target.position - transform.position;

        // Calculate the target direction in camera space
        Vector3 targetDirectionCameraSpace = mainCamera.transform.InverseTransformDirection(targetDirection);

        // Calculate the desired direction in camera space
        Vector3 desiredDirectionCameraSpace = Vector3.RotateTowards(Vector3.forward, targetDirectionCameraSpace, maxAngleFromCenter * Mathf.Deg2Rad, 0.0f);

        // Calculate the desired direction in world space
        Vector3 desiredDirection = mainCamera.transform.TransformDirection(desiredDirectionCameraSpace);

        // Calculate the desired position
        Vector3 desiredPosition = mainCamera.transform.position + desiredDirection.normalized * distanceFromCamera;//3 = distance from camera

        // Update the position and rotation of the arrow
        transform.position = desiredPosition;
        transform.rotation = Quaternion.LookRotation(targetDirection) * Quaternion.Euler(0, -90, 0);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}