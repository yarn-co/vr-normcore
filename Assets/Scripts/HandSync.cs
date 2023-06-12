using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class HandSync : RealtimeComponent<HandSyncModel>
{
    public HandSyncModel handModel;

    private void Awake()
    {
        // Get a reference to the mesh renderer
        //_meshRenderer = GetComponent<MeshRenderer>();
    }

    protected override void OnRealtimeModelReplaced(HandSyncModel previousModel, HandSyncModel currentModel)
    {
        if (previousModel != null)
        {

        }

        if (currentModel != null)
        {
            handModel = currentModel;
        }
    }

}