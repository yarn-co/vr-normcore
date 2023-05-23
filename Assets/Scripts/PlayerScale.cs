using Fusion;
using UnityEngine;
using Unity.XR.CoreUtils;

public class PlayerScale : NetworkBehaviour
{
    [Networked(OnChanged = nameof(ScaleChanged))]
    public float Scale { get; set; }

    public GameObject Rig;

    public XROrigin Origin;

    private static void ScaleChanged(Changed<PlayerScale> changed)
    {
        PlayerScale behavior = changed.Behaviour;

        NetworkObject player = changed.Behaviour.Object;

        float newScale = behavior.Scale;

        Vector3 scaleVector = new(newScale, newScale, newScale);

        if (behavior.HasStateAuthority)
        {
            behavior.Rig.transform.localScale = scaleVector;
            behavior.Origin.CameraFloorOffsetObject.transform.localPosition = new Vector3(0, behavior.Origin.CameraYOffset * newScale, 0);
            
        }

        player.transform.localScale = scaleVector;
    }

    public void Start()
    {
        Scale = 1f;
        Rig = GameObject.FindGameObjectWithTag("Rig");
        Origin = Rig.GetComponent<XROrigin>();
    }

    public void Update()
    {
        if (HasStateAuthority == false) return;
        
        
        
    }
    

}