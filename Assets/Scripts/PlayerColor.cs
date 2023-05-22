using Fusion;
using UnityEngine;

public class PlayerColor : NetworkBehaviour
{
    public MeshRenderer MeshRenderer;

    [Networked(OnChanged = nameof(NetworkColorChanged))]
    public Color NetworkedColor { get; set; }

    private static void NetworkColorChanged(Changed<PlayerColor> changed)
    {
        //if (changed.Behaviour.HasStateAuthority)
        //{
            changed.Behaviour.MeshRenderer.material.color = changed.Behaviour.NetworkedColor;
        //}
    }

    public void Start()
    {
        NetworkedColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
    }

    public void Update()
    {
        if(HasStateAuthority == false){
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Changing the material color here directly does not work since this code is only executed on the client pressing the button and not on every client.
            NetworkedColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }
    }
    

}