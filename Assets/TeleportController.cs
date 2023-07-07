using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportController : MonoBehaviour
{
    public Vector3 respawnPosition;

    private TeleportationProvider teleportationProvider;
    private XRBaseInteractor teleportInteractor;

    void Start()
    {
        teleportationProvider = GetComponent<TeleportationProvider>();

        if (teleportationProvider == null)
        {
            Debug.LogError("No TeleportationProvider component found. Please attach one to the same GameObject.");
            return;
        }

        // Create a new interactor that we'll use for teleporting
        teleportInteractor = new GameObject("TeleportInteractor").AddComponent<XRBaseInteractor>();
    }

    public void RespawnPlayer()
    {
        if (teleportationProvider == null || teleportInteractor == null)
            return;

        // Set up the teleport request
        var request = new TeleportRequest()
        {
            destinationPosition = respawnPosition,
            destinationRotation = Quaternion.identity,
            matchOrientation = MatchOrientation.None
        };

        // Queue the teleport request
        teleportationProvider.QueueTeleportRequest(request);
    }

    void OnDestroy()
    {
        // Destroy the teleport interactor when the component is destroyed
        if (teleportInteractor != null)
            Destroy(teleportInteractor.gameObject);
    }
}