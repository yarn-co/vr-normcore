using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;
using Fusion.XR.Shared.Grabbing;

[RequireComponent(typeof(NetworkGrabbable))]
public class GrabbableCube : NetworkBehaviour
{
    public TextMeshProUGUI authorityText;
    public TextMeshProUGUI debugText;

    private void Awake()
    {
        debugText.text = "";
        var grabbable = GetComponent<NetworkGrabbable>();
        grabbable.onDidGrab.AddListener(OnDidGrab);
        grabbable.grabbable.onWillGrab.AddListener(OnWillGrab);
        grabbable.onDidUngrab.AddListener(OnDidUngrab);
    }

    private void DebugLog(string debug)
    {
        debugText.text = debug;
        Debug.Log(debug);
    }

    private void UpdateStatusCanvas()
    {
        if (Object.HasStateAuthority)
            authorityText.text = "You have the state authority on this object";
        else
            authorityText.text = "You have NOT the state authority on this object";
    }

    public override void FixedUpdateNetwork()
    {
        UpdateStatusCanvas();
    }

    void OnDidUngrab()
    {
        Debug.Log($"{gameObject.name} ungrabbed");
    }

    void OnWillGrab(Grabber newGrabber)
    {
        Debug.Log($"Grab on {gameObject.name} requested by {newGrabber}. Waiting for state authority ...");
    }

    void OnDidGrab(NetworkGrabber newGrabber)
    {
        Debug.Log($"{gameObject.name} grabbed by {newGrabber}");
    }
}
