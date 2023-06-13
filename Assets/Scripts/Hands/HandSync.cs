using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class HandSync : RealtimeComponent<HandSyncModel>
{
    public HandSyncModel handModel;

    protected override void OnRealtimeModelReplaced(HandSyncModel previousModel, HandSyncModel currentModel)
    {
        handModel = currentModel;
    }
}